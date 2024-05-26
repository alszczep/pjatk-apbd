namespace Lab6.Models;

public class Prescription_Medicament
{
    public int? Dose { get; set; }
    public string Details { get; set; }
    
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}