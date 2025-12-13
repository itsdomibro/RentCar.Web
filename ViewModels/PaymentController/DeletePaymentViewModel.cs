namespace RentCar.Web.ViewModels.PaymentController
{
    public class DeletePaymentViewModel
    {
        public Guid PaymentId { get; set; }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public bool PaymentStatus { get; set; }
    }
}
