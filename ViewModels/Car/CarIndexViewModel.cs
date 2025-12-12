namespace RentCar.Web.ViewModels.Car
{
    public class CarIndexViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCars { get; set; }

        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? Year { get; set; }
        public string? SortOrder { get; set; }

        public List<CarListItemViewModel> Cars { get; set; } = new();
    }

    public class CarListItemViewModel
    {
        public Guid CarId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int NumberOfCarSeats { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Status { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Transmission { get; set; } = string.Empty;
        public List<string>? ImageUrls { get; set; }
    }
}
