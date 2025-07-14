using E_commerce_23TH0024.Areas.Admin.Controllers;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Lib.Enums;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Ecommerce;
using E_commerce_23TH0024.Models.Order;
using E_commerce_23TH0024.Service;
using E_commerce_23TH0024.Service.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace E_commerce_23TH0024.Controllers
{

    public class DonHangs_23TH0024Controller : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DonHangService _service;
        private readonly ApplicationDbContext db;
        private readonly Shipping_23TH0024Controller _shipping;
        private readonly MoMoPaymentService _moPaymentService;
        private readonly VietQRPaymentService _vietQRPaymentService;
        public DonHangs_23TH0024Controller(ApplicationDbContext context, 
            IHttpContextAccessor contextAccessor,
            MoMoPaymentService moMoPaymentService,
            VietQRPaymentService vietQRPaymentService,
            DonHangService donHangService
            )
        {
            db = context;
            _service = donHangService;
            _contextAccessor = contextAccessor;
            _shipping = new Shipping_23TH0024Controller(db);
            _moPaymentService = moMoPaymentService;
            _vietQRPaymentService = vietQRPaymentService;
        }
        public ActionResult OrderListForCustomer()
        {
            string UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            string UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
            ViewBag.shippingMethod = new SelectList(DeliveryMethods, "Id", "MethodName");
            if (User.Identity.IsAuthenticated && User.IsInRole("khachhang"))
            {
                var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        public async Task<ActionResult> Order(int MaKH, string SoDienThoai, string DiaChi, int? shippingMethod)
        {
            DonHang donHang = new DonHang();
            var khachhang = db.KhachHang.Find(MaKH);
            if (khachhang == null)
            {
                var newKH = new KhachHang();
                newKH.SoDienThoai = SoDienThoai;
                newKH.DiaChi = DiaChi;
                db.KhachHang.Add(newKH);
                db.SaveChanges();
                MaKH = newKH.Id;
            }
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
                        //DiscountApplied = item.DiscountMax()
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

                //donHang.ShippingFee = CalculateShippingFee(shippingMethod);
                donHang.TotalProductAmount = Total;
                donHang.TotalAmount = Total + donHang.VAT + donHang.ShippingFee;
                db.DonHangs.Add(donHang);
                await db.SaveChangesAsync();

                return RedirectToAction("Checkout", new { id = donHang.Id });
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
            ViewBag.shippingMethod = new SelectList(db.DeliveryMethods, "Id", "MethodName", shippingMethod);
            donHang.KhachHang = khachhang;
            return View(donHang);
        }
        
        public async Task<IActionResult> Checkout(int id)
        {
            var donHang = _service.GetDonHang(id);
            if (donHang == null)
            {
                return NotFound();
            }
            var uniqueOrderId = $"{donHang.Id}_{DateTime.UtcNow.Ticks}";
            var response  = await _moPaymentService.GenerateMoMoQRCode((int)donHang.TotalAmount, uniqueOrderId);

            if (!string.IsNullOrEmpty(response.PayUrl))
            {
                ViewBag.QRCodeImage = $"https://api.qrserver.com/v1/create-qr-code/?data={Uri.EscapeDataString(response.PayUrl)}&size=300x300";
                ViewBag.QrCodeUrl = $"https://api.qrserver.com/v1/create-qr-code/?data={Uri.EscapeDataString(response.QrCodeUrl)}&size=300x300";
                ViewBag.MoMoDeepLink = response.QrCodeUrl;
                ViewBag.PayUrl = response.PayUrl;
                
            }
            // Cập nhật thông tin thanh toán
            
            ViewBag.VietQrCode = _vietQRPaymentService.PaymentQRCode(
                    (int)donHang.TotalAmount,
                    "Thanh toán đơn hàng " + donHang.Id
                );

            return View(donHang);
        }

        

        //[Authorize(Roles = "admin,nhanvien")]
        //public ActionResult Index()
        //{
        //    var donHangs = db.DonHangs.Include(d => d.KhachHang).OrderByDescending(x => x.Id);
        //    return View(donHangs.ToList());
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new BadRequestResult();
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(donHang);
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //public ActionResult Create()
        //{
        //    ViewBag.IdKhachHang = new SelectList(db.KhachHang, "Id", "HoTen");
        //    ViewBag.IdNhanVienDuyet = new SelectList(db.NhanVien, "Id", "HoTen");
        //    ViewBag.IdNhanVienGiao = new SelectList(db.NhanVien, "Id", "HoTen");
        //    return View();
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind("Id,NgayDatHang,NgayGiaoHang,IdKhachHang,IdNhanVienDuyet,IdNhanVienGiao,TinhTrang")] DonHang donHang)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DonHangs.Add(donHang);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MaKH = new SelectList(db.KhachHang, "Id", "HoTen", donHang.IdKhachHang);
        //    ViewBag.MaNVDuyet = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienDuyet);
        //    ViewBag.MaNVGH = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienGiao);
        //    return View(donHang);
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.IdKhachHang = new SelectList(db.KhachHang, "Id", "HoTen", donHang.IdKhachHang);
        //    ViewBag.IdNhanVienDuyet = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienDuyet);
        //    ViewBag.IdNhanVienGiao = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienGiao);
        //    return View(donHang);
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind("Id,NgayDatHang,NgayGiaoHang,IdKhachHang,IdNhanVienDuyet,IdNhanVienGiao,TinhTrang")] DonHang donHang)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(donHang).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MaKH = new SelectList(db.KhachHang, "Id", "HoTen", donHang.IdKhachHang);
        //    ViewBag.IdNhanVienDuyet = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienDuyet);
        //    ViewBag.IdNhanVienGiao = new SelectList(db.NhanVien, "Id", "HoTen", donHang.IdNhanVienGiao);
        //    return View(donHang);
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(donHang);
        //}

        //[Authorize(Roles = "admin,nhanvien")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DonHang donHang = db.DonHangs.Find(id);
        //    if (donHang == null)
        //    {
        //        TempData["ErrorMessage"] = "Không tìm thấy đơn hàng để xóa!";
        //        return RedirectToAction("Index");
        //    }
        //    db.DonHangs.Remove(donHang);
        //    db.SaveChanges();
        //    TempData["SuccessMessage"] = "Xóa đơn hàng thành công!";
        //    return RedirectToAction("Index");
        //}

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
