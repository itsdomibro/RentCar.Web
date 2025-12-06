using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters and no more than 32 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Name must be at least 2 characters and no more than 32 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(64, MinimumLength = 4, ErrorMessage = "Address must be at least 4 characters and no more than 64 characters")]
        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "Driver license number must be at least 2 characters and no more than 32 characters")]
        [Display(Name = "Driver License Number")]
        public string DriverLicenseNumber { get; set; } = string.Empty;
    }
}
