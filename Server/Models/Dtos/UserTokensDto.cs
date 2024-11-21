namespace Server.Models.Dtos;

public class UserTokensDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
