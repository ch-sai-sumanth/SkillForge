using MediatR;

namespace Application.Queries.GetUserSubscription;

public class IsUserSubscribedQuery : IRequest<bool>
{
    public string UserId { get; set; }

    public IsUserSubscribedQuery(string userId) => UserId = userId;
}
