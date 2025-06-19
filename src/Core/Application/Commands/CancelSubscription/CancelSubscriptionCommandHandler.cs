using Application.Interfaces;
using MediatR;

namespace Application.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, bool>
{
    private readonly IUserSubscriptionService _subscriptionService;

    public CancelSubscriptionCommandHandler(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionService.CancelSubscriptionAsync(request.UserId);
    }
}