using Application.DTOs;
using MediatR;

namespace Application.Commands.UpdatePaymentStatus;

public class UpdatePaymentStatusCommand : IRequest<bool>
{
    public string PaymentId { get; set; } = string.Empty;
    public UpdatePaymentStatusDto UpdateDto { get; set; } = null!;
}