using Lab6.Context;
using Lab6.DTOs;
using Lab6.Models;
using Lab6.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Controllers;

[Route("api/prescriptions")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private readonly PrescriptionsContext dbContext;
    private readonly IPrescriptionService prescriptionService;
    private readonly IPatientService patientService;

    public PrescriptionsController(PrescriptionsContext dbContext, IPrescriptionService prescriptionService, IPatientService patientService)
    {
        this.dbContext = dbContext;
        this.prescriptionService = prescriptionService;
        this.patientService = patientService;
    }

    [HttpPost("addPrescription")]
    public async Task<ActionResult> AddPrescription([FromBody] AddPrescriptionDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            await this.prescriptionService.AddPrescription(dto, cancellationToken);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }

    [HttpGet("patientDetails/{idPatient}")]
    public async Task<ActionResult<PatientDetailsDTO?>> PatientDetails(int idPatient, CancellationToken cancellationToken)
    {
        var patient = await this.patientService.GetPatientDetails(idPatient, cancellationToken);
        return patient != null ? patient : NotFound();
    }
}
