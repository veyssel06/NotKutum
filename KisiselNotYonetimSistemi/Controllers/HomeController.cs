using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    [SessionCheck]
    public class HomeController : Controller
    {
        NoteManager _noteManager = new NoteManager();
        CategoryManager _categoryManager = new CategoryManager();

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            ViewBag.TotalNotes = _noteManager.GetAll(x => x.UserID == userId
                                                       && !x.IsArchived
                                                       && !x.IsDeleted).Count;
            ViewBag.TotalCategories = _categoryManager.GetAll(x => x.UserID == userId).Count;
            ViewBag.RecentNotes = _noteManager.GetAll(x => x.UserID == userId
                                                        && !x.IsArchived
                                                        && !x.IsDeleted)
                                              .OrderByDescending(x => x.CreatedDate)
                                              .Take(5)
                                              .ToList();

            var notlar = _noteManager.GetAll(x => x.UserID == userId
                                               && !x.IsArchived
                                               && !x.IsDeleted);

            var kategoriGruplar = notlar
                .GroupBy(x => x.Category != null ? x.Category.CategoryName : "Kategorisiz")
                .Select(g => new { Kategori = g.Key, Sayi = g.Count() })
                .ToList();

            ViewBag.KategoriLabels = kategoriGruplar.Select(x => x.Kategori).ToList();
            ViewBag.KategoriData = kategoriGruplar.Select(x => x.Sayi).ToList();

            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}