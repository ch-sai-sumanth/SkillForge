using Application.DTOs;
using MediatR;

namespace Application.Queries.GetSubscriptionPlan;

public record GetAllSubscriptionPlansQuery : IRequest<List<SubscriptionPlanDto>>;
