using Lab6.DTOs;
using Lab6.Models;
using Lab6.Repositories;

namespace Lab6.Services;

public class PatientService : IPatientService
{
    private IPatientRepository patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        this.patientRepository = patientRepository;
    }

    public async Task<PatientDetailsDTO?> GetPatientDetails(int idPatient, CancellationToken cancellationToken)
    {
        var patient = await this.patientRepository.GetPatient(idPatient, cancellationToken);

        if (patient == null)
        {
            return null;
        }

        return new PatientDetailsDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.Birthdate,
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionDetailsDTO
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DateDue,
                Medicaments = pr.Prescription_Medicaments.Select(pm => new MedicamentDetailsDTO
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList(),
                Doctor = new DoctorDetailsDTO
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName,
                    Email = pr.Doctor.Email
                }
            }).ToList()
        };
    }
}
