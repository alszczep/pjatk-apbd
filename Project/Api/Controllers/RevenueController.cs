using Api.DTOs;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("revenue")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        this.revenueService = revenueService;
    }

    [Authorize]
    [HttpPost("getCurrent")]
    public async Task<ActionResult<decimal>> GetCurrentRevenue([FromBody] RevenueDTO dto,
        CancellationToken cancellationToken)
    {
        try
        {
            return await this.revenueService.GetCurrentRevenueAsync(dto, cancellationToken);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("getPredicted")]
    public async Task<ActionResult<decimal>> GetPredictedRevenue([FromBody] RevenueDTO dto,
        CancellationToken cancellationToken)
    {
        try
        {
            return await this.revenueService.GetPredictedRevenueAsync(dto, cancellationToken);
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
