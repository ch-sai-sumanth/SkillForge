using Application.DTOs;
using MediatR;

namespace Application.Queries.GetMentorRate;

public class GetMentorRateQuery : IRequest<MentorRateDto?>
{
    public string MentorId { get; set; } = string.Empty;
}