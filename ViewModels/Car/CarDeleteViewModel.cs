using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Car
{
    public class CarDeleteViewModel
    {
        public Guid CarId { get; set; }
        public string DisplayName { get; set; }
    }
}
