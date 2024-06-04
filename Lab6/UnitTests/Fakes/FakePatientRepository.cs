using Lab6.Models;
using Lab6.Repositories;

namespace UnitTests.Fakes;

public class FakePatientRepository: IPatientRepository
{
    public Task<Patient?> GetPatient(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult<Patient?>(null);
    }

    public void AddPatient(Patient patient) { }
}
