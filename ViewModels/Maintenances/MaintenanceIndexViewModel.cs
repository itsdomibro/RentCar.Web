namespace RentCar.Web.ViewModels.Maintenances
{
    public class MaintenanceIndexViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMaintenances { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalMaintenances / PageSize);

        public string? EmployeeName { get; set; }
        public string? CarName { get; set; }
        public string? CarModel { get; set; }
        public int? CarYear { get; set; }
        public string? LicensePlate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinCost { get; set; }
        public decimal? MaxCost { get; set; }

        public string SortBy { get; set; } = "Date";
        public bool SortDescending { get; set; } = true;


        public List<MaintenanceListItemViewModel> Maintenances { get; set; } = new();
    }

    public class MaintenanceListItemViewModel
    {
        public Guid MaintenanceId { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        
        public Guid CarId { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int CarYear { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
}
