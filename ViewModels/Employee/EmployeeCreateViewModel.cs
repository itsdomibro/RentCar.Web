using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Employee
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 2)]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
    }
}
