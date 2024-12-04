using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate;
using JwtStore.Infra.Contexts.AccountContext.UseCases.Create;
using MediatR;

namespace JwtStore.Api.Extensions;

public static class AccountContextExtension {

    public static void AddAccountContext(this WebApplicationBuilder builder) {

        #region Create

        builder.Services.AddTransient<IRepository, Repository>();

        builder.Services.AddTransient<IService, Service>();

        #endregion

        #region Authenticate

        builder.Services.AddTransient<IAuthRepository, AuthRepository>();

        #endregion    
    }

    public static void MapAccountEndpoints(this WebApplication app) {

        #region Create
        app.MapPost("api/users", async (
            Request request, IRequestHandler<Request, Response> handler) =>
        {
            Response result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess ? Results.Created($"api/users/{result.Data!.Id}", result)
            : Results.Json(result, statusCode: result.Status);
        });
        #endregion

        #region Authenticate
        app.MapPost("api/authenticate", async (
            AuthRequest request, IRequestHandler<AuthRequest, AuthResponse> handler) =>
        {
            AuthResponse result = await handler.Handle(request, new CancellationToken());
            return result.IsSuccess ? Results.Ok(result)
            : Results.Json(result, statusCode: result.Status);
        });
        #endregion
    }
}
