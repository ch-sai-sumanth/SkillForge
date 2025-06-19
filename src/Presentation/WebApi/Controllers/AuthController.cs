using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Commands.LogoutUser;
using Application.Commons;
using Application.Commons.LoginUser;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserEntity = User.Domain.Entities.User;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        // Check if username/email already exists
       var result = await _mediator.Send(new RegisterUserCommand
       {
           name=registerDto.Name,
           Username = registerDto.Username,
           Email = registerDto.Email,
           Password = registerDto.Password,
           Role = registerDto.Role,
           Skills = registerDto.Skills,
       });
       
       return result.Contains("successfully")? Ok(result) : BadRequest(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var response = await _mediator.Send(new LoginUserQuery()
        {
            Username = loginDto.Username,
            Password = loginDto.Password
        });
        return response == null 
            ? Unauthorized("Invalid username or password") 
            : Ok(response);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
       var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
         if (string.IsNullOrEmpty(userId))
              return Unauthorized("User ID not found in token");
         
         await _mediator.Send(new LogoutUserCommand{ UserId = userId });
            return Ok("User logged out successfully");
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var result = await _mediator.Send(new RefreshTokenQuery
        {
            RefreshToken = refreshToken
        });
        return result==null?
            Unauthorized("Invalid or expired refresh token") :
            Ok(result);
    }
}
