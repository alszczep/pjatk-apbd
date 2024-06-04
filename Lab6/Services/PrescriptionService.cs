using Lab6.Context;
using Lab6.DTOs;
using Lab6.Models;
using Lab6.Repositories;

namespace Lab6.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository prescriptionRepository;
    private readonly IMedicamentRepository medicamentRepository;
    private readonly IPatientRepository patientRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository, IMedicamentRepository medicamentRepository, IPatientRepository patientRepository)
    {
        this.prescriptionRepository = prescriptionRepository;
        this.medicamentRepository = medicamentRepository;
        this.patientRepository = patientRepository;
    }

    public async Task AddPrescription(AddPrescriptionDTO dto, CancellationToken cancellationToken)
    {
        if(dto.Medicaments.Count > 10)
        {
            throw new InvalidOperationException("Medicaments have to be 10 or less");
        }

        if(dto.Date > dto.DueDate)
        {
            throw new InvalidOperationException("Date has to be before due date");
        }

        var patient = await this.patientRepository.GetPatient(dto.Patient.IdPatient, cancellationToken);

        if (patient == null)
        {
            patient = new Patient
            {
                IdPatient = dto.Patient.IdPatient,
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.BirthDate
            };
            this.patientRepository.AddPatient(patient);
        }

        if (!await this.medicamentRepository.CheckIfMedicamentsExist(dto.Medicaments.Select(m => m.IdMedicament).ToList(), cancellationToken))
        {
            throw new InvalidOperationException("Some medicament does not exist");
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DateDue = dto.DueDate,
            IdDoctor = dto.IdDoctor,
            IdPatient = patient.IdPatient,
            Prescription_Medicaments = dto.Medicaments.Select(m => new Prescription_Medicament
            {
                IdMedicament = m.IdMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };

        this.prescriptionRepository.AddPrescription(prescription);

        await this.prescriptionRepository.SaveChangesAsync(cancellationToken);
    }
}
