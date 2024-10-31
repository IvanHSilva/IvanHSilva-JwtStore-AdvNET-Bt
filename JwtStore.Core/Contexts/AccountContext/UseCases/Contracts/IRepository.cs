namespace JwtStore.Core.Contexts.AccountContext.UseCases.Contracts;

public interface IRepository{

    Task<bool> AnyAsync(string email, CancellationToken cancellationToken);
}
