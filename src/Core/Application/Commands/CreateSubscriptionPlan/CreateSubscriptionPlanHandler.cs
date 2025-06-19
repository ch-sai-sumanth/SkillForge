using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using User.Domain.Entities;

namespace Application.Commands.CreateSubscriptionPlan;

public class CreateSubscriptionPlanHandler : IRequestHandler<CreateSubscriptionPlanCommand, string>
{
    private readonly ISubscriptionPlanService _subscriptionPlanService;
    private readonly IMapper _mapper;

    public CreateSubscriptionPlanHandler(ISubscriptionPlanService subscriptionPlanService, IMapper mapper)
    {
        _subscriptionPlanService = subscriptionPlanService;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {

        var id = await _subscriptionPlanService.CreatePlanAsync(request.Dto);
        return id;
    }
}