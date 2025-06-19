using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetUserSubscription;

public class IsUserSubscribedQueryHandler : IRequestHandler<IsUserSubscribedQuery, bool>
{
    private readonly IUserSubscriptionService _subscriptionService;

    public IsUserSubscribedQueryHandler(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(IsUserSubscribedQuery request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.IsUserSubscribedAsync(request.UserId);
    }
}
