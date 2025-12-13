using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Profile
{
    public class ProfileUpdateViewModel
    {
        public Guid CustomerId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 4)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "Driver License Number")]
        public string DriverLicenseNumber { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
