using Microsoft.EntityFrameworkCore;
using RentCar.Web.Models.Entities;

namespace RentCar.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<LtPayment> LtPayment { get; set; }
        public DbSet<MsCar> MsCar { get; set; }
        public DbSet<MsCarImage> MsCarImage { get; set; } 
        public DbSet<MsCustomer> MsCustomer { get; set; }
        public DbSet<MsEmployee> MsEmployee { get; set; }
        public DbSet<TrMaintenance> TrMaintenances { get; set; }
        public DbSet<TrRental> TrRentals { get; set; }
    }
}
