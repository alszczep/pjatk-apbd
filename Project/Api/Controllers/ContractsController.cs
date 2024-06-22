using Api.DTOs;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractsService contractsService;

    public ContractsController(IContractsService contractsService)
    {
        this.contractsService = contractsService;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult> CreateContract([FromBody] CreateContractDTO dto,
        CancellationToken cancellationToken)
    {
        try
        {
            await this.contractsService.CreateContractAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("makePayment")]
    public async Task<ActionResult> MakePayment([FromBody] ContractPaymentDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            await this.contractsService.MakePaymentAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
