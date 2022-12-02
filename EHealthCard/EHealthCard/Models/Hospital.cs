using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            Hospitalizations = new HashSet<Hospitalization>();
            Payments = new HashSet<Payment>();
        }
        [Required]
        [Key]
        [StringLength(20)]
        public string HospitalName { get; set; } = null!;
        [Required]
        [StringLength(5,
        ErrorMessage = "Zip code has to be 5 chars long",
        MinimumLength = 5)]
        public string Zip { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue)]
        public decimal Capacity { get; set; }

        public virtual City ZipNavigation { get; set; } = null!;
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
