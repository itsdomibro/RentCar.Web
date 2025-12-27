using Microsoft.AspNetCore.Mvc;

namespace RentCar.Web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
