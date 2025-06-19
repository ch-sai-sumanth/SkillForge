using MediatR;

namespace Application.Commands.CancelSubscription;

public class CancelSubscriptionCommand : IRequest<bool>
{
    public string UserId { get; set; }

    public CancelSubscriptionCommand(string userId)
    {
        UserId = userId;
    }
}