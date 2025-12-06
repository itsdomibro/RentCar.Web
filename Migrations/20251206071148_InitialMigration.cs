using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCar.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MsCar",
                columns: table => new
                {
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfCarSeats = table.Column<int>(type: "int", nullable: false),
                    Transmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCar", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "MsCustomer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverLicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCustomer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "MsEmployee",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsEmployee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "MsCarImage",
                columns: table => new
                {
                    CarImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsCarImage", x => x.CarImageId);
                    table.ForeignKey(
                        name: "FK_MsCarImage_MsCar_CarId",
                        column: x => x.CarId,
                        principalTable: "MsCar",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrRentals",
                columns: table => new
                {
                    RentalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RentalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrRentals", x => x.RentalId);
                    table.ForeignKey(
                        name: "FK_TrRentals_MsCar_CarId",
                        column: x => x.CarId,
                        principalTable: "MsCar",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrRentals_MsCustomer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MsCustomer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrMaintenances",
                columns: table => new
                {
                    MaintenanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrMaintenances", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_TrMaintenances_MsCar_CarId",
                        column: x => x.CarId,
                        principalTable: "MsCar",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrMaintenances_MsEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "MsEmployee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LtPayment",
                columns: table => new
                {
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RentalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtPayment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_LtPayment_TrRentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "TrRentals",
                        principalColumn: "RentalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LtPayment_RentalId",
                table: "LtPayment",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_MsCar_LicensePlate",
                table: "MsCar",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsCarImage_CarId",
                table: "MsCarImage",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_MsCustomer_Email",
                table: "MsCustomer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsEmployee_Email",
                table: "MsEmployee",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrMaintenances_CarId",
                table: "TrMaintenances",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TrMaintenances_EmployeeId",
                table: "TrMaintenances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrRentals_CarId",
                table: "TrRentals",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_TrRentals_CustomerId",
                table: "TrRentals",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LtPayment");

            migrationBuilder.DropTable(
                name: "MsCarImage");

            migrationBuilder.DropTable(
                name: "TrMaintenances");

            migrationBuilder.DropTable(
                name: "TrRentals");

            migrationBuilder.DropTable(
                name: "MsEmployee");

            migrationBuilder.DropTable(
                name: "MsCar");

            migrationBuilder.DropTable(
                name: "MsCustomer");
        }
    }
}
