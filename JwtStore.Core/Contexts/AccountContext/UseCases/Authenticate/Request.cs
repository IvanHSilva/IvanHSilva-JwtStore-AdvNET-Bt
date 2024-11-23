using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate; 

public record Request(string Name, string Email, string Password) : 
    IRequest<Response>;
