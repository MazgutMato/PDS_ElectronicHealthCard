using System.ComponentModel.DataAnnotations;

namespace EHealthCard.Models
{
	public class HospitalCapacity
	{
		public string HospitalName { get; set; } = null!;
		public int Capacity { get; set; }
		public int CurrentHosp { get; set; }
	}
}
