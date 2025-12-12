namespace RentCar.Web.ViewModels.Maintenances
{
    public class MaintenanceDeleteViewModel
    {
        public Guid MaintenanceId { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int CarYear { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
}
