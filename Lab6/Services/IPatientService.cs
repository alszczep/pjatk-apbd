using Lab6.DTOs;
using Lab6.Models;

namespace Lab6.Services;

public interface IPatientService
{
    Task<PatientDetailsDTO?> GetPatientDetails(int idPatient, CancellationToken cancellationToken);
}
