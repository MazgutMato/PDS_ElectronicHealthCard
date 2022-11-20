using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models
{
    public partial class InsuranceComp
    {
        public InsuranceComp()
        {
            Insurances = new HashSet<Insurance>();
            Payments = new HashSet<Payment>();
        }

        public string CompId { get; set; } = null!;
        public string CompName { get; set; } = null!;

        public virtual ICollection<Insurance> Insurances { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
