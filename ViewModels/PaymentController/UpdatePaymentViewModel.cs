using System.ComponentModel.DataAnnotations;

namespace RentCar.Web.ViewModels.PaymentController
{
    public class UpdatePaymentViewModel
    {
        public Guid PaymentId { get; set; }

        [Required]
        public DateTime Date { get; set ; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public Guid RentalId { get; set; }
    }
}
