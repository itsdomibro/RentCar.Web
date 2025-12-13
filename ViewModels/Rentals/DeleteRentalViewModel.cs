namespace RentCar.Web.ViewModels.Rentals
{
    public class DeleteRentalViewModel
    {
        public Guid RentalId { get; set; }

        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }

        public Guid CustomerId { get; set; }
        public string Email { get; set; }

        public Guid CarId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }
}
