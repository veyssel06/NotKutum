using KisiselNotYonetimSistemi.Business.Concrete;
using KisiselNotYonetimSistemi.Entity.Concrete;
using KisiselNotYonetimSistemi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KisiselNotYonetimSistemi.Controllers
{
    [SessionCheck]
    public class NoteController : Controller
    {
        NoteManager _noteManager = new NoteManager();
        CategoryManager _categoryManager = new CategoryManager();

        private int GetUserID() => HttpContext.Session.GetInt32("UserID") ?? 0;

        public IActionResult Index()
        {
            var notes = _noteManager.GetAll(x => x.UserID == GetUserID()
                                              && !x.IsArchived
                                              && !x.IsDeleted)
                                    .OrderByDescending(x => x.IsPinned)
                                    .ThenByDescending(x => x.CreatedDate)
                                    .ToList();
            ViewData["Title"] = "Notlarım";
            return View(notes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = _categoryManager.GetAll(x => x.UserID == GetUserID());
            ViewData["Title"] = "Yeni Not";
            return View();
        }

        [HttpPost]
        public IActionResult Add(Note note)
        {
            var userId = GetUserID();
            note.UserID = userId;
            note.CreatedDate = DateTime.Now;
            note.NoteStatus = true;
            if (note.CategoryID == 0)
                note.CategoryID = null;
            var result = _noteManager.Add(note);
            if (result == -1)
            {
                ViewBag.Error = "Başlık en az 3 karakter olmalıdır!";
                ViewBag.Categories = _categoryManager.GetAll(x => x.UserID == userId);
                return View(note);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note == null || note.UserID != userId)
                return RedirectToAction("Index");
            ViewBag.Categories = _categoryManager.GetAll(x => x.UserID == userId);
            ViewData["Title"] = "Not Düzenle";
            return View(note);
        }

        [HttpPost]
        public IActionResult Edit(Note note)
        {
            var userId = GetUserID();
            note.UserID = userId;
            note.NoteStatus = true;
            var result = _noteManager.Update(note);
            if (result == -1)
            {
                ViewBag.Error = "Başlık en az 3 karakter olmalıdır!";
                ViewBag.Categories = _categoryManager.GetAll(x => x.UserID == userId);
                return View(note);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note == null || note.UserID != userId)
                return RedirectToAction("Index");
            ViewData["Title"] = note.Title;
            return View(note);
        }

        public IActionResult Delete(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
                _noteManager.Delete(note);
            return RedirectToAction("Index");
        }

        public IActionResult Search(string q)
        {
            var userId = GetUserID();
            var notes = string.IsNullOrEmpty(q)
                ? new List<Note>()
                : _noteManager.GetAll(x => x.UserID == userId &&
                    (x.Title.Contains(q) || x.Content.Contains(q)));
            ViewBag.Query = q;
            ViewData["Title"] = "Arama Sonuçları";
            return View(notes);
        }
        public IActionResult Archive(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
            {
                note.IsArchived = true;
                _noteManager.Update(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult SoftDelete(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
            {
                note.IsDeleted = true;
                _noteManager.Update(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult ArchivedNotes()
        {
            var notes = _noteManager.GetAll(x => x.UserID == GetUserID()
                                              && x.IsArchived
                                              && !x.IsDeleted);
            ViewData["Title"] = "Arşiv";
            return View(notes);
        }
        public IActionResult Trash()
        {
            var notes = _noteManager.GetAll(x => x.UserID == GetUserID()
                                              && x.IsDeleted);
            ViewData["Title"] = "Çöp Kutusu";
            return View(notes);
        }
        public IActionResult Restore(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
            {
                note.IsArchived = false;
                note.IsDeleted = false;
                _noteManager.Update(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult HardDelete(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
                _noteManager.Delete(note);
            return RedirectToAction("Trash");
        }
        public IActionResult Pin(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
            {
                note.IsPinned = true;
                _noteManager.Update(note);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Unpin(int id)
        {
            var userId = GetUserID();
            var note = _noteManager.GetById(id);
            if (note != null && note.UserID == userId)
            {
                note.IsPinned = false;
                _noteManager.Update(note);
            }
            return RedirectToAction("Index");
        }
        public IActionResult SearchJson(string q)
        {
            var userId = GetUserID();
            if (string.IsNullOrEmpty(q))
                return Json(new List<object>());

            var notes = _noteManager.GetAll(x => x.UserID == userId
                                              && !x.IsArchived
                                              && !x.IsDeleted
                                              && (x.Title.Contains(q) || x.Content.Contains(q)))
                                    .Select(x => new {
                                        x.NoteID,
                                        x.Title,
                                        x.Content
                                    })
                                    .ToList();

            return Json(notes);
        }
    }
}