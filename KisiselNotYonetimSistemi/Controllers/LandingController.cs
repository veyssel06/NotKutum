using KisiselNotYonetimSistemi.Business.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    public class LandingController : Controller
    {
        UserManager _userManager = new UserManager();

        public IActionResult Index()
        {
            // Zaten giriş yapmışsa direkt dashboard
            if (HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("Index", "Home");

            // Beni Hatırla token kontrolü → direkt dashboard
            if (Request.Cookies.TryGetValue("RememberToken", out string? token))
            {
                var user = _userManager.GetAll(x =>
                    x.RememberToken == token &&
                    x.TokenExpiry > DateTime.Now).FirstOrDefault();

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserID", user.UserID);
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetString("Role", user.Role ?? "user");
                    return RedirectToAction("Index", "Home");
                }
            }

            // İlk ziyaret mi?
            if (!Request.Cookies.ContainsKey("SiteVisited"))
            {
                Response.Cookies.Append("SiteVisited", "true", new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1),
                    HttpOnly = true
                });
                return View(); // Landing göster
            }

            // Daha önce gelmiş → direkt login
            return RedirectToAction("Login", "Account");
        }
    }
}