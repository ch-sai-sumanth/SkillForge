using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetMentorRate;

public class GetMentorRateQueryHandler : IRequestHandler<GetMentorRateQuery, MentorRateDto?>
{
    private readonly IMentorRateService _mentorRateService;

    public GetMentorRateQueryHandler(IMentorRateService mentorRateService)
    {
        _mentorRateService = mentorRateService;
    }

    public async Task<MentorRateDto?> Handle(GetMentorRateQuery request, CancellationToken cancellationToken)
    {
        return await _mentorRateService.GetMentorRateAsync(request.MentorId);
    }
}