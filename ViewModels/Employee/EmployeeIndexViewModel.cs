namespace RentCar.Web.ViewModels.Employee
{
    public class EmployeeIndexViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalEmployees { get; set; }

        public string? Search { get; set; }

        public List<EmployeeListItemViewModel> Employees { get; set; } = new();
    }

    public class EmployeeListItemViewModel {
        public Guid EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int TotalMaintenances { get; set; }
    }
}
