using Application.DTOs;
using MediatR;

namespace Application.Queries.GetPayments;

public class GetPaymentsByMenteeQuery : IRequest<List<PaymentDto>>
{
    public string MenteeId { get; set; } = string.Empty;
}