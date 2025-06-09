using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserEntity = User.Domain.Entities.User;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService; // Your service to validate users
    private readonly IAuthService _authService; // Your service to handle authentication logic
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService,IAuthService authService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
        _authService = authService;
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

       

        await _authService.CreateUserAsync(registerDto);

        return Ok("User registered successfully");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var authResponse = await _authService.LoginAsync(loginDto.Username, loginDto.Password);
        if (authResponse == null)
            return Unauthorized("Invalid username/email or password");

        return Ok(authResponse); // returns { token, refreshToken }
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        await _authService.LogoutAsync(userId);
        return Ok("Logged out successfully");
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var newTokens = await _authService.RefreshTokenAsync(refreshToken);
        if (newTokens == null)
            return Unauthorized("Invalid or expired refresh token");

        return Ok(newTokens);
    }
}
