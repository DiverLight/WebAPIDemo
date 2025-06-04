// RegisterDto.cs
using System.ComponentModel.DataAnnotations;

namespace XE.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Email phải có định dạng @gmail.com")]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;
}