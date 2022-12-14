using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class InsuranceComp
    {
        public InsuranceComp()
        {
            Insurances = new HashSet<Insurance>();
            Payments = new HashSet<Payment>();
        }

        [Key]
        [StringLength(3,
             ErrorMessage = "Company id has to be 3 chars long",
             MinimumLength = 3)]
        public string CompId { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string CompName { get; set; } = null!;

        public virtual ICollection<Insurance> Insurances { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
