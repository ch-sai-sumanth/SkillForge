using Application.DTOs;
using MediatR;

namespace Application.Queries.GetPayments;

public class GetAllPaymentsQuery : IRequest<List<PaymentDto>>
{
}