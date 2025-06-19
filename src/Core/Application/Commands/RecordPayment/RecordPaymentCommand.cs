using Application.DTOs;
using MediatR;

namespace Application.Commands.RecordPayment;


public class RecordPaymentCommand : IRequest<bool>
{
    public RecordPaymentDto Payment { get; set; } = null!;
}