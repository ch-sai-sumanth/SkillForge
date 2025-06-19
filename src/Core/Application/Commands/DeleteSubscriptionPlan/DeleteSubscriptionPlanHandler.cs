using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Commands.DeleteSubscriptionPlan;

public class DeleteSubscriptionPlanHandler : IRequestHandler<DeleteSubscriptionPlanCommand, bool>
{
    private readonly ISubscriptionPlanRepository _repo;

    public DeleteSubscriptionPlanHandler(ISubscriptionPlanRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        return await _repo.DeleteAsync(request.Id);
    }
}