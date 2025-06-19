using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetUserSubscription;

public class GetUserSubscriptionQueryHandler : IRequestHandler<GetUserSubscriptionQuery, SubscriptionDto?>
{
    private readonly IUserSubscriptionService _subscriptionService;

    public GetUserSubscriptionQueryHandler(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<SubscriptionDto?> Handle(GetUserSubscriptionQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.GetSubscriptionDtoByUserIdAsync(request.UserId);
    }
}
