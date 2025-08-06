using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NhatKyXayDung.Data;
using NhatKyXayDung.Models;

namespace NhatKyXayDung.Controllers
{
    public class CongTrinhController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public CongTrinhController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var model = _context.CongTrinh.ToList();
            return View(model);
        }

        // GET: CongTrinhController/Details/5
        public ActionResult Details(int id)
        {
            var model = _context.CongTrinh.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: CongTrinhController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CongTrinhController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CongTrinh model)
        {
            try
            {
                _context.CongTrinh.Add(model);
                _context.SaveChanges();
                TempData["Success"] = "Thêm mới công trình thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Thêm mới công trình thất bại";
                return View(model);
            }
        }

        // GET: CongTrinhController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _context.CongTrinh.Find(id); 
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: CongTrinhController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CongTrinh model)
        {
            try
            {
                _context.CongTrinh.Update(model);
                _context.SaveChanges();
                TempData["Success"] = "Cập nhật công trình thành công";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["Error"] = "Cập nhật công trình thất bại";
                return View(model);
            }
        }

        // GET: CongTrinhController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CongTrinhController/Delete/5
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
