using Application.DTOs;
using MediatR;

namespace Application.Commands.MentorAvailability;

public class SetMentorAvailabilityCommand : IRequest<bool>
{
    public string MentorId { get; set; } = string.Empty;
    public List<AvailabilityDto> Availabilities { get; set; } = new();
}