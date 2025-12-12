using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Maintenances;

namespace RentCar.Web.ViewModels.Employee
{
    public class EmployeeDetailsViewModel
    {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int TotalMaintenances { get; set; }

        public List<MaintenanceListItemViewModel> Maintenances { get; set; } = new();
    }
}
