using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class Payment
    {
        [Required]
        public decimal PaymentId { get; set; }
        [Required]
        [StringLength(20,
             ErrorMessage = "Hospital name has to be maximal 20 chars long")]
        public string HospitalName { get; set; } = null!;
        [Required]
        [StringLength(3,
             ErrorMessage = "Company id has to be 3 chars long",
             MinimumLength = 3)]
        public string CompId { get; set; } = null!;
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PaymentDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PaymentPeriod { get; set; }
        [Required]
        public string Details { get; set; } = null!;


        [Required]
        [NotMapped]
        public string CompBank { get; set; } = null!;
        [Required]
        [NotMapped]
        public string HospitalBank { get; set; } = null!;
        [Required]
        [NotMapped]
        public string CompIban { get; set; } = null!;
        [Required]
        [NotMapped]
        public string HospitalIban { get; set; } = null!;
        [Required]
        [NotMapped]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }
        public virtual InsuranceComp Comp { get; set; } = null!;
        public virtual Hospital HospitalNameNavigation { get; set; } = null!;
    }
}
