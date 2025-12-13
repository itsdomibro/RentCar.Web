using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.MyRental
{
    public class MyRentalCreateViewModel
    {
        public Guid CarId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public int NumberOfCarSeats { get; set; }
        public string Transmission { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public bool Status { get; set; }
        public List<string> ImageUrls { get; set; } = new();

        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
    }
}
