using Lab6.Models;

namespace Lab6.Repositories;

public interface IPatientRepository
{
    Task<Patient?> GetPatient(int id, CancellationToken cancellationToken);
    void AddPatient(Patient patient);
}
