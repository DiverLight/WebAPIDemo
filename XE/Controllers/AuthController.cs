using Microsoft.AspNetCore.Mvc;
using XE.Services;
using XE.DTOs;

namespace XE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserServices _userService;

    public AuthController(UserServices userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
       var success = await _userService.RegisterAsync(registerDto);
        if (!success)
            return BadRequest("Registration failed. Email may already be in use.");
        return Ok("Registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto);
        if (token == null)
            return Unauthorized("Invalid email or password.");
        return Ok(new { Token = token });
    }
}