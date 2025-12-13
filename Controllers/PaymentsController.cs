using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Web.Data;
using RentCar.Web.Models.Entities;
using RentCar.Web.ViewModels.PaymentController;

namespace RentCar.Web.Controllers
{
    [Authorize(Roles = "Owner")]
    public class PaymentsController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await _context.LtPayment
                .Include(p => p.Rental)
                .AsNoTracking()
                .Select(p => new PaymentIndexViewModel {
                    PaymentId = p.PaymentId,
                    Date = p.Date,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.Rental.PaymentStatus,
                    RentalId = p.RentalId,
                    TotalPrice = p.Rental.TotalPrice
                })
                .ToListAsync();

            return View(payments);
        }

        [HttpGet]
        public IActionResult Create(Guid rentalId)
        {
            var vm = new CreatePaymentViewModel {
                RentalId = rentalId,
                Date = DateTime.UtcNow
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var payment = new LtPayment {
                PaymentId = Guid.NewGuid(),
                Date = vm.Date,
                Amount = vm.Amount,
                PaymentMethod = vm.PaymentMethod,
                RentalId = vm.RentalId
            };

            _context.LtPayment.Add(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var payment = await _context.LtPayment
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment is null)
                return NotFound();

            var vm = new UpdatePaymentViewModel {
                PaymentId = payment.PaymentId,
                Date = payment.Date,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                RentalId = payment.RentalId
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdatePaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var payment = await _context.LtPayment.FirstOrDefaultAsync(p => p.PaymentId == vm.PaymentId);
            if (payment is null)
                return NotFound();

            payment.Date = vm.Date;
            payment.Amount = vm.Amount;
            payment.PaymentMethod = vm.PaymentMethod;
            payment.RentalId = vm.RentalId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var payment = await _context.LtPayment
                .Include(p => p.Rental)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentId == id);

            if (payment is null)
                return NotFound();

            var vm = new DeletePaymentViewModel {
                PaymentId = payment.PaymentId,
                Date = payment.Date,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                TotalPrice = payment.Rental.TotalPrice,
                PaymentStatus = payment.Rental.PaymentStatus
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var payment = await _context.LtPayment.FindAsync(id);
            if (payment is null)
                return NotFound();

            _context.LtPayment.Remove(payment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}