using RentCar.Web.ViewModels.PaymentController;

namespace RentCar.Web.ViewModels.Rentals
{
    public class RentalDetailsViewModel
    {
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }

        public Guid CustomerId { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DriverLicenseNumber { get; set; }

        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public int NumberOfCarSeats { get; set; }
        public string Transmission { get; set; }

        public List<PaymentIndexViewModel> Payments { get; set; } = new();
    }
}
