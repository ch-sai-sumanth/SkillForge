using Application.DTOs;
using MediatR;

namespace Application.Queries.GetUserSubscription;

public class GetUserSubscriptionQuery : IRequest<SubscriptionDto?>
{
    public string UserId { get; set; }

    public GetUserSubscriptionQuery(string userId) => UserId = userId;
}