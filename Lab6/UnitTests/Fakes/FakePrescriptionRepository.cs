using Lab6.Models;
using Lab6.Repositories;

namespace UnitTests.Fakes;

public class FakePrescriptionRepository: IPrescriptionRepository
{
    public void AddPrescription(Prescription prescription) { }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
