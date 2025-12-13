namespace RentCar.Web.ViewModels.Profile
{
    public class ProfileIndexViewModel
    {
        public Guid CustomerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string Role { get; set; }
    }
}
