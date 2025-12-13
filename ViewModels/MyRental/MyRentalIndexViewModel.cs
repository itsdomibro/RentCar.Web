
namespace RentCar.Web.ViewModels.MyRental
{
    public class MyRentalIndexViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRentals { get; set; }

        public List<RentalListItemViewModel> RentalListItems { get; set; } = new();
    }

    public class RentalListItemViewModel
    {
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalRentalDays { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int NumberOfCarSeats { get; set; }
        public int Year { get; set; }
        public decimal PricePerDay { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Transmission { get; set; } = string.Empty;
    }
}
