using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;

namespace Application.Mappings;

public class LearningGoalProfile : Profile
{
    public LearningGoalProfile()
    {
        // Entity to DTO mapping
        CreateMap<LearningGoal, GoalDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.MenteeId, opt => opt.MapFrom(src => src.MenteeId.Value))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.TargetDate, opt => opt.MapFrom(src => src.TargetDate.Value))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToFriendlyString()))
            .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => src.Progress.Percentage))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
            .ForMember(dest => dest.IsDueToday, opt => opt.MapFrom(src => src.IsDueToday))
            .ForMember(dest => dest.IsDueSoon, opt => opt.MapFrom(src => src.IsDueSoon(7)))
            .ForMember(dest => dest.DaysRemaining, opt => opt.MapFrom(src => src.DaysRemaining))
            .ForMember(dest => dest.CanBeModified, opt => opt.MapFrom(src => src.CanBeModified));

        // DTO to Value Objects mappings (for creation/updates)
        CreateMap<CreateGoalDto, LearningGoal>()
            .ConvertUsing((src, dest, context) =>
            {
                var menteeId = UserId.Create(src.MenteeId);
                var title = GoalTitle.Create(src.Title);
                var description = GoalDescription.Create(src.Description);
                var targetDate = TargetDate.Create(src.TargetDate);

                return LearningGoal.Create(menteeId, title, description, targetDate);
            });

        CreateMap<UpdateGoalDto, (GoalTitle Title, GoalDescription Description)>()
            .ConvertUsing(src => (
                GoalTitle.Create(src.Title),
                GoalDescription.Create(src.Description)
            ));
    }
}