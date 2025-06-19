using Application.Interfaces;
using MediatR;

namespace Application.Commands.DeleteAccount;

public class DeleteMyAccountCommandHandler : IRequestHandler<DeleteMyAccountCommand, bool>
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public DeleteMyAccountCommandHandler(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DeleteMyAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var user = await _userService.GetByIdAsync(userId);
        if (user == null) return false;

        // Optional: delete profile picture if exists
        if (!string.IsNullOrWhiteSpace(user.ProfileImagePath))
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImagePath.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        await _userService.DeleteAsync(userId);
        return true;
    }
}