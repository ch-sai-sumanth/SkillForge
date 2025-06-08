using Application.DTOs;
using FluentValidation;

namespace WebApi.Validators;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Old password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\@\!\?\*\.\#\$\%\^\&\-\+]") // You can tweak allowed symbols
            .WithMessage("Password must contain at least one special character.");
    }
}