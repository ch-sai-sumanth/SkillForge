using Application.DTOs;
using FluentValidation;

namespace WebApi.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");

        RuleFor(u => u.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(role => role == "Mentor" || role == "Mentee")
            .WithMessage("Role must be either 'Mentor' or 'Mentee'");
        
        RuleForEach(u => u.Skills)
            .NotEmpty().WithMessage("Skills cannot contain empty entries");
    }
}