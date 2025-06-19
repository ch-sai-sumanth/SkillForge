using Application.DTOs;
using MediatR;

namespace Application.Commands.SetMentorRate;

public class SetMentorRateCommand : IRequest<bool>
{
    public MentorRateDto MentorRate { get; set; } = new();
}