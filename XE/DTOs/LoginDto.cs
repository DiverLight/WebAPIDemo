// LoginDto.cs
namespace XE.DTOs;

public class LoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    // Optional: You can add validation attributes if needed
    // [Required]
    // [MinLength(3)]
    // public string Username { get; set; } = null!;
}