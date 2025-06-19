using Application.Interfaces;
using MediatR;

namespace Application.Commands.RecordPayment;

public class RecordPaymentCommandHandler : IRequestHandler<RecordPaymentCommand, bool>
{
    private readonly IPaymentService _paymentService;

    public RecordPaymentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<bool> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
    {
        await _paymentService.RecordPaymentAsync(request.Payment);
        return true;
    }
}