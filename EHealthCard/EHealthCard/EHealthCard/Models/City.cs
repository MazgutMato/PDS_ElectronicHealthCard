using System;
using System.Collections.Generic;

namespace EHealthCard.Models
{
    public partial class City
    {
        public City()
        {
            Hospitals = new HashSet<Hospital>();
            People = new HashSet<Person>();
        }

        public string Zip { get; set; } = null!;
        public string CityName { get; set; } = null!;

        public virtual ICollection<Hospital> Hospitals { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
