using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Rentals;

namespace RentCar.Web.Controllers
{
    [Authorize(Roles = "Owner")]
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "RentalDate",
            bool sortDesc = false,
            string searchTerm = null,
            bool? paymentStatus = null,
            DateTime? rentalDateFrom = null,
            DateTime? rentalDateTo = null)
        {
            var query = _context.TrRentals
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm)) {
                query = query.Where(r =>
                    r.Customer.Email.Contains(searchTerm) ||
                    r.Car.Name.Contains(searchTerm) ||
                    r.Car.Model.Contains(searchTerm) ||
                    r.Car.LicensePlate.Contains(searchTerm));
            }

            if (paymentStatus.HasValue)
                query = query.Where(r => r.PaymentStatus == paymentStatus.Value);

            if (rentalDateFrom.HasValue)
                query = query.Where(r => r.RentalDate >= rentalDateFrom.Value);

            if (rentalDateTo.HasValue)
                query = query.Where(r => r.RentalDate <= rentalDateTo.Value);

            query = sortBy switch {
                "TotalPrice" => sortDesc ? query.OrderByDescending(r => r.TotalPrice) : query.OrderBy(r => r.TotalPrice),
                "ReturnDate" => sortDesc ? query.OrderByDescending(r => r.ReturnDate) : query.OrderBy(r => r.ReturnDate),
                "Email" => sortDesc ? query.OrderByDescending(r => r.Customer.Email) : query.OrderBy(r => r.Customer.Email),
                _ => sortDesc ? query.OrderByDescending(r => r.RentalDate) : query.OrderBy(r => r.RentalDate),
            };

            var totalRentals = await query.CountAsync();
            var rentals = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RentalItemViewModel {
                    RentalId = r.RentalId,
                    RentalDate = r.RentalDate,
                    ReturnDate = r.ReturnDate,
                    TotalPrice = r.TotalPrice,
                    PaymentStatus = r.PaymentStatus,
                    CustomerId = r.CustomerId,
                    Email = r.Customer.Email,
                    CarId = r.CarId,
                    Name = r.Car.Name,
                    Model = r.Car.Model,
                    Year = r.Car.Year,
                    LicensePlate = r.Car.LicensePlate
                })
                .ToListAsync();

            var vm = new RentalIndexViewModel {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRentals = totalRentals,
                SortBy = sortBy,
                SortDescending = sortDesc,
                SearchTerm = searchTerm,
                PaymentStatusFilter = paymentStatus,
                RentalDateFrom = rentalDateFrom,
                RentalDateTo = rentalDateTo,
                Rentals = rentals
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var rental = await _context.TrRentals
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .Include(r => r.Payments)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RentalId == id);

            if (rental is null)
                return NotFound();

            var vm = new RentalDetailsViewModel {
                RentalId = rental.RentalId,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate,
                TotalPrice = rental.TotalPrice,
                PaymentStatus = rental.PaymentStatus,
                CustomerId = rental.CustomerId,
                Email = rental.Customer.Email,
                CustomerName = rental.Customer.Name,
                PhoneNumber = rental.Customer.PhoneNumber,
                Address = rental.Customer.Address,
                DriverLicenseNumber = rental.Customer.DriverLicenseNumber,
                CarId = rental.CarId,
                CarName = rental.Car.Name,
                Model = rental.Car.Model,
                Year = rental.Car.Year,
                LicensePlate = rental.Car.LicensePlate,
                NumberOfCarSeats = rental.Car.NumberOfCarSeats,
                Transmission = rental.Car.Transmission,
                Payments = rental.Payments.Select(p => new ViewModels.PaymentController.PaymentIndexViewModel {
                    PaymentId = p.PaymentId,
                    Date = p.Date,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    RentalId = p.RentalId,
                    TotalPrice = rental.TotalPrice,
                    PaymentStatus = rental.PaymentStatus
                }).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateRentalViewModel {
                Customers = await _context.MsCustomer
                    .Select(c => new SelectListItem {
                        Value = c.CustomerId.ToString(),
                        Text = c.Name + " (" + c.Email + ")"
                    })
                    .ToListAsync(),
                Cars = await _context.MsCar
                    .Select(c => new SelectListItem {
                        Value = c.CarId.ToString(),
                        Text = c.Name + " " + c.Model + " [" + c.LicensePlate + "]"
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRentalViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var rental = new TrRental {
                RentalId = Guid.NewGuid(),
                RentalDate = vm.RentalDate,
                ReturnDate = vm.ReturnDate,
                TotalPrice = vm.TotalPrice,
                PaymentStatus = vm.PaymentStatus,
                CustomerId = vm.CustomerId,
                CarId = vm.CarId
            };

            _context.TrRentals.Add(rental);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var rental = await _context.TrRentals.AsNoTracking().FirstOrDefaultAsync(r => r.RentalId == id);
            if (rental is null)
                return NotFound();

            var vm = new UpdateRentalViewModel {
                RentalId = rental.RentalId,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate,
                TotalPrice = rental.TotalPrice,
                PaymentStatus = rental.PaymentStatus,
                CustomerId = rental.CustomerId,
                CarId = rental.CarId,
                Customers = await _context.MsCustomer
                    .Select(c => new SelectListItem {
                        Value = c.CustomerId.ToString(),
                        Text = c.Name + " (" + c.Email + ")"
                    })
                    .ToListAsync(),
                Cars = await _context.MsCar
                    .Select(c => new SelectListItem {
                        Value = c.CarId.ToString(),
                        Text = c.Name + " " + c.Model + " [" + c.LicensePlate + "]"
                    })
                    .ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateRentalViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var rental = await _context.TrRentals.FirstOrDefaultAsync(r => r.RentalId == vm.RentalId);
            if (rental is null)
                return NotFound();

            rental.RentalDate = vm.RentalDate;
            rental.ReturnDate = vm.ReturnDate;
            rental.TotalPrice = vm.TotalPrice;
            rental.PaymentStatus = vm.PaymentStatus;
            rental.CustomerId = vm.CustomerId;
            rental.CarId = vm.CarId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rental = await _context.TrRentals
                .Include(r => r.Customer)
                .Include(r => r.Car)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RentalId == id);

            if (rental is null)
                return NotFound();

            var vm = new DeleteRentalViewModel {
                RentalId = rental.RentalId,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate,
                TotalPrice = rental.TotalPrice,
                PaymentStatus = rental.PaymentStatus,
                CustomerId = rental.CustomerId,
                Email = rental.Customer.Email,
                CarId = rental.CarId,
                Name = rental.Car.Name,
                Model = rental.Car.Model,
                Year = rental.Car.Year,
                LicensePlate = rental.Car.LicensePlate
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rental = await _context.TrRentals.FindAsync(id);
            if (rental is null)
                return NotFound();

            _context.TrRentals.Remove(rental);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}