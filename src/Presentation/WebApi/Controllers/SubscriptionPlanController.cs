using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionPlanController : ControllerBase
{
    private readonly ISubscriptionPlanService _subscriptionPlanService;

    public SubscriptionPlanController(ISubscriptionPlanService subscriptionPlanService)
    {
        _subscriptionPlanService = subscriptionPlanService;
    }

    // POST: api/subscriptionplan/create
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlan([FromBody] CreateSubscriptionPlanDto dto)
    {
        var id = await _subscriptionPlanService.CreatePlanAsync(dto);
        return Ok(new { message = "Plan created", planId = id });

    }

    // GET: api/subscriptionplan/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPlans()
    {
        var plans = await _subscriptionPlanService.GetAllPlansAsync();
        return Ok(plans);
    }

    // GET: api/subscriptionplan/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlanById(string id)
    {
        var plan = await _subscriptionPlanService.GetPlanByIdAsync(id);
        if (plan == null)
            return NotFound(new { message = "Subscription plan not found." });

        return Ok(plan);
    }

    // PUT: api/subscriptionplan/update/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdatePlan([FromRoute] string id, [FromBody] UpdateSubscriptionPlanDto dto)
    {
        var updated = await _subscriptionPlanService.UpdatePlanAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Subscription plan not found for update." });

        return Ok(new { message = "Subscription plan updated successfully." });
    }

    // DELETE: api/subscriptionplan/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePlan(string id)
    {
        var deleted = await _subscriptionPlanService.DeletePlanAsync(id);
        if (!deleted)
            return NotFound(new { message = "Subscription plan not found for deletion." });

        return Ok(new { message = "Subscription plan deleted successfully." });
    }
}
