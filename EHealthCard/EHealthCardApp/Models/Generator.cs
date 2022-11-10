using System.ComponentModel.DataAnnotations;

namespace EHealthCardApp.Models
{
    public class Generator
    {
        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Cities { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Diagnoses { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int DiagnosisTypes { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Hospitals { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Hospitalizations { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Insurances { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int InsuranceComps { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Payments { get; set; }

        [Range(1, 1000000,
            ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int People { get; set; }
    }
}
