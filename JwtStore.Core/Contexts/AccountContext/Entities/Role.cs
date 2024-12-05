﻿namespace JwtStore.Core.Contexts.AccountContext.Entities;

public class Role {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public IEnumerable<User> Users { get; set; } = [];
}
