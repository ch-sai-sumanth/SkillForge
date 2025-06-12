using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class SubscriptionPlanProfile : Profile
{
    public SubscriptionPlanProfile()
    {
        CreateMap<CreateSubscriptionPlanDto, SubscriptionPlan>();

        CreateMap<SubscriptionPlanDto, SubscriptionPlan>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Prevent overwriting Id
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Prevent overwriting CreatedAt

        CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
    }
}