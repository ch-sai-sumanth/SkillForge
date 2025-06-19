using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.UploadProfile;

public class UploadProfilePictureCommand : IRequest<string>
{
    public IFormFile File { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
}