using Flunt.Notifications;
using Flunt.Validations;
using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;

public class AuthHandler(IAuthRepository repository) : IRequestHandler<AuthRequest, AuthResponse> {
    
    private readonly IAuthRepository _repository = repository;

    public async Task<AuthResponse> Handle(AuthRequest request, CancellationToken cancellationToken) {
        
        #region RequestValidation

        try {
            Contract<Notification> res = AuthSpecification.Ensure(request);
            if (!res.IsValid)
                return new AuthResponse("Requisição inválida", 400, res.Notifications);
        }
        catch {
            return new AuthResponse("Não foi possível validar sua requisição", 500);
        }

        #endregion

        #region ProfileRecover

        User user;
        try {
            user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (user is null)
                return new AuthResponse("Perfil não encontrado", 404);
        }
        catch (Exception) {
            return new AuthResponse("Não foi possível recuperar seu perfil", 500);
        }

        #endregion

        #region PasswordCheck

        if (!user.Password.Challenge(request.Password))
            return new AuthResponse("Usuário ou senha inválidos", 400);

        #endregion

        #region VerifiedAccountCheck

        try {
            if (!user.Email.Verification.IsActive)
                return new AuthResponse("Conta inativa", 400);
        }
        catch {
            return new AuthResponse("Não foi possível verificar seu perfil", 500);
        }

        #endregion

        #region DataReturn

        try {
            ResponseData data = new()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(r => r.Name).ToArray()
            };

            return new AuthResponse(string.Empty, data);
        }
        catch {
            return new AuthResponse("Não foi possível obter os dados do perfil", 500);
        }

        #endregion
    }
}