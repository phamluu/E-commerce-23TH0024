using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoard_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public DashBoard_23TH0024Controller(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _db = context;
            _signInManager = signInManager;
        }
        public ActionResult Index()
        {
            //ViewBag.DonHang = ThongKeDonHang();
            //ViewBag.SanPham = ThongKeSanPham();
            //ViewBag.KhachHang = ThongKeKhachHang();
            return View();
        }

        private List<ThongKeDonHang> ThongKeDonHang()
        {
            var thongKe = _db.DonHangs
                    .Where(x => x.NgayDatHang.HasValue)
                    .GroupBy(x => new { Thang = x.NgayDatHang.Value.Month, Nam = x.NgayDatHang.Value.Year })
                    .Select(g => new ThongKeDonHang
                    {
                        Thang = g.Key.Thang,
                        Nam = g.Key.Nam,
                        TongTien = g.Sum(x => x.TotalProductAmount)
                    })
                    .OrderBy(x => x.Nam)
                    .ThenBy(x => x.Thang)
                    .ToList();
            return thongKe;
        }
        private int ThongKeSanPham()
        {
            var sanphams = _db.SanPham.Count();
            return sanphams;
        }
        private int ThongKeKhachHang()
        {
            var khachhangs = _db.KhachHang.Count();
            return khachhangs;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();  // Đăng xuất người dùng

            if (returnUrl != null)
            {
                return Redirect(returnUrl);  // Chuyển hướng nếu có URL
            }

            return RedirectToAction("Index", "Home", new { area = "" });  // Hoặc quay về trang chủ nếu không có returnUrl
        }
    }

}