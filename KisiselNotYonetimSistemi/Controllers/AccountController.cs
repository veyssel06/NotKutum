using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    public class AccountController : Controller
    {
        UserManager _userManager = new UserManager();

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, bool rememberMe = false)
        {
            string hashedPassword = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(password)));

            var user = _userManager.GetAll(x =>
                x.Username == username &&
                x.Password == hashedPassword).FirstOrDefault();

            if (user != null)
            {
                if (!user.UserStatus)
                {
                    ViewBag.Error = "Hesabınız engellenmiştir. Lütfen yönetici ile iletişime geçin.";
                    return View();
                }

                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role ?? "user");

                if (rememberMe)
                {
                    string newToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
                    user.RememberToken = newToken;
                    user.TokenExpiry = DateTime.Now.AddDays(30);
                    _userManager.Update(user);

                    Response.Cookies.Append("RememberToken", newToken, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            user.UserStatus = true;
            user.Role = "User";
            user.Password = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(user.Password)));
            _userManager.Add(user);
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var user = _userManager.GetById(userId);
            if (user != null)
            {
                user.RememberToken = null;
                user.TokenExpiry = null;
                _userManager.Update(user);
            }

            Response.Cookies.Delete("RememberToken");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult DeleteAccount()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var user = _userManager.GetById(userId);
            if (user != null)
            {
                _userManager.Delete(user);
                HttpContext.Session.Clear();
            }
            Response.Cookies.Delete("RememberToken");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult CheckUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Json(new { available = false });

            var exists = _userManager.GetAll(x => x.Username == username).Any();
            return Json(new { available = !exists });
        }
    }
}