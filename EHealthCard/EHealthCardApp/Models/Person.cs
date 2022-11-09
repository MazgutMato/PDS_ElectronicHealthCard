using System;
using System.Collections.Generic;

namespace ElectronicHealthCardApp.Models;

public partial class Person
{
    public string PersonId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Zip { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; } = new List<Hospitalization>();

    public virtual ICollection<Insurance> Insurances { get; } = new List<Insurance>();

    public virtual City? ZipNavigation { get; set; }
}
