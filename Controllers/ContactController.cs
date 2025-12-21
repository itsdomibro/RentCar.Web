using Microsoft.AspNetCore.Mvc;

namespace RentCar.Web.Controllers
{
    public class ContactController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
