using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetPayments;

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    private readonly IPaymentService _paymentService;

    public GetPaymentByIdQueryHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<PaymentDto?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _paymentService.GetPaymentByIdAsync(request.PaymentId);
    }
}