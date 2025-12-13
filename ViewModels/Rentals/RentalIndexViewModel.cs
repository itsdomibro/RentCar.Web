using RentCar.Web.Models.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.Rentals
{
    public class RentalIndexViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRentals { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRentals / PageSize);

        public string SearchTerm { get; set; }
        public bool? PaymentStatusFilter { get; set; }
        public DateTime? RentalDateFrom { get; set; }
        public DateTime? RentalDateTo { get; set; }

        public string SortBy { get; set; } = "RentalDate";
        public bool SortDescending { get; set; } = false;

        public List<RentalItemViewModel> Rentals { get; set; } = new();
    }

    public class RentalItemViewModel
    {
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }

        public Guid CustomerId { get; set; }
        public string Email { get; set; }

        public Guid CarId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
    }
}
