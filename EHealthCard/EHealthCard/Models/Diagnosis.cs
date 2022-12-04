using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHealthCard.Models
{
    public partial class Diagnosis
    {
        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        [StringLength(20)]
        public string HospitalName { get; set; } = null!;

        [Required]
        [StringLength(10, ErrorMessage = "Person ID has to be 10 chars long", MinimumLength = 10)]
        public string PersonId { get; set; } = null!;

        [Required]
        [StringLength(5, ErrorMessage = "Diagnoses Type ID has to be 5 chars long", MinimumLength = 5)]
        public string DiagnosisId { get; set; } = null!;

        [Required]
        public byte[]? Document { get; set; } = new byte[0];

        public virtual DiagnosesType DiagnosisNavigation { get; set; } = null!;
        public virtual Hospitalization Hospitalization { get; set; } = null!;
    }
}
