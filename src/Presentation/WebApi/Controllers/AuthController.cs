using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserEntity = User.Domain.Entities.User;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService; // Your service to validate users
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        // Check if username/email already exists
        var userByUsername = await _userService.GetByUsernameAsync(registerDto.Username);
        if (userByUsername != null)
            return BadRequest("Username is already taken.");

        var userByEmail = await _userService.GetByEmailAsync(registerDto.Email);
        if (userByEmail != null)
            return BadRequest("An account with this email already exists.");

       

        await _userService.CreateUserAsync(registerDto);

        return Ok("User registered successfully");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        UserDto user = null;
        string input = loginDto.Username; // This could be username or email

        if (IsValidEmail(input))
        {
            // Input is email - fetch user by email first
            var userByEmail = await _userService.GetByEmailAsync(input);
            if (userByEmail != null)
            {
                user = await _userService.ValidateUserAsync(userByEmail.Username, loginDto.Password);
            }
        }
        else
        {
            // Input is username - validate directly
            user = await _userService.ValidateUserAsync(input, loginDto.Password);
        }

        if (user == null)
        {
            return Unauthorized("Invalid username/email or password");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(UserDto user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private bool IsValidEmail(string input)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(input);
            return addr.Address == input;
        }
        catch
        {
            return false;
        }
    }
}
