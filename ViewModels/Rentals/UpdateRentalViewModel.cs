using Microsoft.AspNetCore.Mvc.Rendering;
using RentCar.Web.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Rentals
{
    public class UpdateRentalViewModel
    {
        public Guid RentalId { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime RentalDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }
        [Required]
        public bool PaymentStatus { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid CarId { get; set; }

        public IEnumerable<SelectListItem> Customers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Cars { get; set; } = new List<SelectListItem>();
    }
}
