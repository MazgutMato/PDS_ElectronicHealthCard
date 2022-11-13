using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models;

public partial class Insurance
{
    
    [Required]
    public string PersonId { get; set; } = null!;
    [Required]
    public string CompId { get; set; } = null!;
    [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DateStart { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? DateEnd { get; set; }

    public virtual InsuranceComp Comp { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
