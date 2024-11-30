using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Infra.Data;

namespace JwtStore.Core.Contexts.SharedContext.UseCases.Authenticate {
    public class Repository(AppDbContext context) : IRepository {

        private readonly AppDbContext _context = context;

        public Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken) {
            return Task.FromResult<User>(_context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email, cancellationToken)); 
        }
    }
}
