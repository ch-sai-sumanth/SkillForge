using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class UserSubscriptionProfile : Profile
{
    public UserSubscriptionProfile()
    {
        // Mapping from input DTO to domain entity for creating subscriptions
        CreateMap<SubscribeUserDto, UserSubscription>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SubscribedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore());

        // Mapping from entity to output DTO for querying subscriptions
        CreateMap<UserSubscription, UserSubscriptionDto>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow <= src.ExpiresAt));
    }
}