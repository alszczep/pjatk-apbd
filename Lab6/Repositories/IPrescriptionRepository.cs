using Lab6.Models;

namespace Lab6.Repositories;

public interface IPrescriptionRepository
{
    void AddPrescription(Prescription prescription);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
