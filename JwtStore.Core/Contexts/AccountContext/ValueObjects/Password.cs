using JwtStore.Core.Contexts.SharedContext.ValueObjects;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace JwtStore.Core.Contexts.AccountContext.ValueObjects;

public class Password : ValueObject
{

    public static string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string Special = "!@#$%^&*{}[](),.;:-_+-/";

    public string Hash { get; } = GeneratePassword();
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    protected Password() { }

    public Password(string? text = null)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            text = GeneratePassword();

        Hash = Hashing(text);
    }

    private static string GeneratePassword(short lenght = 16, bool includeSpecial = true,
        bool upperCase = false)
    {

        string chars = includeSpecial ? Valid + Special : Valid;
        int start = upperCase ? 26 : 0;
        int index = 0;
        var result = new char[lenght];
        var random = new Random();

        while (index < lenght)
            result[index++] = chars[random.Next(start, chars.Length)];

        return new string(result);
    }

    private static string Hashing(string password, short saltSize = 16,
        short keySize = 32, int iterations = 10000, char splitChar = '.')
    {

        if (string.IsNullOrEmpty(password))
            throw new Exception("Password shouldnt be empty!");

        password += Configuration.Secrets.PasswordSaltKey;

        using Rfc2898DeriveBytes algorithm = new(password, saltSize,
            iterations, HashAlgorithmName.SHA256);
        string key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        string salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}{splitChar}{salt}{splitChar}{key}";
    }

    private static bool Verify(string hash, string password, short keySize = 32,
        int iterations = 10000, char splitChar = '.')
    {

        password += Configuration.Secrets.PasswordSaltKey;

        string[] parts = hash.Split(splitChar, 3);

        if (parts.Length != 3) return false;

        int hashIterations = Convert.ToInt32(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] key = Convert.FromBase64String(parts[2]);

        if (hashIterations != iterations) return false;

        using Rfc2898DeriveBytes algorithm = new(password, salt,
    iterations, HashAlgorithmName.SHA256);
        byte[] keyToCheck = algorithm.GetBytes(keySize);

        return keyToCheck.SequenceEqual(key);
    }
}
