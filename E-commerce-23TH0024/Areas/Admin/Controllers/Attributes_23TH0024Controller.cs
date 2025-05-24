using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Attributes_23TH0024Controller : Controller
    {
        private ApplicationDbContext db;
        public Attributes_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Attributes_23TH0024
        public ActionResult Index()
        {
            return View(db.ProductAttributes.ToList());
        }

        // GET: Attributes_23TH0024/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductAttribute attributes = db.ProductAttributes.Find(id);
            if (attributes == null)
            {
                return NotFound();
            }
            return View(attributes);
        }

        // GET: Attributes_23TH0024/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Attributes_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,AttributeName")] ProductAttribute attributes)
        {
            if (ModelState.IsValid)
            {
                db.ProductAttributes.Add(attributes);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }

            return View(attributes);
        }

        // GET: Attributes_23TH0024/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductAttribute attributes = db.ProductAttributes.Find(id);
            if (attributes == null)
            {
                return NotFound();
            }
            return View(attributes);
        }

        // POST: Attributes_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("AttributeID,AttributeName")] ProductAttribute attributes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attributes).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Cập nhật thất bại";
            return View(attributes);
        }

        // GET: Attributes_23TH0024/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductAttribute attributes = db.ProductAttributes.Find(id);
            if (attributes == null)
            {
                return NotFound();
            }
            return View(attributes);
        }

        // POST: Attributes_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProductAttribute attributes = db.ProductAttributes.Find(id);
            db.ProductAttributes.Remove(attributes);
            TempData["SuccessMessage"] = "Xóa thành công";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
