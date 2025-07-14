using System.Data;
using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.Order;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DiscountRules_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        public DiscountRules_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
        }
        // Giảm giá theo loại khách hàng, loại khách hàng, hoặc giảm giá vận chuyển
        //Lấy DiscountAmount, DiscrountPercent
        public decimal calculateDiscount(int? ProductGroupID, int? CustomerTypeID, decimal? orderTotal)
        {
            IEnumerable<DiscountRule> data = db.DiscountRules.Where(x => (!ProductGroupID.HasValue || x.IdLoaiSanPham == ProductGroupID)
                                 && (!CustomerTypeID.HasValue || x.IdCustomerType == CustomerTypeID)
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

        public ActionResult Index()
        {
            var discountRules = db.DiscountRules.OrderByDescending(x => x.Created_at);
            return View(discountRules.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            DiscountRule discountRule = db.DiscountRules.Find(id);
            if (discountRule == null)
            {
                return NotFound();
            }
            return View(discountRule);
        }

        
        public ActionResult Create()
        {
            ViewBag.IdLoaiSanPham = new SelectList(db.LoaiSanPham, "Id", "TenLSP");
            ViewBag.IdCustomerType = new SelectList(db.CustomerTypes, "Id", "CustomerTypeName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Discount_Type,Description,MinTotalPrice,DiscountAmount," +
            "DiscrountPercent,IdLoaiSanPham,IdCustomerType,StartDate,EndDate")] DiscountRule discountRule)
        {
            if (ModelState.IsValid)
            {
                discountRule.Created_at = DateTime.Now;
                db.DiscountRules.Add(discountRule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdLoaiSanPham = new SelectList(db.LoaiSanPham, "Id", "TenLSP", discountRule.IdLoaiSanPham);
            ViewBag.IdCustomerType = new SelectList(db.CustomerTypes, "Id", "CustomerTypeName", discountRule.IdCustomerType);
            return View(discountRule);
        }

        
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
            ViewBag.IdLoaiSanPham = new SelectList(db.LoaiSanPham, "Id", "TenLSP", discountRule.IdLoaiSanPham);
            ViewBag.IdCustomerType = new SelectList(db.CustomerTypes, "Id", "CustomerTypeName", discountRule.IdCustomerType);
            return View(discountRule);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,Discount_Type,Description,MinTotalPrice,DiscountAmount,DiscountPercent,IdLoaiSanPham,IdCustomerType,StartDate,EndDate")] DiscountRule discountRule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discountRule).State = EntityState.Modified;
                db.Entry(discountRule).Property(x => x.Created_at).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLoaiSanPham = new SelectList(db.LoaiSanPham, "Id", "TenLSP", discountRule.IdLoaiSanPham);
            ViewBag.IdCustomerType = new SelectList(db.CustomerTypes, "Id", "CustomerTypeName", discountRule.IdCustomerType);
            return View(discountRule);
        }

        
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
