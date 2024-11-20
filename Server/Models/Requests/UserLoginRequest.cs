using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests;

public class UserLoginRequest
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
