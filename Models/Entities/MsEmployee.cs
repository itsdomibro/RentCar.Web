namespace RentCar.Web.Models.Entities
{
    public class MsEmployee
    {
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<TrMaintenance> Maintenances { get; set; } = new List<TrMaintenance>();
    }
}
