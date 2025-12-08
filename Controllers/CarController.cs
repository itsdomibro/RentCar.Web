using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Car;

namespace RentCar.Web.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly AppDbContext _context;

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cars = await _context.MsCar
                .Include(c => c.CarImages)
                .Select(c => new CarListViewModel {
                    CarId = c.CarId,
                    Name = c.Name,
                    Model = c.Model,
                    Year = c.Year,
                    NumberOfCarSeats = c.NumberOfCarSeats,
                    PricePerDay = c.PricePerDay,
                    Status = c.Status,
                    ImageUrls = c.CarImages.Select(i => i.ImageLink).ToList()
                })
                .ToListAsync();

            return View(cars);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var car = await _context.MsCar
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);
            if (car == null) return NotFound();

            var vm = new CarDetailsViewModel {
                CarId = car.CarId,
                Name = car.Name,
                Model = car.Model,
                Year = car.Year,
                NumberOfCarSeats = car.NumberOfCarSeats,
                LicensePlate = car.LicensePlate,
                Transmission = car.Transmission,
                PricePerDay = car.PricePerDay,
                Status = car.Status,
                ImageUrls = car.CarImages.Select(i => i.ImageLink).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var car = new MsCar {
                Name = vm.Name,
                Model = vm.Model,
                Year = vm.Year,
                LicensePlate = vm.LicensePlate,
                NumberOfCarSeats = vm.NumberOfCarSeats,
                Transmission = vm.Transmission,
                PricePerDay = vm.PricePerDay,
                Status = vm.Status,
                CarImages = vm.ImageUrls.Select(url => new MsCarImage {
                    ImageLink = url
                }).ToList()
            };

            _context.Add(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var car = await _context.MsCar.FindAsync(id);
            if (car == null) return NotFound();

            var vm = new CarEditViewModel {
                Name = car.Name,
                Model = car.Model,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                NumberOfCarSeats = car.NumberOfCarSeats,
                Transmission = car.Transmission,
                PricePerDay = car.PricePerDay,
                Status = car.Status
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CarEditViewModel vm)
        {
            var car = await _context.MsCar.FindAsync(id);
            if (car == null) return NotFound();

            if (!ModelState.IsValid) return View(vm);

            if (vm.Name != null) car.Name = vm.Name;
            if (vm.Model != null) car.Model = vm.Model;
            if (vm.Year.HasValue) car.Year = vm.Year.Value;
            if (vm.LicensePlate != null) car.LicensePlate = vm.LicensePlate;
            if (vm.NumberOfCarSeats.HasValue) car.NumberOfCarSeats = vm.NumberOfCarSeats.Value;
            if (vm.Transmission != null) car.Transmission = vm.Transmission;
            if (vm.PricePerDay.HasValue) car.PricePerDay = vm.PricePerDay.Value;
            if (vm.Status.HasValue) car.Status = vm.Status.Value;
            if (vm.ImageUrls != null) {
                _context.MsCarImage.RemoveRange(car.CarImages);
                car.CarImages = vm.ImageUrls.Select(url => new MsCarImage {
                    CarId = car.CarId,
                    ImageLink = url
                }).ToList();
            }

            _context.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _context.MsCar.FindAsync(id);
            if (car == null) return NotFound();

            var vm = new CarDeleteViewModel {
                CarId = car.CarId,
                DisplayName = $"{car.Year} {car.Name} {car.Model}"
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var car = await _context.MsCar.FindAsync(id);
            if (car == null) return NotFound();

            _context.MsCar.Remove(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
