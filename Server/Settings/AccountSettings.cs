namespace Server.Settings;

public class AccountSettings
{
    public int SaltSize { get; set; }
    public int KeySize { get; set; }
    public int Iterations { get; set; }
}
