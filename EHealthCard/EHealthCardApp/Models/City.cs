using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models;

public partial class City
{
    public string Zip { get; set; } = null!;

    public string CityName { get; set; } = null!;

    public virtual ICollection<Hospital> Hospitals { get; } = new List<Hospital>();

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
