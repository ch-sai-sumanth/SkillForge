using MediatR;

namespace Application.Commands.DeleteMentorRate;

public class DeleteMentorRateCommand : IRequest<bool>
{
    public string MentorId { get; set; } = string.Empty;
}