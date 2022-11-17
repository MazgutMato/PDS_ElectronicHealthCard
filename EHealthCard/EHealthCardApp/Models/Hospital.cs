using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models;

public partial class Hospital
{
    [Required]
    [Key]
    [StringLength(20)]
    public string HospitalName { get; set; } = null!;
    [Required]
    [StringLength(5,
        ErrorMessage = "Zip code has to be 5 chars long",
        MinimumLength = 5)]
    public string Zip { get; set; } = null!;
    [Required]
    [Range(1,10000)]
    public int Capacity { get; set; }

    public virtual ICollection<Hospitalization> Hospitalizations { get; } = new List<Hospitalization>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual City ZipNavigation { get; set; } = null!;
}
