using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;

namespace E_commerce_23TH0024.Controllers
{
    
    public class KhachHangs_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private Shipping_23TH0024Controller _location = new Shipping_23TH0024Controller();

        #region Dành cho khách hàng
        [Authorize]
        public ActionResult ViewProfile()
        {
            var UserID = User.Identity.GetUserId();
            var data = db.KhachHangs.SingleOrDefault(x => x.UserID.ToString() == UserID.ToString());
            return View(data);
        }
        [Authorize]
        public ActionResult UpdateProfile()
        {
            var UserID = User.Identity.GetUserId();
            var data = db.KhachHangs.SingleOrDefault(x => x.UserID.ToString() == UserID.ToString());
            if (data != null)
            {
                ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", data.CityID);
                return View(data);
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateProfile(KhachHang customer)
        {
            if (ModelState.IsValid)
            {
                var UserID = User.Identity.GetUserId();
                var data = db.KhachHangs.SingleOrDefault(x => x.UserID.ToString() == UserID.ToString());
                if (data != null)
                {
                    data.HoTen = customer.HoTen;
                    data.CityID = customer.CityID;
                    data.DiaChi = customer.DiaChi;
                    string diachi = customer.DiaChi;
                    var city = db.Cities.SingleOrDefault(x => x.Id == customer.CityID);
                    if (city != null)
                    {
                        diachi = customer.DiaChi + ", " + city.CityName;
                    }
                    try
                    {
                        var (lat1, lng1) = await _location.GetCoordinatesFromAddressAsync(diachi);
                        data.Longitude = lng1;
                        data.Latitude = lat1;
                        db.Entry(data).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin: " + ex.Message);
                        return View(customer);
                    }

                    return RedirectToAction("ViewProfile");
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng.");
                    return View(customer);
                }
            }
            return View(customer);
        }
        #endregion

        #region MyRegion
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var khachHangs = db.KhachHangs.Include(k => k.AspNetUser).Include(x => x.CustomerType);
            return View(khachHangs.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create()
        {
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "UserName");
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName");
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("MaKH,HoTen,UserID,SoDienThoai,CityID,DiaChi,CustomerTypeID")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                var city = db.Cities.SingleOrDefault(x => x.Id == khachHang.CityID);
                string diachi = khachHang.DiaChi;
                if (city != null)
                {
                    diachi = khachHang.DiaChi + ", " + khachHang.City.CityName;
                }
                var (lat1, lng1) = await _location.GetCoordinatesFromAddressAsync(diachi);
                khachHang.Longitude = lng1;
                khachHang.Latitude = lat1;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "UserName", khachHang.UserID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", khachHang.CustomerTypeID);
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "UserName", khachHang.UserID);
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", khachHang.CityID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", khachHang.CustomerTypeID);
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("MaKH,HoTen,UserID,SoDienThoai,CityID,DiaChi, CustomerTypeID")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                var existingKhachHang = db.KhachHangs.FirstOrDefault(k => k.Id == khachHang.Id);

                if (existingKhachHang != null)
                {
                    string cityName = existingKhachHang.City != null ? existingKhachHang.City.CityName : "";
                    if (existingKhachHang.CityID != khachHang.CityID)
                    {
                        var city = db.Cities.SingleOrDefault(x => x.Id == khachHang.CityID);
                        if (city != null)
                        {
                            cityName = city.CityName;
                        }
                    }
                    string diachi = khachHang.DiaChi + ", " + cityName;
                    var (lat1, lng1) = await _location.GetCoordinatesFromAddressAsync(diachi);
                    existingKhachHang.Longitude = lng1;
                    existingKhachHang.Latitude = lat1;
                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.UserID = khachHang.UserID;
                    existingKhachHang.SoDienThoai = khachHang.SoDienThoai;
                    existingKhachHang.CityID = khachHang.CityID;
                    existingKhachHang.DiaChi = khachHang.DiaChi;
                    existingKhachHang.CustomerTypeID = khachHang.CustomerTypeID;
                    db.Entry(existingKhachHang).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "UserName", khachHang.UserID);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", khachHang.CustomerTypeID);
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
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
        #endregion

    }
}
