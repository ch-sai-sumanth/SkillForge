using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class MentorRateProfile : Profile
{
    public MentorRateProfile()
    {
        CreateMap<SetMentorRateDto, MentorRate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<MentorRateDto, MentorRate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<MentorRate, MentorRateDto>();
    }
}