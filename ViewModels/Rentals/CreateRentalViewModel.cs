using Microsoft.AspNetCore.Mvc.Rendering;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.PaymentController;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Rentals
{
    public class CreateRentalViewModel
    {
        [Required]
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
        [Required]
        public bool PaymentStatus { get; set; } = false;
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid CarId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Cars { get; set; } = new List<SelectListItem>();
    }
}
