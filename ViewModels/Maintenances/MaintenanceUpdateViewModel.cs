using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Maintenances
{
    public class MaintenanceUpdateViewModel
    {
        public Guid MaintenanceId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [StringLength(256)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Cost")]
        public decimal Cost { get; set; }
        [Required]
        [Display(Name = "Car")]
        public Guid CarId { get; set; }
        [Required]
        [Display(Name = "Employee")]
        public Guid EmployeeId { get; set; }

        public IEnumerable<SelectListItem>? Cars { get; set; }
        public IEnumerable<SelectListItem>? Employees { get; set; }
    }
}
