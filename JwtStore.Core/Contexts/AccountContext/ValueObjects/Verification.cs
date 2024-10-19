namespace JwtStore.Core.Contexts.AccountContext.ValueObjects;

public class Verification
{

    public string Code { get; } = Guid.NewGuid().ToString("N")[..6].ToUpper();
    public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddMinutes(5);
    public DateTime VerifiedAt { get; private set; } = DateTime.UtcNow;
    public bool IsActive => VerifiedAt < ExpiresAt;

    public Verification() { }

    public void Verify(string code)
    {
        if (IsActive) throw new Exception("Este item já foi ativado");
        if (ExpiresAt < DateTime.UtcNow) throw new Exception("Código expirado");
        if (!string.Equals(code.Trim(), Code.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new Exception("Código de verificação inválido");

        ExpiresAt = DateTime.UtcNow.AddMinutes(150);
    }
}
