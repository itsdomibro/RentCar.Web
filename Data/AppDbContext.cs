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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MsCustomer
            modelBuilder.Entity<MsCustomer>(entity => {
                entity.HasKey(c => c.CustomerId);
                entity.HasIndex(c => c.Email)
                    .IsUnique();
            });

            // MsCar
            modelBuilder.Entity<MsCar>(entity => {
                entity.HasKey(c => c.CarId);
                entity.HasIndex(c => c.LicensePlate)
                    .IsUnique();
            });

            // MsCarImage
            modelBuilder.Entity<MsCarImage>(entity => {
                entity.HasKey(ci => ci.CarImageId);

                entity.HasOne(ci => ci.Car)
                    .WithMany(c => c.CarImages)
                    .HasForeignKey(ci => ci.CarId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // MsEmployee
            modelBuilder.Entity<MsEmployee>(entity => {
                entity.HasKey(e => e.EmployeeId);
                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            // TrMaintenance
            modelBuilder.Entity<TrMaintenance>(entity => {
                entity.HasKey(m => m.MaintenanceId);

                entity.HasOne(m => m.Employee)
                    .WithMany(e => e.Maintenances)
                    .HasForeignKey(m => m.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Car)
                    .WithMany(c => c.Maintenances)
                    .HasForeignKey(m => m.CarId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // TrRental
            modelBuilder.Entity<TrRental>(entity => {
                entity.HasKey(r => r.RentalId);

                entity.HasOne(r => r.Customer)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Car)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // LtPayment
            modelBuilder.Entity<LtPayment>(entity => {
                entity.HasKey(p => p.PaymentId);
                entity.HasOne(p => p.Rental)
                    .WithMany(r => r.Payments)
                    .HasForeignKey(p => p.RentalId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
