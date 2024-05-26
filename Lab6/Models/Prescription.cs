using System;
using System.Collections.Generic;

namespace Lab6.Models;

public partial class Prescription
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DateDue { get; set; }
    
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
    
    public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
}
