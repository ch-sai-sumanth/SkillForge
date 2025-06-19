using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Commands.UpdateSubscriptionPlan;

public class UpdateSubscriptionPlanHandler : IRequestHandler<UpdateSubscriptionPlanCommand, bool>
{
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private readonly IMapper _mapper;

    public UpdateSubscriptionPlanHandler(ISubscriptionPlanService subscriptionPlanService, IMapper mapper)
    {
        _subscriptionPlanService = subscriptionPlanService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        return await _subscriptionPlanService.UpdatePlanAsync(request.Id, request.Dto);
    }
}