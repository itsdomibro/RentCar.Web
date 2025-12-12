using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Car;

namespace RentCar.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? pickupDate, DateTime? returnDate, int? year, string? sortOrder, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.MsCar
                .Include(c => c.CarImages)
                .Where(c => c.Status == true);

            if (pickupDate.HasValue && returnDate.HasValue && pickupDate < returnDate) {
                query = query.Where(c => !c.Rentals.Any(r =>
                    r.RentalDate < returnDate.Value &&
                    r.ReturnDate > pickupDate.Value));
            }

            if (year.HasValue) {
                query = query.Where(c => c.Year == year.Value);
            }

            query = sortOrder switch {
                "asc" => query.OrderBy(c => c.PricePerDay),
                "desc" => query.OrderByDescending(c => c.PricePerDay),
                _ => query.OrderBy(c => c.Name)
            };

            var totalCars = await query.CountAsync();

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CarListItemViewModel {
                    CarId = c.CarId,
                    Name = c.Name,
                    Model = c.Model,
                    Year = c.Year,
                    NumberOfCarSeats = c.NumberOfCarSeats,
                    PricePerDay = c.PricePerDay,
                    Status = c.Status,
                    LicensePlate = c.LicensePlate,
                    Transmission = c.Transmission,
                    ImageUrls = c.CarImages.Select(i => i.ImageLink).ToList()
                })
                .ToListAsync();

            var vm = new CarIndexViewModel {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCars = totalCars,

                PickupDate = pickupDate,
                ReturnDate = returnDate,
                Year = year,
                SortOrder = sortOrder,

                Cars = cars,
            };

            ViewBag.SortOptions = new SelectList(new[]
            {
                new { Value = "", Text = "Name (default)" },
                new { Value = "asc", Text = "Lowest Price" },
                new { Value = "desc", Text = "Highest Price" }
             }, "Value", "Text", sortOrder);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var car = await _context.MsCar
                .Include(c => c.CarImages)
                .AsNoTracking()
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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
            };

            if (vm.ImageFiles != null) {
                car.CarImages = new List<MsCarImage>();

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cars");
                if (!Directory.Exists(uploadDir)) {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (var file in vm.ImageFiles) {
                    if (file == null || file.Length == 0) continue;
                    
                    var extension = string.IsNullOrEmpty(file.FileName) ? ".jpg" : Path.GetExtension(file.FileName);
                    var fileName = Guid.NewGuid() + extension;
                    var filePath = Path.Combine(uploadDir, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    car.CarImages.Add(new MsCarImage { CarId = car.CarId, ImageLink = "/images/cars/" + fileName });
                }
            }

            _context.Add(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(Guid id)
        {
            var car = await _context.MsCar
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);
            if (car == null) return NotFound();

            var vm = new CarUpdateViewModel {
                Name = car.Name,
                Model = car.Model,
                Year = car.Year,
                LicensePlate = car.LicensePlate,
                NumberOfCarSeats = car.NumberOfCarSeats,
                Transmission = car.Transmission,
                PricePerDay = car.PricePerDay,
                Status = car.Status,
                ImageUrls = car.CarImages.Select(i => i.ImageLink).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, CarUpdateViewModel vm)
        {
            if (!ModelState.IsValid) {
                vm.ImageUrls = await _context.MsCarImage
                    .Where(i => i.CarId == id)
                    .Select(i => i.ImageLink)
                    .ToListAsync();
                return View(vm);
            }

            var car = await _context.MsCar
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.CarId == id);
            if (car == null) return NotFound();

            car.Name = vm.Name;
            car.Model = vm.Model;
            car.Year = vm.Year;
            car.LicensePlate = vm.LicensePlate;
            car.NumberOfCarSeats = vm.NumberOfCarSeats;
            car.Transmission = vm.Transmission;
            car.PricePerDay = vm.PricePerDay;
            car.Status = vm.Status;

            if (vm.ImageFiles != null && vm.ImageFiles.Any()) {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cars");
                if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                foreach (var file in vm.ImageFiles) {
                    if (file == null || file.Length == 0) continue;
                    
                    var extension = string.IsNullOrEmpty(file.FileName) ? ".jpg" : Path.GetExtension(file.FileName);
                    var fileName = Guid.NewGuid() + extension;
                    var filePath = Path.Combine(uploadDir, fileName);

                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    car.CarImages.Add(new MsCarImage { CarId = car.CarId, ImageLink = "/images/cars/" + fileName });
                }
            }

            if (vm.ImageUrls != null) {
                var toRemove = car.CarImages.Where(i => !vm.ImageUrls.Contains(i.ImageLink)).ToList();
                _context.MsCarImage.RemoveRange(toRemove);
            }

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
