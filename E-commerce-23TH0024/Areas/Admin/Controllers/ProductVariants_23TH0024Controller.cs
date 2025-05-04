using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Areas.AdminControllers
{
 
    public class ProductVariants_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        // GET: ProductVariants_23TH0024
        public ActionResult Index()
        {
            var productVariants = db.ProductVariants.Include(p => p.SanPham);
            return View(productVariants.ToList());
        }

        // GET: ProductVariants_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductVariant productVariant = db.ProductVariants.Find(id);
            if (productVariant == null)
            {
                return NotFound();
            }
            return View(productVariant);
        }

        public ActionResult Create(int? ProductId = null)
        {
            var productAttributes = db.ProductAttributes.ToList();
            return PartialView("Create", productAttributes);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ProductVariant model, List<ProductVariantAttribute> attributes)
        {
            if (ModelState.IsValid)
            {
                db.ProductVariants.Add(model);
                db.SaveChanges();
                foreach (var attribute in attributes)
                {
                    attribute.IdProductVariant = model.Id;
                    db.ProductVariantAttributes.Add(attribute); 
                }
                db.SaveChanges(); 
                return Json(new { success = true, message = "Biến thể và thuộc tính đã được thêm thành công!" });
            }
            return Json(new { success = false, message = "Có lỗi xảy ra khi lưu sản phẩm hoặc thuộc tính." });
        }

        // GET: ProductVariants_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariant productVariant = db.ProductVariants.Find(id);
            if (productVariant == null)
            {
                return NotFound();
            }
            ViewBag.ProductID = new SelectList(db.SanPhams, "MaSP", "TenSP", productVariant.IdSanPham);
            return View(productVariant);
        }

        // POST: ProductVariants_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("VariantID,ProductID,StockQuantity,Price")] ProductVariant productVariant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productVariant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.SanPhams, "MaSP", "TenSP", productVariant.IdSanPham);
            return View(productVariant);
        }

        
        public ActionResult Delete(int? id)
        {
            var productVariant = db.ProductVariants.FirstOrDefault(pv => pv.Id == id);
            if (productVariant != null)
            {
                db.ProductVariants.Remove(productVariant);
                var productVariantAttributes = db.ProductVariantAttributes.Where(pva => pva.IdProductVariant == id).ToList();
                db.ProductVariantAttributes.RemoveRange(productVariantAttributes);
                db.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        // POST: ProductVariants_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductVariant productVariant = db.ProductVariants.Find(id);
            db.ProductVariants.Remove(productVariant);
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
