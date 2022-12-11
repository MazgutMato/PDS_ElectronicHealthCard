using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHealthCard.Models
{
    public partial class DailyCosts
    {
        public DailyCosts(string name, int days, int costPerDay, int allCosts)
        {
            Name = name;
            Days = days;
            CostPerDay = costPerDay;
            AllCosts = allCosts;
        }

        public string Name { get; set; } = null!;

        public int Days { get; set; } = 0;

        public int CostPerDay { get; set; } = 0;

        public int AllCosts { get; set; } = 0;
    }
}
