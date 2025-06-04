using System.ComponentModel.DataAnnotations;


// LoginDto.cs
namespace XE.DTOs;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Email phải có định dạng @gmail.com")]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

}