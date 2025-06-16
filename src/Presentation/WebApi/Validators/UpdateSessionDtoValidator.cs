using Application.DTOs;
using FluentValidation;

namespace WebApi.Validators;

public class UpdateSessionDtoValidator : AbstractValidator<UpdateSessionDto>
{
    public UpdateSessionDtoValidator()
    {
        RuleFor(x => x.Topic).NotEmpty().MaximumLength(100);
        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Start time must be in the future.");
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time.");
    }
}