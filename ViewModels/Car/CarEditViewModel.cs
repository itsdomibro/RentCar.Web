using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Car
{
    public class CarEditViewModel
    {
        [StringLength(64, MinimumLength = 1)]
        public string? Name { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string? Model { get; set; }

        [Range(1000, 5000)]
        public int? Year { get; set; }

        [StringLength(64, MinimumLength = 1)]
        public string? LicensePlate { get; set; }

        [Range(1, 128)]
        public int? NumberOfCarSeats { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string? Transmission { get; set; }

        public decimal? PricePerDay { get; set; }

        public bool? Status { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
