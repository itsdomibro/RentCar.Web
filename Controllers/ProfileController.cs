using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Migrations;
using RentCar.Web.ViewModels.Profile;
using System.Security.Claims;

namespace RentCar.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var customer = await _context.MsCustomer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == userId);

            if (customer is null) return NotFound();

            var vm = new ProfileIndexViewModel {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                DriverLicenseNumber = customer.DriverLicenseNumber,
                Role = customer.Role
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var customer = await _context.MsCustomer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerId == userId);

            if (customer is null) return NotFound();

            var vm = new ProfileUpdateViewModel {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                DriverLicenseNumber = customer.DriverLicenseNumber,
                Role = customer.Role
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProfileUpdateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var customer = await _context.MsCustomer.FirstOrDefaultAsync(c => c.CustomerId == userId);
            if (customer is null) return NotFound();

            customer.Email = vm.Email;
            customer.Name = vm.Name;
            customer.PhoneNumber = vm.PhoneNumber;
            customer.Address = vm.Address;
            customer.DriverLicenseNumber = vm.DriverLicenseNumber;
            customer.Role = vm.Role;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ProfileChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var userId = GetUserId();
            if (userId is null) return Unauthorized();

            var customer = await _context.MsCustomer.FirstOrDefaultAsync(c => c.CustomerId == userId);
            if (customer is null) return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(vm.OldPassword, customer.PasswordHash))
                return Unauthorized();

            if (vm.OldPassword == vm.NewPassword) {
                ModelState.AddModelError("", "New password must be different from old password.");
                return View(vm);
            }

            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.NewPassword);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private Guid? GetUserId()
        {
            var claim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(claim, out var id) ? id : null;
        }
    }
}
