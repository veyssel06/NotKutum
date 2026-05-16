using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Entity.Concrete;
using KisiselNotYonetimSistemi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    [SessionCheck]
    public class CategoryController : Controller
    {
        CategoryManager _categoryManager = new CategoryManager();

        private int GetUserID() => HttpContext.Session.GetInt32("UserID") ?? 0;

        public IActionResult Index()
        {
            var categories = _categoryManager.GetAll(x => x.UserID == GetUserID());
            ViewData["Title"] = "Kategoriler";
            return View(categories);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Yeni Kategori";
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            category.CategoryStatus = true;
            category.UserID = GetUserID();
            _categoryManager.Add(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryManager.GetById(id);
            if (category == null || category.UserID != GetUserID())
                return RedirectToAction("Index");
            ViewData["Title"] = "Kategori Düzenle";
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            category.UserID = GetUserID();
            var result = _categoryManager.Update(category);
            if (result == -1)
            {
                ViewBag.Error = "Kategori adı en az 3 karakter olmalıdır!";
                return View(category);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryManager.GetById(id);
            if (category != null && category.UserID == GetUserID())
                _categoryManager.Delete(category);
            return RedirectToAction("Index");
        }
    }
}