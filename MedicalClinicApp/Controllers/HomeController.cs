using Microsoft.AspNetCore.Mvc;

namespace MedicalClinicApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
