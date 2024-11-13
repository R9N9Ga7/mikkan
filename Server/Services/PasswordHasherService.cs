using Microsoft.Extensions.Options;
using Server.Interfaces.Services;
using Server.Settings;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace Server.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public PasswordHasherService(IOptions<AccountSettings> options)
    {
        _accountSettings = options.Value;
    }

    public string HashPassword(string password)
    {
        var salt = GenerateSalt();
        var key = GenerateKey(password, salt);

        var saltHash = Convert.ToBase64String(salt);
        var keyHash = Convert.ToBase64String(key);

        return $"{saltHash}{SplitSeparator}{keyHash}";
    }

    public bool VerifyPassword(string hash, string password)
    {
        var parts = hash.Split(SplitSeparator);

        if (parts.Length < 2)
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[0]);
        var key = Convert.FromBase64String(parts[1]);

        var keyToCheck = GenerateKey(password, salt);
        var isEqual = keyToCheck.SequenceEqual(key);

        return isEqual;
    }

    byte[] GenerateSalt()
    {
        var salt = new byte[_accountSettings.SaltSize];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return salt;
    }

    byte[] GenerateKey(string password, byte[] salt)
    {
        using var alg = new Rfc2898DeriveBytes(
            password, salt, _accountSettings.Iterations, HashAlgorithmName.SHA256);
        var key = alg.GetBytes(_accountSettings.KeySize);
        return key;
    }

    const char SplitSeparator = ':';

    readonly AccountSettings _accountSettings;
}
