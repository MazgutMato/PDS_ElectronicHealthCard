using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHealthCard.Models
{
    public partial class Diagnosis
    {
        public DateTime DateStart { get; set; }
        public string HospitalName { get; set; } = null!;
        public string PersonId { get; set; } = null!;
        public string DiagnosisId { get; set; } = null!;
        public byte[]? Document { get; set; }

        public virtual DiagnosesType DiagnosisNavigation { get; set; } = null!;
        public virtual Hospitalization Hospitalization { get; set; } = null!;
    }
}
