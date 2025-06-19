using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class UserSubscriptionProfile : Profile
{
    public UserSubscriptionProfile()
    {
        CreateMap<SubscribeUserDto, UserSubscription>();
        CreateMap<UserSubscription, SubscriptionDto>(); }
}