using Application.Commands.CreateSubscriptionPlan;
using Application.Commands.DeleteSubscriptionPlan;
using Application.Commands.UpdateSubscriptionPlan;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetSubscriptionPlan;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionPlanController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionPlanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanDto dto)
    {
        var id = await _mediator.Send(new CreateSubscriptionPlanCommand(dto));
        return Ok(new { message = "Plan created", planId = id });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPlans()
    {
        var plans = await _mediator.Send(new GetAllSubscriptionPlansQuery());
        return Ok(plans);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlanById(string id)
    {
        var plan = await _mediator.Send(new GetSubscriptionPlanByIdQuery(id));
        if (plan == null)
            return NotFound(new { message = "Subscription plan not found." });

        return Ok(plan);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdatePlan([FromRoute] string id, [FromBody] UpdateSubscriptionPlanDto dto)
    {
        var updated = await _mediator.Send(new UpdateSubscriptionPlanCommand(id, dto));
        if (!updated)
            return NotFound(new { message = "Subscription plan not found for update." });

        return Ok(new { message = "Subscription plan updated successfully." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePlan(string id)
    {
        var deleted = await _mediator.Send(new DeleteSubscriptionPlanCommand(id));
        if (!deleted)
            return NotFound(new { message = "Subscription plan not found for deletion." });

        return Ok(new { message = "Subscription plan deleted successfully." });
    }
}
