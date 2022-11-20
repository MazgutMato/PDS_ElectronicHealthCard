using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models
{
    public partial class Payment
    {
        public decimal PaymentId { get; set; }
        public string HospitalName { get; set; } = null!;
        public string CompId { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentPeriod { get; set; }
        public string Details { get; set; } = null!;

        public virtual InsuranceComp Comp { get; set; } = null!;
        public virtual Hospital HospitalNameNavigation { get; set; } = null!;
    }
}
