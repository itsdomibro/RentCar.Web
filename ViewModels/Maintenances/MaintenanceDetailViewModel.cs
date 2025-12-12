namespace RentCar.Web.ViewModels.Maintenances
{
    public class MaintenanceDetailViewModel
    {
        public Guid MaintenanceId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public Guid CarId { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int CarYear { get; set; }
        public int NumberOfCarSeats { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Transmission { get; set; } = string.Empty;
    }
}
