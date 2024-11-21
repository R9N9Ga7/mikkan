using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests;

public class UserTokensRequest
{
    [Required]
    public string AccessToken { get; set; } = null!;

    [Required]
    public string RefreshToken { get; set; } = null!;
}
