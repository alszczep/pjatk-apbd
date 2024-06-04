using Lab6.Context;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private PrescriptionsContext prescriptionsContext;

    public PrescriptionRepository(PrescriptionsContext prescriptionsContext)
    {
        this.prescriptionsContext = prescriptionsContext;
    }

    public void AddPrescription(Prescription prescription)
    {
        this.prescriptionsContext.Prescriptions.Add(prescription);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await this.prescriptionsContext.SaveChangesAsync(cancellationToken);
    }
}
