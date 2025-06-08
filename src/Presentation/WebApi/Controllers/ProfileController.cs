using System.Security.Claims;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/profile/me
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userService.GetByIdAsync(userId);
        return user != null ? Ok(user) : NotFound("User not found.");
    }

    // PUT: api/profile/me
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _userService.UpdateProfileAsync(userId, dto);
        return Ok(result);
    }
    
    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _userService.ChangePasswordAsync(userId, dto);

        if (!result)
            return BadRequest("Invalid old password.");

        return Ok("Password changed successfully.");
    }
    
    [HttpPost("upload-profile-picture")]
// [Authorize]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("Only JPG, JPEG, and PNG files are allowed.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
            return NotFound("User not found.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        //  Delete old image if exists
        if (!string.IsNullOrWhiteSpace(user.ProfileImagePath))
        {
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImagePath.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        // 💾 Save new image
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update user with new image path
        var relativePath = $"/uploads/{fileName}";
        user.ProfileImagePath = relativePath;
        await _userService.UpdateAsync(user);

        // ✅ Return full image URL
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var fullImageUrl = $"{baseUrl}{relativePath}";

        return Ok(new { imageUrl = fullImageUrl });
    }
    [HttpDelete("delete-account")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
            return NotFound("User not found.");

        // 🧹 Delete profile image from server if it exists
        if (!string.IsNullOrWhiteSpace(user.ProfileImagePath))
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImagePath.TrimStart('/'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        // 🗑️ Delete user from database
        await _userService.DeleteAsync(userId);

        return Ok("Account deleted successfully.");
    }



}