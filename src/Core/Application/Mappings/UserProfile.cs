using Application.DTOs;
using AutoMapper;

using UserEntity = User.Domain.Entities.User;

namespace Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>().ReverseMap();
    }
}