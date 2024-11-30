using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;


namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Authenticate; 

public class AuthRepository(AppDbContext context) : IAuthRepository {

    private readonly AppDbContext _context = context;

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.Users.AsNoTracking().FirstAsync(u => u.Email.Address == email, cancellationToken); 

}
