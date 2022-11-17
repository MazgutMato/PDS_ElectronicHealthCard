using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class Hospitalization
{
    [Required]
    [StringLength(10, ErrorMessage = "Person ID has to be 10 chars long", MinimumLength = 10)]
    public string PersonId { get; set; } = null!;
    [Required]
    [StringLength(20,ErrorMessage = "Person ID has to be 10 chars long")]
    public string HospitalName { get; set; } = null!;
    [Required]
    public DateTime DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public virtual ICollection<Diagnosis> Diagnoses { get; } = new List<Diagnosis>();

    public virtual Hospital HospitalNameNavigation { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
