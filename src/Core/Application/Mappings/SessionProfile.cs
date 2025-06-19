using Application.DTOs;
using AutoMapper;
using Domain;
using User.Domain.Entities;

namespace Application.Mappings;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<Session, SessionDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<SessionStatus>(src.Status)));
    }
}