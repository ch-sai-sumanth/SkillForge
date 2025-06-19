using MediatR;

namespace Application.Commands.DeleteSubscriptionPlan;

public record DeleteSubscriptionPlanCommand(string Id) : IRequest<bool>;
