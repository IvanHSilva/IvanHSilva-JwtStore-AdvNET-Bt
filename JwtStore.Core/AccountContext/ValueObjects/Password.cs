using JwtStore.Core.SharedContext.ValueObjects;
using System.Security.Cryptography;

namespace JwtStore.Core.AccountContext.ValueObjects;

public class Password : ValueObject {
    
    public static string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string Special = "!@#$%^&*{}[](),.;:-_+-/";

    public string Hash { get; } = GeneratePassword();
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    private static string GeneratePassword(short lenght = 16, bool includeSpecial = true,
        bool upperCase = false) {

        string chars = includeSpecial ? (Valid + Special) : Valid;
        int start = upperCase ? 26 : 0;
        int index = 0;
        var result = new char[lenght];
        var random = new Random();  

        while(index < lenght)
            result[index++] = chars[random.Next(start, chars.Length)];

        return new string(result);
    }

    private static string Hashing(string password, short saltSize = 16,
        short keySize = 32, int iterations = 10000, char splitChar = '.') {
        
        if (string.IsNullOrEmpty(password))
            throw new Exception("Password shouldnt be empty!");

        password += Configuration.Secrets.PasswordSaltKey;

        using Rfc2898DeriveBytes algorithm = new(password, saltSize, 
            iterations, HashAlgorithmName.SHA256);
        string key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        string salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}{splitChar}{salt}{splitChar}{key}";
    }
}
