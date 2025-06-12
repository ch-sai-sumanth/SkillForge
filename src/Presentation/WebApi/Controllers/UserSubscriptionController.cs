using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserSubscriptionController : ControllerBase
{
    private readonly IUserSubscriptionService _subscriptionService;

    public UserSubscriptionController(IUserSubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    // POST: api/userSubscription/subscribe
    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeUser([FromBody] SubscribeUserDto dto)
    {
        await _subscriptionService.SubscribeUserAsync(dto);
        return Ok(new { message = "User subscribed successfully." });
    }

    // GET: api/userSubscription/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserSubscription(string userId)
    {
        var subscription = await _subscriptionService.GetSubscriptionDtoByUserIdAsync(userId);
        if (subscription == null)
            return NotFound(new { message = "Subscription not found." });

        return Ok(subscription);
    }

    // GET: api/userSubscription/is-subscribed/{userId}
    [HttpGet("is-subscribed/{userId}")]
    public async Task<IActionResult> IsUserSubscribed(string userId)
    {
        var isSubscribed = await _subscriptionService.IsUserSubscribedAsync(userId);
        return Ok(new { isSubscribed });
    }

    // GET: api/userSubscription/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
        return Ok(subscriptions);
    }

    // POST: api/userSubscription/cancel/{userId}
    [HttpPost("cancel/{userId}")]
    public async Task<IActionResult> CancelSubscription(string userId)
    {
        var result = await _subscriptionService.CancelSubscriptionAsync(userId);
        if (!result)
            return BadRequest(new { message = "Subscription could not be cancelled or already expired." });

        return Ok(new { message = "Subscription cancelled successfully." });
    }
}
