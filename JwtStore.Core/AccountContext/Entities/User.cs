using JwtStore.Core.AccountContext.ValueObjects;
using JwtStore.Core.SharedContext.Entities;

namespace JwtStore.Core.AccountContext.Entities;

public class User : Entity {
    public string Name { get; set; } = string.Empty;
    public Email Email { get; private set; } = null!;
}
