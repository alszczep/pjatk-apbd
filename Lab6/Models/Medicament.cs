using System;
using System.Collections.Generic;

namespace Lab6.Models;

public partial class Medicament
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    
    public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
}
