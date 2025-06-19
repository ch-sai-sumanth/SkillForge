using Application.Interfaces;
using MediatR;

namespace Application.Commands.SubscribeUser;

public class SubscribeUserCommandHandler : IRequestHandler<SubscribeUserCommand,bool>
{
    private readonly IUserSubscriptionService _subscriptionService;

    public SubscribeUserCommandHandler(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> Handle(SubscribeUserCommand request, CancellationToken cancellationToken)
    {
        await _subscriptionService.SubscribeUserAsync(request.Dto);
        return true;
    }
}