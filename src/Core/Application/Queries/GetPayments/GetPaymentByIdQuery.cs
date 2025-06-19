using Application.DTOs;
using MediatR;

namespace Application.Queries.GetPayments;

public class GetPaymentByIdQuery : IRequest<PaymentDto?>
{
    public string PaymentId { get; set; } = string.Empty;
}