using Api.DTOs;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Microsoft.AspNetCore.Components.Route("clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService clientsService;

    public ClientsController(IClientsService clientsService)
    {
        this.clientsService = clientsService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<ActionResult> AddClient([FromBody] AddClientDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            await this.clientsService.AddClientAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("update")]
    public async Task<ActionResult> UpdateClient([FromBody] UpdateClientDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            await this.clientsService.UpdateClientAsync(dto, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("delete/{id:guid}")]
    public async Task<ActionResult> DeleteClient(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await this.clientsService.DeleteClientAsync(id, cancellationToken);
            return this.Ok();
        }
        catch (Exception e)
        {
            return this.BadRequest(e.Message);
        }
    }
}
