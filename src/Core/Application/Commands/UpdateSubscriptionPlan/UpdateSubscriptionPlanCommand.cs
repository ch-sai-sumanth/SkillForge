using Application.DTOs;
using MediatR;

namespace Application.Commands.UpdateSubscriptionPlan;

public record UpdateSubscriptionPlanCommand(string Id, UpdateSubscriptionPlanDto Dto) : IRequest<bool>;
