namespace RentCar.Web.Models.Entities
{
    public class MsCar
    {
        public Guid CarId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public int NumberOfCarSeats { get; set; }
        public string Transmission { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
        public bool Status { get; set; }

        public ICollection<TrRental> Rentals { get; set; } = new List<TrRental>();
        public ICollection<TrMaintenance> Maintenances { get; set; } = new List<TrMaintenance>();
        public ICollection<MsCarImage> CarImages { get; set; } = new List<MsCarImage>();
    }
}
