using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Car
{
    public class CarCreateViewModel
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;
    }
}
