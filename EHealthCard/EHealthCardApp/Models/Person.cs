using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class Person
{
    [Required]
    [StringLength(10, ErrorMessage = "Person ID has to be 10 chars long", MinimumLength = 10)]
    public string PersonId { get; set; } = null!;

    [Required]
    [StringLength(5, ErrorMessage = "Zip Code has to be 5 chars long", MinimumLength = 5)]
    public string Zip { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = null!;
    [StringLength(16)]
    public string? Phone { get; set; }
    [StringLength(40)]
    public string? Email { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; } = new List<Hospitalization>();

    public virtual ICollection<Insurance> Insurances { get; } = new List<Insurance>();

    public virtual City ZipNavigation { get; set; } = null!;
}
