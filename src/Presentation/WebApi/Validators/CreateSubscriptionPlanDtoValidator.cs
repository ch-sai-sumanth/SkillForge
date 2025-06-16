using Application.DTOs;
using FluentValidation;

namespace WebApi.Validators;

public class CreateSubscriptionPlanDtoValidator : AbstractValidator<CreateSubscriptionPlanDto>
{
    public CreateSubscriptionPlanDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.DurationInDays).GreaterThan(0);
        RuleFor(x => x.MaxSessions).GreaterThan(0);
    }
}