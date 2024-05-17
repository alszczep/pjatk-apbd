using Api.Context;
using Api.DTOs;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/trips")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly S24454Context dbContext;

    public TripsController(S24454Context dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<TripDTO>>> GetTrips(CancellationToken cancellationToken)
    {
        return await this.dbContext.Trips
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new TripCountryDTO
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(c => new TripClientDTO
                {
                    FirstName = c.IdClientNavigation.FirstName,
                    LastName = c.IdClientNavigation.LastName,
                }).ToList()
            })
            .OrderByDescending(t => t.DateFrom)
            .ToListAsync(cancellationToken);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, AddClientToTripDTO dto, CancellationToken cancellationToken)
    {
        if(idTrip != dto.IdTrip)
        {
            // doesn't quite make sense, but the assignment requires both of these ids
            return BadRequest("IdTrip does not match.");
        }

        var trip = await this.dbContext.Trips
            .Include(t => t.ClientTrips)
            .Select(t => new
            {
                IdTrip = t.IdTrip,
                Name = t.Name,
                ClientTrips = t.ClientTrips.Select(ct => new
                {
                    IdClient = ct.IdClient
                })
            })
            .FirstOrDefaultAsync(t => t.IdTrip == dto.IdTrip && t.Name == dto.TripName, cancellationToken);

        if(trip == null)
        {
            return BadRequest("Trip does not exist.");
        }

        var client = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel, cancellationToken);

        if (client != null)
        {
            if (trip.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
            {
                return BadRequest("Client is already signed up for this trip.");
            }
        }
        else
        {
            var newIdClient = await this.dbContext.Clients.MaxAsync(c => c.IdClient, cancellationToken) + 1;

            client = new Client
            {
                IdClient = newIdClient,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Pesel = dto.Pesel,
                Email = dto.Email,
                Telephone = dto.Telephone
            };

            this.dbContext.Clients.Add(client);
        }

        var newClientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            PaymentDate = dto.PaymentDate?.ToDateTime(TimeOnly.MinValue),
            RegisteredAt = DateTime.Now
        };

        this.dbContext.ClientTrips.Add(newClientTrip);

        await this.dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }

}
