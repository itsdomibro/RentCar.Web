namespace RentCar.Web.Models.Entities
{
    public class TrRental
    {
        public Guid RentalId { get; set; } = Guid.NewGuid();
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; } = false;
        public Guid CustomerId { get; set; }
        public MsCustomer Customer { get; set; } = null!;
        public Guid CarId { get; set; }
        public MsCar Car { get; set; } = null!;

        public ICollection<LtPayment> Payments { get; set; } = new List<LtPayment>();
    }
}
