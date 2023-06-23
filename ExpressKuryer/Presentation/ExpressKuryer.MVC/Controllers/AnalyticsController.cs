using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    public class AnalyticsController : Controller
    {
        public IActionResult Index()
        {
            TempData["Title"] = "Analitika";
            return View();
        }
    }
}
