using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Employee;
using RentCar.Web.ViewModels.Maintenances;

namespace RentCar.Web.Controllers
{
    [Authorize(Roles = "Owner")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.MsEmployee.AsQueryable();

            if (!String.IsNullOrEmpty(search)) {
                query = query.Where(e => EF.Functions.Like(e.Name, $"%{search}%"));
            }

            var totalEmployees = await query.CountAsync();
            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EmployeeListItemViewModel {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Position = e.Position,
                    TotalMaintenances = _context.TrMaintenances.Count(m => m.EmployeeId == e.EmployeeId)
                }).ToListAsync();

            var vm = new EmployeeIndexViewModel {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalEmployees = totalEmployees,

                Search = search,

                Employees = employees,
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var vm = await _context.MsEmployee
                .Where(e => e.EmployeeId == id)
                .Select(e => new EmployeeDetailsViewModel {
                    EmployeeId = e.EmployeeId,
                    Name = e.Name,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Position = e.Position,
                    TotalMaintenances = e.Maintenances.Count(),

                    Maintenances = e.Maintenances.Select(m => new MaintenanceListItemViewModel {
                        MaintenanceId = m.MaintenanceId,
                        Date = m.Date,
                        Cost = m.Cost,
                        EmployeeId = e.EmployeeId,
                        EmployeeName = e.Name,
                        EmployeeEmail = e.Email,
                        CarId = m.CarId,
                        CarName = m.Car.Name,
                        CarModel = m.Car.Model,
                        CarYear = m.Car.Year,
                        LicensePlate = m.Car.LicensePlate
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (await _context.MsEmployee.AnyAsync(e => e.Email == vm.Email)) {
                ModelState.AddModelError("Email", "That email already exists");
                return View(vm);
            }

            var employee = new MsEmployee {
                Name = vm.Name,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Position = vm.Position
            };

            _context.MsEmployee.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var employee = await _context.MsEmployee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();

            var vm = new EmployeeUpdateViewModel {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, EmployeeUpdateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if (await _context.MsEmployee.AnyAsync(e => e.Email == vm.Email && e.EmployeeId != id)) {
                ModelState.AddModelError("Email", "That email already exists");
                return View(vm);
            }

            var employee = await _context.MsEmployee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();

            employee.Name = vm.Name;
            employee.Email = vm.Email;
            employee.PhoneNumber = vm.PhoneNumber;
            employee.Position = vm.Position;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = employee.EmployeeId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.MsEmployee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();

            var vm = new EmployeeDeleteViewModel {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Email = employee.Email
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _context.MsEmployee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();

            _context.MsEmployee.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
