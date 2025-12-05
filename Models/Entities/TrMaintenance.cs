namespace RentCar.Web.Models.Entities
{
    public class TrMaintenance
    {
        public Guid MaintenanceId { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public Guid CarId { get; set; }
        public MsCar Car { get; set; } = null!;
        public Guid EmployeeId { get; set; }
        public MsEmployee Employee { get; set; } = null!;
    }
}
