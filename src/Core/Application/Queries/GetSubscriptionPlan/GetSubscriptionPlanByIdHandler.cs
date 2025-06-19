using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Queries.GetSubscriptionPlan;

public class GetSubscriptionPlanByIdHandler : IRequestHandler<GetSubscriptionPlanByIdQuery, SubscriptionPlanDto?>
{
    private readonly ISubscriptionPlanRepository _repo;
    private readonly IMapper _mapper;

    public GetSubscriptionPlanByIdHandler(ISubscriptionPlanRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<SubscriptionPlanDto?> Handle(GetSubscriptionPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _repo.GetByIdAsync(request.Id);
        return plan == null ? null : _mapper.Map<SubscriptionPlanDto>(plan);
    }
}