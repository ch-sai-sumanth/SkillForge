using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // POST: api/payment/record
    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment([FromBody] RecordPaymentDto dto)
    {
        await _paymentService.RecordPaymentAsync(dto);
        return Ok(new { message = "Payment recorded successfully." });
    }

    // GET: api/payment/mentee/{menteeId}
    [HttpGet("mentee/{menteeId}")]
    public async Task<IActionResult> GetPaymentsByMentee(string menteeId)
    {
        var payments = await _paymentService.GetPaymentsByMenteeAsync(menteeId);
        return Ok(payments);
    }

    // GET: api/payment/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(string id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
            return NotFound(new { message = "Payment not found." });

        return Ok(payment);
    }

    // GET: api/payment/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPayments()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(payments);
    }

    // PUT: api/payment/update-status/{id}
    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdatePaymentStatus(string id, [FromQuery] UpdatePaymentStatusDto updatePaymentStatusDto)
    {
        var updated = await _paymentService.UpdatePaymentStatusAsync(id, updatePaymentStatusDto);
        if (!updated)
            return NotFound(new { message = "Payment not found or status update failed." });

        return Ok(new { message = "Payment status updated successfully." });
    }
}