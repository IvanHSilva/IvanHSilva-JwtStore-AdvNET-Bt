using JwtStore.Core.Contexts.AccountContext.Entities;

namespace JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;

public interface IService{

    Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken);
}
