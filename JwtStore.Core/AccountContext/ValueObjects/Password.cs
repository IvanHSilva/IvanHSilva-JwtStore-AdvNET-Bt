using JwtStore.Core.SharedContext.ValueObjects;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject {
    
    public static string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string Special = "!@#$%^&*{}[](),.;:-_+-/";

    public string Hash { get; } = GeneratePassword();
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    private static string GeneratePassword(short lenght = 16, bool includeSpecial = true,
        
        bool upperCase = false) {
        var chars = includeSpecial ? (Valid + Special) : Valid;
        int start = upperCase ? 26 : 0;
        int index = 0;
        var result = new char[lenght];
        var random = new Random();  

        while(index < lenght)
            result[index++] = chars[random.Next(start, chars.Length)];

        return new string(result);
    }
}
