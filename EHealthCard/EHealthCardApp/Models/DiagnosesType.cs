using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class DiagnosesType
{
    [Required]
    [StringLength(5, ErrorMessage = "Diagnoses ID has to be 5 chars long", MinimumLength = 5)]
    public string DiagnosisId { get; set; } = null!;
    [Required]
    [StringLength(50)]
    public string Description { get; set; } = null!;
    [Required]
    public decimal DailyCosts { get; set; }

    public virtual ICollection<Diagnosis> Diagnoses { get; } = new List<Diagnosis>();
}
