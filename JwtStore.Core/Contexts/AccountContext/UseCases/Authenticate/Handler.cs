using Flunt.Notifications;
using Flunt.Validations;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class Handler(IRepository repository) : IRequestHandler<Request, Response> {
    
    private readonly IRepository _repository = repository;

    public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
        
        #region RequestValidation

        try {
            Contract<Notification> res = Specification.Ensure(request);
            if (!res.IsValid)
                return new Response("Requisição inválida", 400, res.Notifications);
        }
        catch {
            return new Response("Não foi possível validar sua requisição", 500);
        }

        #endregion

        #region ProfileRecover

        User user;
        try {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return new Response("Perfil não encontrado", 404);
        }
        catch (Exception) {
            return new Response("Não foi possível recuperar seu perfil", 500);
        }

        #endregion

        #region PasswordCheck

        if (!user.Password.Challenge(request.Password))
            return new Response("Usuário ou senha inválidos", 400);

        #endregion

        #region VerifiedAccountCheck

        try {
            if (!user.Email.Verification.IsActive)
                return new Response("Conta inativa", 400);
        }
        catch {
            return new Response("Não foi possível verificar seu perfil", 500);
        }

        #endregion

        #region DataReturn

        try {
            ResponseData data = new()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = []
            };

            return new Response(string.Empty, data);
        }
        catch {
            return new Response("Não foi possível obter os dados do perfil", 500);
        }

        #endregion
    }
}