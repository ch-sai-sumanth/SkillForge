using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetPayments;

public class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
{
    private readonly IPaymentService _paymentService;

    public GetAllPaymentsQueryHandler(IPaymentService paymentService)
    
    {
        _paymentService = paymentService;
    }

    public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        return await _paymentService.GetAllPaymentsAsync();
    }
}