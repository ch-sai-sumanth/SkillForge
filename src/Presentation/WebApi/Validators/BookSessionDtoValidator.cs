using Application.DTOs;
using FluentValidation;

namespace WebApi.Validators;

public class BookSessionDtoValidator : AbstractValidator<BookSessionDto>
{
    public BookSessionDtoValidator()
    {
        RuleFor(x => x.MentorId).NotEmpty();
        RuleFor(x => x.MenteeId).NotEmpty().NotEqual(x => x.MentorId).WithMessage("Mentor and mentee can't be the same.");
        RuleFor(x => x.StartTime).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
    }
}
