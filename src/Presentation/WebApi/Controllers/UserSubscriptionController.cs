using Application.Commands.CancelSubscription;
using Application.Commands.SubscribeUser;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries.GetUserSubscription;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserSubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserSubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeUser([FromBody] SubscribeUserDto dto)
    {
        await _mediator.Send(new SubscribeUserCommand { Dto = dto });
        return Ok(new { message = "User subscribed successfully." });
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserSubscription(string userId)
    {
        var result = await _mediator.Send(new GetUserSubscriptionQuery(userId));
        return result == null
            ? NotFound(new { message = "Subscription not found." })
            : Ok(result);
    }

    [HttpGet("is-subscribed/{userId}")]
    public async Task<IActionResult> IsUserSubscribed(string userId)
    {
        var result = await _mediator.Send(new IsUserSubscribedQuery(userId));
        return Ok(new { isSubscribed = result });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var result = await _mediator.Send(new GetAllSubscriptionsQuery());
        return Ok(result);
    }

    [HttpPost("cancel/{userId}")]
    public async Task<IActionResult> CancelSubscription(string userId)
    {
        var result = await _mediator.Send(new CancelSubscriptionCommand(userId));
        return !result
            ? BadRequest(new { message = "Subscription could not be cancelled or already expired." })
            : Ok(new { message = "Subscription cancelled successfully." });
    }
}

