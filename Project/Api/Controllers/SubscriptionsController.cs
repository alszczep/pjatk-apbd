using Api.DTOs;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("subscriptions")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService subscriptionsService;

    public SubscriptionsController(ISubscriptionsService subscriptionsService)
    {
        this.subscriptionsService = subscriptionsService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateSubscription([FromBody] CreateSubscriptionDTO dto,
        CancellationToken cancellationToken)
    {
        try
        {
            await this.subscriptionsService.CreateSubscriptionAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("makePayment")]
    public async Task<ActionResult> MakePayment([FromBody] SubscriptionPaymentDTO dto,
        CancellationToken cancellationToken)
    {
        try
        {
            await this.subscriptionsService.MakePaymentAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
