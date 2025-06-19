using Application.DTOs;
using MediatR;

namespace Application.Queries.GetUserSubscription;

public class GetAllSubscriptionsQuery : IRequest<IEnumerable<SubscriptionDto>> { }
