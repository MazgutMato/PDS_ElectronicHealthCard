using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class Insurance
    {
        [Required]
        [StringLength(10, ErrorMessage = "Person ID has to be 10 chars long", MinimumLength = 10)]
        public string PersonId { get; set; } = null!;

        [Required]
        [StringLength(3, ErrorMessage = "Insruance Company ID has to be 3 chars long", MinimumLength = 3)]
        public string CompId { get; set; } = null!;

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateStart { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateEnd { get; set; }

        public virtual InsuranceComp Comp { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
