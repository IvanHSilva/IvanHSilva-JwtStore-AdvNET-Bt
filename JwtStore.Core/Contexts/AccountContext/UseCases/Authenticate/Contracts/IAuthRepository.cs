using JwtStore.Core.Contexts.AccountContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;

public interface IAuthRepository {

    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
}
