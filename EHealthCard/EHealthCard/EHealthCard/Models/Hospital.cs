using System;
using System.Collections.Generic;

namespace EHealthCard.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            Hospitalizations = new HashSet<Hospitalization>();
            Payments = new HashSet<Payment>();
        }

        public string HospitalName { get; set; } = null!;
        public string Zip { get; set; } = null!;
        public decimal Capacity { get; set; }

        public virtual City ZipNavigation { get; set; } = null!;
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
