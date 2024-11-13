using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests;

public class UserCreateRequest
{
    [Required]
    [MinLength(3, ErrorMessage = "Username is too small")]
    [MaxLength(64, ErrorMessage = "Username is too long")]
    public string Username { get; set; } = null!;

    [Required]
    [MinLength(3, ErrorMessage = "Password is too small")]
    [MaxLength(64, ErrorMessage = "Password is too long")]
    public string Password { get; set; } = null!;
}
