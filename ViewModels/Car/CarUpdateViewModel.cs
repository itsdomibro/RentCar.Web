using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Car
{
    public class CarUpdateViewModel
    {
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string Model { get; set; }
        [Required]
        [Range(1000, 5000)]
        public int Year { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string LicensePlate { get; set; }
        [Required]
        [Range(1, 128)]
        public int NumberOfCarSeats { get; set; }
        [Required]
        [StringLength(128, MinimumLength = 1)]
        public string Transmission { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        [Required]
        public bool Status { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }


}