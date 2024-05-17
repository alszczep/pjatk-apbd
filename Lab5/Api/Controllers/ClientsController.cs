using Api.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly S24454Context dbContext;

    public ClientsController(S24454Context dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient, CancellationToken cancellationToken)
    {
        var client = await this.dbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient, cancellationToken);

        if (client == null)
        {
            return NotFound();
        }

        var clientTripsCount = await this.dbContext.ClientTrips
            .CountAsync(ct => ct.IdClient == idClient, cancellationToken);

        if (clientTripsCount > 0)
        {
            return BadRequest("Client cannot have any trips assigned.");
        }

        this.dbContext.Clients.Remove(client);
        await this.dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }

}
