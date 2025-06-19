using Application.Interfaces;
using MediatR;

namespace Application.Commands.UpdatePaymentStatus;

public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, bool>
{
    private readonly IPaymentService _paymentService;

    public UpdatePaymentStatusCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<bool> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.UpdatePaymentStatusAsync(request.PaymentId, request.UpdateDto);
    }
}