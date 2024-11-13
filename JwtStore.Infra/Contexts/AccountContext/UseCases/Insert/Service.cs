using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Insert;

public class Service : IService {
    public Task SendVerificationEmailAsync(User user, CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}
