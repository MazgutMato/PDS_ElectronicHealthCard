namespace EHealthCard.Models
{
    public class PaymentSum
    {
        public string ?HospitalName { get; set; } = null;
        public string? CompID { get; set; } = null;
        public List<int> Payments { get; set; } = new List<int>();
    }
}
