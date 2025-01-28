namespace Server.Interfaces.Services;

public interface IPasswordEncryptionService
{
    public string Encrypt(string plainText);
    public string Decrypt(string cipherText);
}
