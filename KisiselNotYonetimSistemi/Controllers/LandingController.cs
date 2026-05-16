using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}