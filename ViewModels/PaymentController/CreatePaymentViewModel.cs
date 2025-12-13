
namespace RentCar.Web.ViewModels.PaymentController
{
    public class CreatePaymentViewModel
    {
        public DateTime Date { get; set;  } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public Guid RentalId { get; set; }
    }
}
