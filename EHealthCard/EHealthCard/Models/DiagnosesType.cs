using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class DiagnosesType
    {
        public DiagnosesType()
        {
            Diagnoses = new HashSet<Diagnosis>();
        }
        [Required]
        [StringLength(5, ErrorMessage = "Diagnoses Type ID has to be 5 chars long", MinimumLength = 5)]
        public string DiagnosisId { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Description { get; set; } = null!;

        [Required]
        public decimal DailyCosts { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
    }
}
