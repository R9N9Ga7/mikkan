namespace Server.Interfaces.Services;

public interface IPasswordHasherService
{
    public string HashPassword(string password);
    public bool VerifyPassword(string hash, string password);
}
