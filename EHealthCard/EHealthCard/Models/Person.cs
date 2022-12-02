using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHealthCard.Models
{
    public partial class Person
    {
        public Person()
        {
            Hospitalizations = new HashSet<Hospitalization>();
            Insurances = new HashSet<Insurance>();
        }

        public string PersonId { get; set; } = null!;
        public string Zip { get; set; } = null!;

        [Required]
        [NotMapped]
        [StringLength(20)]
        public string FirstName { get; set; } = null!;
        [Required]
        [NotMapped]
        [StringLength(30)]
        public string LastName { get; set; } = null!;
        [Required]
        [NotMapped]
        [StringLength(16)]
        public string? Phone { get; set; }
        [NotMapped]
        [StringLength(40)]
        public string? Email { get; set; }

        public virtual City ZipNavigation { get; set; } = null!;
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<Insurance> Insurances { get; set; }
    }
}
