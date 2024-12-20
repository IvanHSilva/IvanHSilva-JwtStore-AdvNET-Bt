﻿using JwtStore.Core.Contexts.AccountContext.Entities;
using JwtStore.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using JwtStore.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtStore.Infra.Contexts.AccountContext.UseCases.Create;

public class Repository(AppDbContext context) : IRepository {

    private readonly AppDbContext _context = context;

    public async Task<bool> AnyAsync(string email, CancellationToken cancellationToken)
        => await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email, cancellationToken);

    public async Task SaveAsync(User user, CancellationToken cancellationToken) {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
