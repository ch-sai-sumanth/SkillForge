using MediatR;

namespace Application.Commands.UpdateMentorSkills;

public class UpdateMentorSkillsCommand : IRequest<bool>
{
    public string MentorId { get; set; } = string.Empty;
    public List<string> Skills { get; set; } = new();
}