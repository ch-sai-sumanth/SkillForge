using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class LearningTaskProfile : Profile
{
    public LearningTaskProfile()
    {
        CreateMap<LearningTask,TaskDto>();
    }
}