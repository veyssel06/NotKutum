using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    [SessionCheck]
    public class ProfileController : Controller
    {
        UserManager _userManager = new UserManager();
        NoteManager _noteManager = new NoteManager();

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var user = _userManager.GetById(userId);
            ViewBag.TotalNotes = _noteManager.GetAll(x => x.UserID == userId && !x.IsDeleted).Count;
            ViewBag.ArchivedNotes = _noteManager.GetAll(x => x.UserID == userId && x.IsArchived && !x.IsDeleted).Count;
            ViewBag.DeletedNotes = _noteManager.GetAll(x => x.UserID == userId && x.IsDeleted).Count;
            ViewData["Title"] = "Profilim";
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUsername(string newUsername)
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var user = _userManager.GetById(userId);

            if (string.IsNullOrWhiteSpace(newUsername) || newUsername.Length < 3)
            {
                TempData["Error"] = "Kullanıcı adı en az 3 karakter olmalıdır.";
                return RedirectToAction("Index");
            }

            var exists = _userManager.GetAll(x => x.Username == newUsername && x.UserID != userId).Any();
            if (exists)
            {
                TempData["Error"] = "Bu kullanıcı adı zaten kullanılıyor.";
                return RedirectToAction("Index");
            }

            user.Username = newUsername;
            _userManager.Update(user);
            HttpContext.Session.SetString("Username", newUsername);
            TempData["Success"] = "Kullanıcı adı güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdatePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var user = _userManager.GetById(userId);

            string hashedCurrent = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(currentPassword)));

            if (user.Password != hashedCurrent)
            {
                TempData["Error"] = "Mevcut şifre hatalı.";
                return RedirectToAction("Index");
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "Yeni şifreler eşleşmiyor.";
                return RedirectToAction("Index");
            }

            if (newPassword.Length < 6)
            {
                TempData["Error"] = "Yeni şifre en az 6 karakter olmalıdır.";
                return RedirectToAction("Index");
            }

            user.Password = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(newPassword)));
            _userManager.Update(user);
            TempData["Success"] = "Şifre başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}