using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models
{
    public partial class Hospitalization
    {
        public Hospitalization()
        {
            Diagnoses = new HashSet<Diagnosis>();
        }

        public string PersonId { get; set; } = null!;
        public string HospitalName { get; set; } = null!;
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public virtual Hospital HospitalNameNavigation { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Diagnosis> Diagnoses { get; set; }
    }
}
