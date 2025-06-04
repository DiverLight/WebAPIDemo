using MongoDB.Driver;
using XE.Models;
using XE.Services;
using XE.DTOs;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace XE.Services;

public class UserServices
{
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _config;

    public UserServices(IConfiguration config)
    {
        _config = config;
        var client = new MongoClient(_config.GetConnectionString("MongoDb"));
        var database = client.GetDatabase("XEDB");
        _users = database.GetCollection<User>("Users");

    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        // Kiểm tra confirm password
        if (dto.Password != dto.ConfirmPassword)
            return false; // mật khẩu không khớp

        // Kiểm tra email đã tồn tại
        if (await _users.Find(u => u.Email == dto.Email).AnyAsync())
            return false; // địa chỉ email đã tồn tại

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _users.InsertOneAsync(user);
        return true; // đăng ký thành công
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _users.Find(u => u.Email == dto.Email).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null; // tài khoản hoặc mật khẩu không hợp lệ
        // Generate a JWT token or session ID here
        // For simplicity, returning the user ID as a token
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
           new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Name, user.Username),
       };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}