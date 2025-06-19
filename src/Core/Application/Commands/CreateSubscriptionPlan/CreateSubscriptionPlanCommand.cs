using Application.DTOs;
using MediatR;

namespace Application.Commands.CreateSubscriptionPlan;

public record CreateSubscriptionPlanCommand(CreateSubscriptionPlanDto Dto) : IRequest<string>;
