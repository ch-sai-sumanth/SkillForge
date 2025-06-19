using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetUserSubscription;

public class GetAllSubscriptionsQueryHandler : IRequestHandler<GetAllSubscriptionsQuery, IEnumerable<SubscriptionDto>>
{
    private readonly IUserSubscriptionService _subscriptionService;

    public GetAllSubscriptionsQueryHandler(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<IEnumerable<SubscriptionDto>> Handle(GetAllSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetAllSubscriptionsAsync();
    }
}
