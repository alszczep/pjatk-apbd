using Lab6.Context;
using Lab6.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab6.Repositories;

public class PatientRepository : IPatientRepository
{
    private PrescriptionsContext prescriptionsContext;

    public PatientRepository(PrescriptionsContext prescriptionsContext)
    {
        this.prescriptionsContext = prescriptionsContext;
    }

    public Task<Patient?> GetPatient(int id, CancellationToken cancellationToken)
    {
        return this.prescriptionsContext.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Prescription_Medicaments)
            .ThenInclude(p => p.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == id, cancellationToken);
    }

    public void AddPatient(Patient patient)
    {
        this.prescriptionsContext.Patients.Add(patient);
    }
}
