namespace Server.Settings;

public class AuthSettings
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int RefreshTokenExpiresTimeInMinutes {  get; set; }
    public int AccessTokenExpiresTimeInMinutes {  get; set; }
}
