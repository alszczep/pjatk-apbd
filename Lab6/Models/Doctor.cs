using System;
using System.Collections.Generic;

namespace Lab6.Models;

public partial class Doctor
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public ICollection<Prescription> Prescriptions { get; set; }
}
