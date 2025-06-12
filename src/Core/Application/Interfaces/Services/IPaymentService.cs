using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task RecordPaymentAsync(RecordPaymentDto dto);
    Task<List<PaymentDto>> GetPaymentsByMenteeAsync(string menteeId);
    Task<PaymentDto?> GetPaymentByIdAsync(string paymentId);
    Task<List<PaymentDto>> GetAllPaymentsAsync();
    Task<bool> UpdatePaymentStatusAsync(string paymentId, UpdatePaymentStatusDto newStatus);
}