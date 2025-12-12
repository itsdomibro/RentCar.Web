using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.Auth;
using System.Security.Claims;

namespace RentCar.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.MsCustomer.AnyAsync(c => c.Email == model.Email)) {
                ModelState.AddModelError("", "Email has already been registered");
                return View(model);
            }

            var newCustomer = new MsCustomer {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                DriverLicenseNumber = model.DriverLicenseNumber
            };

            _context.MsCustomer.Add(newCustomer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customer = await _context.MsCustomer.FirstOrDefaultAsync(c => c.Email == model.Email);
            if (customer == null) {
                ModelState.AddModelError("", "Email is not found");
                return View(model);
            }
            if (!BCrypt.Net.BCrypt.Verify(model.Password, customer.PasswordHash)) {
                ModelState.AddModelError("", "Password does not match");
                return View(model);
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(ClaimTypes.Role, customer.Role)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index", "Home");
        }
    }
}
