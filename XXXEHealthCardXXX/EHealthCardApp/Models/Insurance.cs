using System;
using System.Collections.Generic;

namespace EHealthCardApp.Models
{
    public partial class Insurance
    {
        public string PersonId { get; set; } = null!;
        public string CompId { get; set; } = null!;
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public virtual InsuranceComp Comp { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
