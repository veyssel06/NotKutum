using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    [AdminCheck]
    public class AdminController : Controller
    {
        UserManager _userManager = new UserManager();
        NoteManager _noteManager = new NoteManager();
        CategoryManager _categoryManager = new CategoryManager();

        // Dashboard — istatistikler
        public IActionResult Index()
        {
            ViewBag.TotalUsers = _userManager.GetAll(x => true).Count;
            ViewBag.TotalNotes = _noteManager.GetAll(x => true).Count;
            ViewBag.TotalCategories = _categoryManager.GetAll(x => true).Count;
            ViewBag.BannedUsers = _userManager.GetAll(x => !x.UserStatus).Count;
            ViewData["Title"] = "Admin Panel";
            return View();
        }

        // Kullanıcı listesi
        public IActionResult Users()
        {
            var users = _userManager.GetAll(x => true);
            ViewData["Title"] = "Kullanıcılar";
            return View(users);
        }

        // Banla
        public IActionResult Ban(int id)
        {
            var user = _userManager.GetById(id);
            if (user != null && user.Role != "admin")
            {
                user.UserStatus = false;
                _userManager.Update(user);
            }
            return RedirectToAction("Users");
        }

        // Ban kaldır
        public IActionResult Unban(int id)
        {
            var user = _userManager.GetById(id);
            if (user != null)
            {
                user.UserStatus = true;
                _userManager.Update(user);
            }
            return RedirectToAction("Users");
        }

        // Tüm notlar
        public IActionResult AllNotes()
        {
            var notes = _noteManager.GetAll(x => true);
            var users = _userManager.GetAll(x => true);
            ViewBag.Users = users.ToDictionary(u => u.UserID, u => u.Username);
            ViewData["Title"] = "Tüm Notlar";
            return View(notes);
        }

        // Not sil (admin)
        public IActionResult DeleteNote(int id)
        {
            var note = _noteManager.GetById(id);
            if (note != null)
                _noteManager.Delete(note);
            return RedirectToAction("AllNotes");
        }
    }
}