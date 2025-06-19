using System.Security.Claims;
using Application.Commands.ChangePassword;
using Application.Commands.DeleteAccount;
using Application.Commands.UpdateProfile;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Commands.UploadProfile;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ProfileController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var query = new GetMyProfileQuery();
        var result = await _mediator.Send(query);
        return result != null ? Ok(result) : NotFound("User not found.");
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        var command = new UpdateMyProfileCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var command = new ChangePasswordCommand(dto);
        var result = await _mediator.Send(command);
        return result ? Ok("Password changed successfully.") : BadRequest("Invalid old password.");
    }

    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var command = new DeleteMyAccountCommand();
        var result = await _mediator.Send(command);
        return result ? Ok("Account deleted successfully.") : NotFound("User not found.");
    }
    [HttpPost("upload-profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var imageUrl = await _mediator.Send(new UploadProfilePictureCommand
        {
            File = file,
            UserId = userId
        });

        return Ok(new { imageUrl });
    }
}