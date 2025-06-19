using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSubscriptionPlan;

public record GetSubscriptionPlanByIdQuery(string Id) : IRequest<SubscriptionPlanDto?>;
