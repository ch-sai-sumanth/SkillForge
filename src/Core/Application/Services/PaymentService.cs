using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Entities;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
    }

    public async Task RecordPaymentAsync(RecordPaymentDto dto)
    {
        var payment = _mapper.Map<Payment>(dto);
        payment.Id = Guid.NewGuid().ToString();
        payment.RecordedAt = DateTime.UtcNow;
        payment.CreatedAt = DateTime.UtcNow;

        await _paymentRepository.CreateAsync(payment);
    }

    public async Task<List<PaymentDto>> GetPaymentsByMenteeAsync(string menteeId)
    {
        var payments = await _paymentRepository.GetPaymentsByMenteeAsync(menteeId);
        return _mapper.Map<List<PaymentDto>>(payments);
    }

    public async Task<PaymentDto?> GetPaymentByIdAsync(string paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        return payment == null ? null : _mapper.Map<PaymentDto>(payment);
    }

    public async Task<List<PaymentDto>> GetAllPaymentsAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();
        return _mapper.Map<List<PaymentDto>>(payments);
    }

    public async Task<bool> UpdatePaymentStatusAsync(string paymentId, UpdatePaymentStatusDto updatePaymentStatusDto)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null) return false;

        // Avoid unnecessary update if status is already the same
        if (payment.Status.Equals(updatePaymentStatusDto.status.ToString(), StringComparison.OrdinalIgnoreCase))
            return false;

        // Update and save
        if (updatePaymentStatusDto.status.Equals(PaymentStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            payment.Status = PaymentStatus.Pending.ToString();
        }
        else if (updatePaymentStatusDto.status.Equals(PaymentStatus.Confirmed.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            payment.Status = PaymentStatus.Confirmed.ToString();
        }
        else if (updatePaymentStatusDto.status.Equals(PaymentStatus.Failed.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            payment.Status = PaymentStatus.Failed.ToString();
        }
        else
        {
            return false; // Invalid status
        }
        payment.Status = updatePaymentStatusDto.status.ToString();
        await _paymentRepository.UpdateAsync(payment);
        return true;
    }
}