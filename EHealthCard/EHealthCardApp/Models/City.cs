using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class City
{
    [Required]
    [StringLength(5, 
        ErrorMessage = "Zip code has to be 5 chars long",
        MinimumLength = 5)]
    public string Zip { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string CityName { get; set; } = null!;

    public virtual ICollection<Hospital> Hospitals { get; } = new List<Hospital>();

    public virtual ICollection<Person> People { get; } = new List<Person>();
}
