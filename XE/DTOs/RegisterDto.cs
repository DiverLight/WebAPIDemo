// RegisterDto.cs
namespace XE.DTOs;

public class RegisterDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    // Optional: You can add validation attributes if needed
    // [Required]
    // [EmailAddress]
    // public string Email { get; set; } = null!;
}