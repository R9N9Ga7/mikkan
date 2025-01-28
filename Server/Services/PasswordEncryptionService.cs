using Microsoft.Extensions.Options;
using Server.Interfaces.Services;
using Server.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services;

public class PasswordEncryptionService : IPasswordEncryptionService
{
    public PasswordEncryptionService(IOptions<AccountSettings> options)
    {
        var encryptionKey = options.Value.EncryptionKey;

        if (string.IsNullOrEmpty(encryptionKey))
        {
            throw new ArgumentException("Key must not be empty");
        }

        _key = Encoding.UTF8.GetBytes(encryptionKey);
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentException("Password must not be empty");
        }

        var (cipher, iv) = EncryptStringToBytes_Aes(plainText, _key);

        var base64Cipher = Convert.ToBase64String(cipher);
        var base64IV = Convert.ToBase64String(iv);

        return $"{base64Cipher}{SplitSeparator}{base64IV}";
    }

    public string Decrypt(string cipherText)
    {
        var parts = cipherText.Split(SplitSeparator);

        if (parts.Length < 2)
        {
            throw new ArgumentException("Wrong cipherPassword");
        }

        var cipher = Convert.FromBase64String(parts[0]);
        var iv = Convert.FromBase64String(parts[1]);

        return DecryptStringFromBytes_Aes(cipher, _key, iv);
    }

    static (byte[] encrypted, byte[] IV) EncryptStringToBytes_Aes(string plainText, byte[] Key)
    {
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        using Aes aesAlg = Aes.Create();

        aesAlg.GenerateIV();
        aesAlg.Key = Key;

        using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        csEncrypt.Write(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
        csEncrypt.FlushFinalBlock();

        var encrypted = msEncrypt.ToArray();
        return (encrypted, aesAlg.IV);
    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException(nameof(IV));
        }

        using Aes aesAlg = Aes.Create();

        aesAlg.Key = Key;
        aesAlg.IV = IV;

        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(cipherText);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }

    readonly byte[] _key;

    const string SplitSeparator = ".";
}
