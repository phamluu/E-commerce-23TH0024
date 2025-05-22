using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_23TH0024.Areas.Admin.Controllers;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Service;
using E_commerce_23TH0024.Lib.Enums;
using E_commerce_23TH0024.Lib;

namespace E_commerce_23TH0024.Areas.AdminControllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DonHangs_23TH0024Controller : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DonHangService _service;
        private readonly ApplicationDbContext db;
        private readonly Shipping_23TH0024Controller _shipping;
        public DonHangs_23TH0024Controller(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            db = context;
            _shipping = new Shipping_23TH0024Controller(db);
            _service = new DonHangService(db);
        }
        public ActionResult OrderListForCustomer()
        {
            string UserID = User.Identity.GetUserId();
           
                var kh = db.KhachHang.SingleOrDefault(x => x.IdAspNetUsers == UserID);
                if (kh != null)
                {
                    var donhangs = db.DonHangs.FromSqlRaw("EXEC GetDonHangs @UserID, @SoDienThoai, @Email",
              new SqlParameter("@UserID", (object)UserID ?? DBNull.Value),
              new SqlParameter("@SoDienThoai", DBNull.Value),
              new SqlParameter("@Email", DBNull.Value)
              ).ToList();
                    if (donhangs.Count == 0)
                        ViewBag.TB = "Không có thông tin tìm kiếm.";
                    return View(donhangs);
                }
                else
                {
                    ViewBag.TB = "Không tồn tài khách hàng";
                }
              
            return View();
        }
        private Cart GetCart()
        {
            var cartCookie = _contextAccessor.HttpContext.Request.Cookies["Cart"];
            if (cartCookie != null)
            {
                var cartData = cartCookie;
                var cart = JsonConvert.DeserializeObject<Cart>(cartData);
                foreach (var item in cart.Items)
                {
                    var product = db.SanPham.SingleOrDefault(x => x.Id == item.Id);
                    if (product != null)
                    {
                        item.Anh = product.Anh;
                        item.TenSP = product.TenSP;
                        item.DonGia = product.DonGia.Value;
                        item.LoaiSanPham = product.LoaiSanPham;
                    }
                }
                return cart;
            }
            return new Cart();
        }
        public decimal CalculateShippingFee(int shippingMethod)
        {
            double lat = 12.2797806597436;
            double lng = 109.199100989104;
            string UserID = User.Identity.GetUserId();
            var kh = db.KhachHang.SingleOrDefault(x => x.IdAspNetUsers.ToString() == UserID);
            if (kh == null)
            {
                return 0;
            }
            double distance = _shipping.CalculateDistance((double)kh.Latitude, (double)kh.Longitude, lat, lng);
            
            var fees = db.ShippingRates.Where(x =>  x.IdDeliveryMethod.HasValue && x.IdDeliveryMethod.Value == shippingMethod
                                                        && x.FromDistance.HasValue && x.FromDistance.Value <= distance
                                                        && (x.ToDistance.HasValue ? x.ToDistance.Value >= distance : true)
                                                      );
            if (fees != null && fees.Any())
            {
                return Math.Round(fees.Min(x => x.FixedPrice + x.PricePerKm * (decimal)distance), 2);
            }
            return 0;
        }
        [AllowAnonymous]
        public async Task<ActionResult> Order()
        {
            DonHang donHang = new DonHang();
            var cart = GetCart();
            if (cart.Items.Count == 0)
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }
            if (!cart.Total.HasValue)
            {
                TempData["Message"] = "Không tính được tổng tiền sản phẩm";
                return RedirectToAction("Index");
            }

            donHang.TotalProductAmount =cart.Total.Value;

            var DeliveryMethods = db.DeliveryMethods.Where(x => x.ActiveStatus == true);
            ViewBag.shippingMethod = new SelectList(DeliveryMethods, "ShippingMethodID", "MethodName");
            if (User.Identity.IsAuthenticated && User.IsInRole("khachhang"))
            {
                var UserID = User.Identity.GetUserId();
                var khachhang = db.KhachHang.SingleOrDefault(x => x.IdAspNetUsers.ToString() == UserID);
                if (khachhang == null)
                {
                    //return RedirectToAction("Login", "Account");
                }
                donHang.KhachHang = khachhang;
                if (DeliveryMethods.Any())
                {
                    donHang.ShippingFee = CalculateShippingFee(DeliveryMethods.First().Id);
                    donHang.TotalAmount = donHang.TotalProductAmount + donHang.ShippingFee;
                }
                else
                {
                    donHang.ShippingFee = 0;
                    donHang.TotalAmount = donHang.TotalProductAmount;
                }
            }
            return View(donHang);
        }

        
        [HttpPost]
        public async Task<ActionResult> Order(int MaKH, int shippingMethod)
        {
            DonHang donHang = new DonHang();
            var khachhang = db.KhachHang.SingleOrDefault(x => x.Id == MaKH);
            try
            {
                donHang.NgayDatHang = DateTime.Now;
                donHang.IdKhachHang = MaKH;
                donHang.IdDeliveryMethod = shippingMethod;
                List<CartItem> cartItems = GetCart().Items;
                decimal Total = 0;
                foreach (var item in cartItems)
                {
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        IdSanPham = item.Id,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia.Value,
                        DiscountApplied = item.DiscountMax()
                    };
                    donHang.ChiTietDonHangs.Add(chiTietDonHang);
                    if (chiTietDonHang.DiscountApplied > 0)
                    {
                        Total += chiTietDonHang.SoLuong * (chiTietDonHang.DonGia - chiTietDonHang.DiscountApplied);
                    }
                    else
                    {
                        Total += chiTietDonHang.SoLuong * chiTietDonHang.DonGia;
                    }
                    
                }

                donHang.ShippingFee = CalculateShippingFee(shippingMethod);
                donHang.TotalProductAmount = Total;
                donHang.TotalAmount = Total + donHang.VAT + donHang.ShippingFee;
                db.DonHangs.Add(donHang);
                await db.SaveChangesAsync();

                //return RedirectToAction("Checkout", new { id = donHang.SoHD });
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
            ViewBag.shippingMethod = new SelectList(db.DeliveryMethods, "ShippingMethodID", "MethodName", shippingMethod);
            donHang.KhachHang = khachhang;
            return View(donHang);
        }
       
        public ActionResult Checkout(int id)
        {
            var donHang = db.DonHangs.Include(d => d.ChiTietDonHangs).FirstOrDefault(d => d.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }
        
        public ActionResult Index()
        {
            var donHangs = _service.GetDonHangs();
            return View(donHangs);
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            DonHangViewModel donHang = _service.GetDonHangViewModel(id.Value);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHang, "IdKhachHang", "HoTen");
            ViewBag.MaNVDuyet = new SelectList(db.NhanVien, "IdNhanVien", "HoTen");
            ViewBag.MaNVGH = new SelectList(db.NhanVien, "IdNhanVien", "HoTen");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,NgayDatHang,NgayGiaoHang,IdKhachHang,IdNhanVienDuyet,IdNhanVienGiao,TinhTrang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHang, "MaKH", "HoTen", donHang.IdKhachHang);
            ViewBag.MaNVDuyet = new SelectList(db.NhanVien, "MaNV", "HoTen", donHang.IdNhanVienDuyet);
            ViewBag.MaNVGH = new SelectList(db.NhanVien, "MaNV", "HoTen", donHang.IdNhanVienGiao);
            return View(donHang);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); 
            }
            DonHang donHang = _service.GetDonHang(id.Value);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewBag.IdKhachHang = new SelectList(db.KhachHang, "Id", "HoTen", donHang.IdKhachHang);
            ViewBag.IdNhanVienDuyet = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienDuyet);
            ViewBag.IdNhanVienGiao = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienGiao);
            ViewBag.TinhTrang = new SelectList(
                                    Enum.GetValues(typeof(OrderStatus))
                                        .Cast<OrderStatus>()
                                        .Select(e => new { Id = (int)e, Name = e.GetDisplayName() }),
                                    "Id",
                                    "Name",
                                    donHang.TinhTrang
                                );
            return View(donHang);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,NgayDatHang,NgayGiaoHang,IdKhachHang,IdNhanVienDuyet,IdNhanVienGiao,TinhTrang")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateDonHang(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKhachHang = new SelectList(db.KhachHang, "Id", "HoTen", donHang.IdKhachHang);
            ViewBag.IdNhanVienDuyet = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienDuyet);
            ViewBag.IdNhanVienGiao = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienGiao);
            ViewBag.TinhTrang = new SelectList(
                    Enum.GetValues(typeof(OrderStatus))
                        .Cast<OrderStatus>()
                        .Select(e => new { Id = (int)e, Name = e.ToString() }),
                    "Id",
                    "Name",
                    donHang.TinhTrang
                );
            return View(donHang);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return NotFound();
            }
            return View(donHang);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng để xóa!";
                return RedirectToAction("Index");
            }
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
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
