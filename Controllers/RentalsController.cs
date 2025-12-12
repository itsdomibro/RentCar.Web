using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Rental;
using System.Security.Claims;

namespace RentCar.Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly AppDbContext _context;

        public RentalsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdString, out var userId)) return Unauthorized();

            var query = _context.TrRentals
                .Include(r => r.Car).ThenInclude(c => c.CarImages)
                .Where(r => r.CustomerId == userId);

            var totalRentals = await query.CountAsync();

            var rentals = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RentalListItemViewModel {
                    RentalId = r.RentalId,
                    RentalDate = r.RentalDate,
                    ReturnDate = r.ReturnDate,
                    TotalRentalDays = (r.ReturnDate - r.RentalDate).Days,
                    TotalPrice = r.TotalPrice,
                    PaymentStatus = r.PaymentStatus,

                    Name = r.Car.Name,
                    Model = r.Car.Model,
                    NumberOfCarSeats = r.Car.NumberOfCarSeats,
                    Year = r.Car.Year,
                    PricePerDay = r.Car.PricePerDay,
                    LicensePlate = r.Car.LicensePlate,
                    Transmission = r.Car.Transmission
                }).ToListAsync();

            var vm = new RentalIndexViewModel { 
               PageNumber = pageNumber,
               PageSize = pageSize,
               TotalRentals = totalRentals,

               RentalListItems = rentals
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {
            var car = await _context.MsCar
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);

            if (car == null) return NotFound();

            var vm = new RentalCreateViewModel {
                CarId = car.CarId,
                Name = car.Name,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                ImageUrls = car.CarImages.Select(i => i.ImageLink).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalCreateViewModel vm)
        {
            if (!ModelState.IsValid || vm.RentalDate >= vm.ReturnDate) {
                ModelState.AddModelError("", "Invalid rental dates");
                return View(vm);
            }

            var car = await _context.MsCar.FindAsync(vm.CarId);
            if (car == null) return NotFound();


            var userIdString = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString == null) return NotFound();
            
            var totalRentalDays = (vm.ReturnDate - vm.RentalDate).Days;
            Guid.TryParse(userIdString, out var userId);

            var rental = new TrRental {
                RentalDate = vm.RentalDate,
                ReturnDate = vm.ReturnDate,
                TotalPrice = car.PricePerDay * totalRentalDays,
                PaymentStatus = false,
                CustomerId = userId,
                CarId = vm.CarId,
            };

            _context.Add(rental);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Rentals");
        }
    }
}
