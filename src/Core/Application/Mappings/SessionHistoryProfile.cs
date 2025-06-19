using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class SessionHistoryProfile : Profile
{
    public SessionHistoryProfile()
    {
        CreateMap<SessionHistory, SessionHistoryDto>()
            .ForMember(dest => dest.OldStatus, opt => opt.MapFrom(src => src.OldStatus.ToString()))
            .ForMember(dest => dest.NewStatus, opt => opt.MapFrom(src => src.NewStatus.ToString()));
    }
}