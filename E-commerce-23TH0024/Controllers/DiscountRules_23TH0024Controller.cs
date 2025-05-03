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

namespace E_commerce_23TH0024.Controllers
{
    public class DiscountRules_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        // Giảm giá theo loại khách hàng, loại khách hàng, hoặc giảm giá vận chuyển
        //Lấy DiscountAmount, DiscrountPercent
        public decimal calculateDiscount(int? ProductGroupID, int? CustomerTypeID, decimal? orderTotal)
        {
           IEnumerable<DiscountRule> data = db.DiscountRules.Where(x => (!ProductGroupID.HasValue || x.ProductGroupID == ProductGroupID)
                                && (!CustomerTypeID.HasValue || x.CustomerTypeID == CustomerTypeID)
                                && (!orderTotal.HasValue || x.MinTotalPrice <= orderTotal)       
                                );
            decimal discount = 0;
            return discount;
        }
        public ActionResult DeleteAll()
        {
            var discountRules = db.DiscountRules.ToList();
            if (discountRules.Any())
            {
                db.DiscountRules.RemoveRange(discountRules);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Tất cả các bản ghi đã được xóa thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không có bản ghi nào để xóa!";
            }
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Index()
        {
            var discountRules = db.DiscountRules.Include(d => d.LoaiSanPham).OrderByDescending(x => x.Created_at);
            return View(discountRules.ToList());
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new  BadRequestResult();
            }
            DiscountRule discountRule = db.DiscountRules.Find(id);
            if (discountRule == null)
            {
                return NotFound();
            }
            return View(discountRule);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Create()
        {
            ViewBag.ProductGroupID = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName");
            return View();
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("RuleID,Name,Discount_Type,Description,MinTotalPrice,DiscountAmount," +
            "DiscrountPercent,ProductGroupID,CustomerTypeID,StartDate,EndDate")] DiscountRule discountRule)
        {
            if (ModelState.IsValid)
            {
                discountRule.Created_at = DateTime.Now;
                db.DiscountRules.Add(discountRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupID = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", discountRule.ProductGroupID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", discountRule.CustomerTypeID);
            return View(discountRule);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountRule discountRule = db.DiscountRules.Find(id);
            if (discountRule == null)
            {
                return NotFound();
            }
            ViewBag.ProductGroupID = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", discountRule.ProductGroupID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", discountRule.CustomerTypeID);
            return View(discountRule);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("RuleID,Name,Discount_Type,Description,MinTotalPrice,DiscountAmount,DiscountPercent,ProductGroupID,CustomerTypeID,StartDate,EndDate")] DiscountRule discountRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discountRule).State = EntityState.Modified;
                db.Entry(discountRule).Property(x => x.Created_at).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupID = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", discountRule.ProductGroupID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", discountRule.CustomerTypeID);
            return View(discountRule);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountRule discountRule = db.DiscountRules.Find(id);
            if (discountRule == null)
            {
                return NotFound();
            }
            return View(discountRule);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiscountRule discountRule = db.DiscountRules.Find(id);
            if (discountRule == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy chương trình giảm giá để xóa!";
                return RedirectToAction("Index");
            }
            db.DiscountRules.Remove(discountRule);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa chương trình giảm giá thành công!";
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
