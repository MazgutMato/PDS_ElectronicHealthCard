using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models
{
    public partial class DiagnosesType
    {
        public DiagnosesType()
        {
            Diagnoses = new HashSet<Diagnosis>();
        }

        public string DiagnosisId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal DailyCosts { get; set; }

        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
    }
}
