using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHealthCard.Models
{
    public partial class MostInsured
    {
        public MostInsured(string personId, string firstName, string lastName, int numberInsured)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            NumberInsured = numberInsured;
        }

        public string PersonId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int NumberInsured { get; set; } = 0;
    }
}
