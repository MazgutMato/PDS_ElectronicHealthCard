using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
    public partial class City
    {
        public City()
        {
            Hospitals = new HashSet<Hospital>();
            People = new HashSet<Person>();
        }

        [Required]
        [Key]
        [StringLength(5,
        ErrorMessage = "Zip code has to be 5 chars long",
        MinimumLength = 5)]
        public string Zip { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string CityName { get; set; } = null!;

        public virtual ICollection<Hospital> Hospitals { get; set; }
        public virtual ICollection<Person> People { get; set; }
    }
}
