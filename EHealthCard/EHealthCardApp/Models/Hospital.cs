using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models;

public partial class Hospital
{
    public string HospitalName { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; } = new List<Hospitalization>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual City ZipNavigation { get; set; } = null!;
}
