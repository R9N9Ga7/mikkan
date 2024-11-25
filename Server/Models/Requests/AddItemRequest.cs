using System.ComponentModel.DataAnnotations;

namespace Server.Models.Requests;

public class AddItemRequest
{
    [Required]
    [MaxLength(256, ErrorMessage = "Name is too long")]
    public string Name { get; set; } = null!;

    [MaxLength(256, ErrorMessage = "Login is too long")]
    public string Login {  get; set; } = string.Empty;

    [MaxLength(256, ErrorMessage = "Password is too long")]
    public string Password { get; set; } = string.Empty;
}
