namespace RentCar.Web.Models.Entities
{
    public class MsCustomer
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DriverLicenseNumber { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";

        public ICollection<TrRental> Rentals { get; set; } = new List<TrRental>();
    }
}
