using Application.Commands.RecordPayment;
using Application.Commands.UpdatePaymentStatus;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetPayments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment([FromBody] RecordPaymentDto dto)
    {
        var command = new RecordPaymentCommand { Payment = dto };
        var result = await _mediator.Send(command);
        return result ? Ok(new { message = "Payment recorded successfully." }) : BadRequest();
    }

    [HttpGet("mentee/{menteeId}")]
    public async Task<IActionResult> GetPaymentsByMentee(string menteeId)
    {
        var result = await _mediator.Send(new GetPaymentsByMenteeQuery { MenteeId = menteeId });
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(string id)
    {
        var result = await _mediator.Send(new GetPaymentByIdQuery { PaymentId = id });
        return result is not null ? Ok(result) : NotFound(new { message = "Payment not found." });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _mediator.Send(new GetAllPaymentsQuery());
        return Ok(result);
    }

    [HttpPut("update-status/{id}")]
    public async Task<IActionResult> UpdatePaymentStatus(string id, [FromQuery] UpdatePaymentStatusDto updateDto)
    {
        var result = await _mediator.Send(new UpdatePaymentStatusCommand
        {
            PaymentId = id,
            UpdateDto = updateDto
        });

        return result
            ? Ok(new { message = "Payment status updated successfully." })
            : NotFound(new { message = "Payment not found or status update failed." });
    }
}