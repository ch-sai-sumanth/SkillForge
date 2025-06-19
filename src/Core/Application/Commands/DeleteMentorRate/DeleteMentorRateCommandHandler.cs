using Application.Interfaces;
using MediatR;

namespace Application.Commands.DeleteMentorRate;

public class DeleteMentorRateCommandHandler : IRequestHandler<DeleteMentorRateCommand, bool>
{
    private readonly IMentorRateService _mentorRateService;

    public DeleteMentorRateCommandHandler(IMentorRateService mentorRateService)
    {
        _mentorRateService = mentorRateService;
    }

    public async Task<bool> Handle(DeleteMentorRateCommand request, CancellationToken cancellationToken)
    {
        return await _mentorRateService.DeleteMentorRateAsync(request.MentorId);
    }
}