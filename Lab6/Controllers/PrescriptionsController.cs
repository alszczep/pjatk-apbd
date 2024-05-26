using Lab6.Context;
using Lab6.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Controllers;

[Route("api/prescriptions")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly PrescriptionsContext dbContext;

    public PrescriptionsController(PrescriptionsContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPost("addPrescription")]
    public async Task<ActionResult> AddPrescription([FromBody] AddPrescriptionDTO dto, CancellationToken cancellationToken)
    {
    //     var client = await this.dbContext.Clients
    //         .Include(c => c.ClientTrips)
    //         .FirstOrDefaultAsync(c => c.IdClient == idClient, cancellationToken);
    //
    //     if (client == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var clientTripsCount = await this.dbContext.ClientTrips
    //         .CountAsync(ct => ct.IdClient == idClient, cancellationToken);
    //
    //     if (clientTripsCount > 0)
    //     {
    //         return BadRequest("Client cannot have any trips assigned.");
    //     }
    //
    //     this.dbContext.Clients.Remove(client);
    //     await this.dbContext.SaveChangesAsync(cancellationToken);
    //
        return Ok();
    }
    
    [HttpGet("patientDetails/{idPatient}")]
    public async Task<ActionResult<PatientDetailsDTO>> PatientDetails(int idPatient, CancellationToken cancellationToken)
    {
        //     var client = await this.dbContext.Clients
        //         .Include(c => c.ClientTrips)
        //         .FirstOrDefaultAsync(c => c.IdClient == idClient, cancellationToken);
        //
        //     if (client == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var clientTripsCount = await this.dbContext.ClientTrips
        //         .CountAsync(ct => ct.IdClient == idClient, cancellationToken);
        //
        //     if (clientTripsCount > 0)
        //     {
        //         return BadRequest("Client cannot have any trips assigned.");
        //     }
        //
        //     this.dbContext.Clients.Remove(client);
        //     await this.dbContext.SaveChangesAsync(cancellationToken);
        //
        return Ok();
    }
}
