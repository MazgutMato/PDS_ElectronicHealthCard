using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class Person
{
    [Required]
    [StringLength(10, MinimumLength = 10,
        ErrorMessage = "The field PersonId must be a string with a length of 10.")]
    public string PersonId { get; set; } = null!;
    
    [Required]
    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = null!;

    public string? Zip { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; } = new List<Hospitalization>();

    public virtual ICollection<Insurance> Insurances { get; } = new List<Insurance>();

    public virtual City? ZipNavigation { get; set; }
}
