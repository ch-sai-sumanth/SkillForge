using Application.Interfaces;
using MediatR;

namespace Application.Commands.SetMentorRate;

public class SetMentorRateCommandHandler : IRequestHandler<SetMentorRateCommand, bool>
{
    private readonly IMentorRateService _mentorRateService;

    public SetMentorRateCommandHandler(IMentorRateService mentorRateService)
    {
        _mentorRateService = mentorRateService;
    }

    public async Task<bool> Handle(SetMentorRateCommand request, CancellationToken cancellationToken)
    {
        await _mentorRateService.SetMentorRateAsync(request.MentorRate);
        return true;
    }
}