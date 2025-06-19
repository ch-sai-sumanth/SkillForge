using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetSubscriptionPlan;

public class GetAllSubscriptionPlansHandler : IRequestHandler<GetAllSubscriptionPlansQuery, List<SubscriptionPlanDto>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IMapper _mapper;

    public GetAllSubscriptionPlansHandler(ISubscriptionPlanRepository repo, IMapper mapper)
    {
        _subscriptionPlanRepository = repo;
        _mapper = mapper;
    }

    public async Task<List<SubscriptionPlanDto>> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _subscriptionPlanRepository.GetAllPlansAsync();
        return _mapper.Map<List<SubscriptionPlanDto>>(plans);
    }
}