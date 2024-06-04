namespace Lab6.DTOs;

public class PatientDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public List<PrescriptionDetailsDTO> Prescriptions { get; set; } = null!;
}

public class PrescriptionDetailsDTO
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public List<MedicamentDetailsDTO> Medicaments { get; set; } = null!;
    public DoctorDetailsDTO Doctor { get; set; } = null!;
}

public class MedicamentDetailsDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;
}

public class DoctorDetailsDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
}
