using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NhatKyXayDung.Data;
using NhatKyXayDung.Models;

namespace NhatKyXayDung.Controllers
{
    public class NhatKyController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public NhatKyController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var model = _context.NhatKy.Include(x => x.CongTrinh).ToList();
            return View(model);
        }

        // GET: NhatKyController/Details/5
        public ActionResult Details(int id)
        {
            var model = _context.NhatKy.Include(x => x.CongTrinh).FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: NhatKyController/Create
        public ActionResult Create()
        {
            ViewBag.IdCongTrinh = new SelectList(_context.CongTrinh, "Id", "TenCongTrinh");
            return View();
        }

        // POST: NhatKyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhatKy model)
        {
            try
            {
                _context.NhatKy.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm mới nhật ký thành công";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Thêm mới nhật ký thất bại";
                return View(model);
            }
        }

        // GET: NhatKyController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.IdCongTrinh = new SelectList(_context.CongTrinh, "Id", "TenCongTrinh");
            var model = _context.NhatKy.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: NhatKyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhatKy model)
        {
            try
            {
                _context.NhatKy.Update(model);
                _context.SaveChanges();
                TempData["Success"] = "Cập nhật nhật ký thành công";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Cập nhật nhật ký thất bại";
                return View(model);
            }
        }

        // GET: NhatKyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NhatKyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
