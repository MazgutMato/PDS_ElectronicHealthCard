using System;
using System.Collections.Generic;

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

        public virtual City ZipNavigation { get; set; } = null!;
        public virtual ICollection<Hospitalization> Hospitalizations { get; set; }
        public virtual ICollection<Insurance> Insurances { get; set; }
    }
}
