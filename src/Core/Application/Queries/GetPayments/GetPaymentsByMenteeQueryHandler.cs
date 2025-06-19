using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetPayments;


public class GetPaymentsByMenteeQueryHandler : IRequestHandler<GetPaymentsByMenteeQuery, List<PaymentDto>>
{
    private readonly IPaymentService _paymentService;

    public GetPaymentsByMenteeQueryHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<List<PaymentDto>> Handle(GetPaymentsByMenteeQuery request, CancellationToken cancellationToken)
    {
        return await _paymentService.GetPaymentsByMenteeAsync(request.MenteeId);
    }
}