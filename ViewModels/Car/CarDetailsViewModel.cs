using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Car
{
    public class CarDetailsViewModel
    {
        public Guid CarId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int NumberOfCarSeats { get; set; }
        public string LicensePlate { get; set; }
        public string Transmission { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Status { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
