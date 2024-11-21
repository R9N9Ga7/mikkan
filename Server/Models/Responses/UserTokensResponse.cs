namespace Server.Models.Responses;

public class UserTokensResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
