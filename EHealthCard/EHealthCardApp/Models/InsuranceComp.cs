using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models;

public partial class InsuranceComp
{
    public string CompId { get; set; } = null!;

    public string CompName { get; set; } = null!;

    public virtual ICollection<Insurance> Insurances { get; } = new List<Insurance>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();
}
