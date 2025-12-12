using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Employee;
using RentCar.Web.ViewModels.Maintenances;

namespace RentCar.Web.Controllers
{
    [Authorize(Roles = "Owner")]
    public class MaintenancesController : Controller
    {
        private readonly AppDbContext _context;

        public MaintenancesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? employeeName,
            string? carName,
            string? carModel,
            int? carYear,
            string? licensePlate,
            DateTime? startDate,
            DateTime? endDate,
            decimal? minCost,
            decimal? maxCost,
            string sortBy = "Date",
            bool sortDescending = true,
            int pageNumber = 1,
            int pageSize = 10) {

            var query = _context.TrMaintenances.AsQueryable();

            if (!string.IsNullOrEmpty(employeeName))
                query = query.Where(m => EF.Functions.Like(m.Employee.Name, $"%{employeeName}%"));

            if (!string.IsNullOrEmpty(carName))
                query = query.Where(m => EF.Functions.Like(m.Car.Name, $"%{carName}%"));

            if (startDate.HasValue)
                query = query.Where(m => m.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(m => m.Date <= endDate.Value);

            if (!string.IsNullOrEmpty(licensePlate))
                query = query.Where(m => EF.Functions.Like(m.Car.LicensePlate, $"%{licensePlate}%"));

            if (minCost.HasValue)
                query = query.Where(m => m.Cost >= minCost.Value);

            if (maxCost.HasValue)
                query = query.Where(m => m.Cost <= maxCost.Value);

            query = sortBy switch {
                "Date" => sortDescending ? query.OrderByDescending(m => m.Date) : query.OrderBy(m => m.Date),
                "Cost" => sortDescending ? query.OrderByDescending(m => m.Cost) : query.OrderBy(m => m.Cost),
                "EmployeeName" => sortDescending ? query.OrderByDescending(m => m.Employee.Name) : query.OrderBy(m => m.Employee.Name),
                "CarName" => sortDescending ? query.OrderByDescending(m => m.Car.Name) : query.OrderBy(m => m.Car.Name),
                _ => query.OrderByDescending(m => m.Date)
            };

            var totalMaintenances = await query.CountAsync();
            var maintenances = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MaintenanceListItemViewModel {
                    MaintenanceId = m.MaintenanceId,
                    Date = m.Date,
                    Cost = m.Cost,

                    EmployeeId = m.EmployeeId,
                    EmployeeName = m.Employee.Name,
                    EmployeeEmail = m.Employee.Email,

                    CarId = m.CarId,
                    CarName = m.Car.Name,
                    CarModel = m.Car.Model,
                    CarYear = m.Car.Year,
                    LicensePlate = m.Car.LicensePlate,
                }).ToListAsync();

            var vm = new MaintenanceIndexViewModel {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalMaintenances = totalMaintenances,

                Maintenances = maintenances
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _context.TrMaintenances
                .Where(m => m.MaintenanceId == id)
                .Select(m => new MaintenanceDetailViewModel {
                    MaintenanceId = m.MaintenanceId,
                    Date = m.Date,
                    Description = m.Description,
                    Cost = m.Cost,

                    EmployeeId = m.EmployeeId,
                    EmployeeName = m.Employee.Name,
                    EmployeeEmail= m.Employee.Email,
                    Position = m.Employee.Position,
                    PhoneNumber = m.Employee.PhoneNumber,

                    CarId = m.CarId,
                    CarName = m.Car.Name,
                    CarModel = m.Car.Model,
                    CarYear = m.Car.Year,
                    NumberOfCarSeats = m.Car.NumberOfCarSeats,
                    LicensePlate = m.Car.LicensePlate,
                    Transmission = m.Car.Transmission,
                }).FirstOrDefaultAsync();
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //ViewBag.Cars = await _context.MsCar
            //    .Select(c => new SelectListItem {
            //        Value = c.CarId.ToString(),
            //        Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
            //    })
            //    .ToListAsync();

            //ViewBag.Employees = await _context.MsEmployee
            //    .Select(e => new SelectListItem {
            //        Value = e.EmployeeId.ToString(),
            //        Text = $"{e.Name} ({e.Position})"
            //    })
            //    .ToListAsync();

            var vm = new MaintenanceCreateViewModel {
                Date = DateTime.Today,
                Cars = await _context.MsCar
                    .Select(c => new SelectListItem {
                        Value = c.CarId.ToString(),
                        Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
                    }).ToListAsync(),

                Employees = await _context.MsEmployee
                    .Select(e => new SelectListItem {
                        Value = e.EmployeeId.ToString(),
                        Text = $"{e.Name} ({e.Position})"
                    }).ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(MaintenanceCreateViewModel vm)
        {
            if (!ModelState.IsValid) {
                //ViewBag.Cars = await _context.MsCar
                //    .Select(c => new SelectListItem {
                //        Value = c.CarId.ToString(),
                //        Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
                //    })
                //    .ToListAsync();

                //ViewBag.Employees = await _context.MsEmployee
                //    .Select(e => new SelectListItem {
                //        Value = e.EmployeeId.ToString(),
                //        Text = $"{e.Name} ({e.Position})"
                //    })
                //    .ToListAsync();

                Console.WriteLine(vm.EmployeeId);
                Console.WriteLine(vm.CarId);

                vm.Cars = await _context.MsCar
                    .Select(c => new SelectListItem {
                        Value = c.CarId.ToString(),
                        Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
                    }).ToListAsync();

                vm.Employees = await _context.MsEmployee
                    .Select(e => new SelectListItem {
                        Value = e.EmployeeId.ToString(),
                        Text = $"{e.Name} ({e.Position})"
                    }).ToListAsync();

                return View(vm);
            }

            var maintenance = new TrMaintenance {
                MaintenanceId = Guid.NewGuid(),
                Date = vm.Date,
                Description = string.IsNullOrWhiteSpace(vm.Description) ? "-" : vm.Description,
                Cost = vm.Cost,
                CarId = vm.CarId,
                EmployeeId = vm.EmployeeId
            };

            _context.TrMaintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var maintenance = await _context.TrMaintenances
                .Include(m => m.Car)
                .Include(m => m.Employee)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);

            if (maintenance == null) return NotFound();

            var vm = new MaintenanceUpdateViewModel {
                MaintenanceId = maintenance.MaintenanceId,
                Date = maintenance.Date,
                Description = maintenance.Description,
                Cost = maintenance.Cost,
                CarId = maintenance.CarId,
                EmployeeId = maintenance.EmployeeId
            };

            ViewBag.Cars = await _context.MsCar
                .Select(c => new SelectListItem {
                    Value = c.CarId.ToString(),
                    Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
                }).ToListAsync();

            ViewBag.Employees = await _context.MsEmployee
                .Select(e => new SelectListItem {
                    Value = e.EmployeeId.ToString(),
                    Text = $"{e.Name} ({e.Position})"
                }).ToListAsync();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, MaintenanceUpdateViewModel vm)
        {
            if (!ModelState.IsValid) {
                ViewBag.Cars = await _context.MsCar
                    .Select(c => new SelectListItem {
                        Value = c.CarId.ToString(),
                        Text = $"{c.Name} ({c.Model}, {c.Year}) - {c.LicensePlate}"
                    }).ToListAsync();

                ViewBag.Employees = await _context.MsEmployee
                    .Select(e => new SelectListItem {
                        Value = e.EmployeeId.ToString(),
                        Text = $"{e.Name} ({e.Position})"
                    }).ToListAsync();

                return View(vm);
            }

            var maintenance = await _context.TrMaintenances
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);

            if (maintenance == null) return NotFound();

            maintenance.Date = vm.Date;
            maintenance.Description = vm.Description ?? "-";
            maintenance.Cost = vm.Cost;
            maintenance.CarId = vm.CarId;
            maintenance.EmployeeId = vm.EmployeeId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = maintenance.MaintenanceId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var maintenance = await _context.TrMaintenances
                .Include(m => m.Employee)
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
            if (maintenance == null) return NotFound();

            var vm = new MaintenanceDeleteViewModel {
                MaintenanceId = id,
                Date = maintenance.Date,
                EmployeeName = maintenance.Employee.Name,
                CarName = maintenance.Car.Name,
                CarModel = maintenance.Car.Model,
                CarYear = maintenance.Car.Year,
                LicensePlate = maintenance.Car.LicensePlate
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var maintenance = await _context.TrMaintenances
                .FirstOrDefaultAsync(m => m.MaintenanceId == id);
            if (maintenance == null) return NotFound();

            _context.TrMaintenances.Remove(maintenance);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
