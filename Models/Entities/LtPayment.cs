namespace RentCar.Web.Models.Entities
{
    public class LtPayment
    {
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public Guid RentalId { get; set; }
        public TrRental Rental { get; set; } = null!;
    }
}
