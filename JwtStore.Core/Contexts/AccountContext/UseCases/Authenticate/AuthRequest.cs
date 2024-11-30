using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate; 

public record AuthRequest(string Name, string Email, string Password) : 
    IRequest<AuthResponse>;
