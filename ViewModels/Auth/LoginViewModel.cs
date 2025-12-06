using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Auth
{
    public class LoginViewModel
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
    }
}
