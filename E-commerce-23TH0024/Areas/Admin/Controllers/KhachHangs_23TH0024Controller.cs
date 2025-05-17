using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Owin.BuilderProperties;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "admin")]
    public class KhachHangs_23TH0024Controller : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly Shipping_23TH0024Controller _location;
        public KhachHangs_23TH0024Controller(ApplicationDbContext context) : base(context)
        {
            _location = new Shipping_23TH0024Controller(db);
        }

        #region Dành cho khách hàng
        [Authorize]
        public ActionResult ViewProfile()
        {
            var UserID = User.Identity.GetUserId();
            var data = db.KhachHangs.SingleOrDefault(x => x.IdAspNetUsers.ToString() == UserID.ToString());
            return View(data);
        }
        [Authorize]
        public ActionResult UpdateProfile()
        {
            var UserID = User.Identity.GetUserId();
            var data = db.KhachHangs.SingleOrDefault(x => x.IdAspNetUsers.ToString() == UserID.ToString());
            if (data != null)
            {
                ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName", data.IdCity);
                return View(data);
            }
            ViewBag.IdCity = new SelectList(db.Cities, "IdCity", "CityName");
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
                var data = db.KhachHangs.SingleOrDefault(x => x.IdAspNetUsers.ToString() == UserID.ToString());
                if (data != null)
                {
                    data.HoTen = customer.HoTen;
                    data.IdCity = customer.IdCity;
                    data.DiaChi = customer.DiaChi;
                    string diachi = customer.DiaChi;
                    var city = db.Cities.SingleOrDefault(x => x.Id == customer.IdCity);
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
        
        public ActionResult Index()
        {
            var khachhangs = db.KhachHangs.ToList();
            var userIds = khachhangs
                .Where(nv => nv.IdAspNetUsers != null)
                .Select(nv => nv.IdAspNetUsers)
                .Distinct()
                .ToList();

            var users = db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u);

            var viewModel = khachhangs.Select(nv => new KhachHangViewModel
            {
                KhachHang = nv,
                AspNetUser = nv.IdAspNetUsers != null && users.ContainsKey(nv.IdAspNetUsers)
                    ? users[nv.IdAspNetUsers]
                    : null,
                UserName = nv.IdAspNetUsers != null && users.ContainsKey(nv.IdAspNetUsers)
                    ? users[nv.IdAspNetUsers].UserName
                    : null
            }).ToList();
            return View(viewModel);
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
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName");
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
                var city = db.Cities.SingleOrDefault(x => x.Id == khachHang.IdCity);
                string diachi = khachHang.DiaChi;
                if (city != null)
                {
                    diachi = khachHang.DiaChi;
                        //+ ", " + khachHang.City.CityName;
                }
                var (lat1, lng1) = await _location.GetCoordinatesFromAddressAsync(diachi);
                khachHang.Longitude = lng1;
                khachHang.Latitude = lat1;
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.City = new SelectList(db.Cities, "CityID", "CityName");
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", khachHang.IdAspNetUsers);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", khachHang.IdCustomerType);
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
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", khachHang.IdAspNetUsers);
            ViewBag.CityID = new SelectList(db.Cities, "Id", "CityName", khachHang.IdCity);
            ViewBag.IdCustomerType = new SelectList(db.CustomerTypes, "Id", "CustomerTypeName", khachHang.IdCustomerType);
            return View(khachHang);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,HoTen,IdAspNetUsers,SoDienThoai,CityID,DiaChi, IdCustomerType")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                var existingKhachHang = db.KhachHangs.FirstOrDefault(k => k.Id == khachHang.Id);

                if (existingKhachHang != null)
                {
                    string cityName = "";
                    //string cityName = existingKhachHang.City != null ? existingKhachHang.City.CityName : "";
                    //if (existingKhachHang.IdCity != khachHang.IdCity)
                    //{
                    //    var city = db.Cities.SingleOrDefault(x => x.Id == khachHang.IdCity);
                    //    if (city != null)
                    //    {
                    //        cityName = city.CityName;
                    //    }
                    //}
                    string diachi = khachHang.DiaChi + ", " + cityName;
                    var (lat1, lng1) = await _location.GetCoordinatesFromAddressAsync(diachi);
                    existingKhachHang.Longitude = lng1;
                    existingKhachHang.Latitude = lat1;
                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.IdAspNetUsers = khachHang.IdAspNetUsers;
                    existingKhachHang.SoDienThoai = khachHang.SoDienThoai;
                    existingKhachHang.IdCity = khachHang.IdCity;
                    existingKhachHang.DiaChi = khachHang.DiaChi;
                    existingKhachHang.IdCustomerType = khachHang.IdCustomerType;
                    db.Entry(existingKhachHang).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", khachHang.IdAspNetUsers);
            ViewBag.CustomerTypeID = new SelectList(db.CustomerTypes, "CustomerTypeID", "CustomerTypeName", khachHang.IdCustomerType);
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
