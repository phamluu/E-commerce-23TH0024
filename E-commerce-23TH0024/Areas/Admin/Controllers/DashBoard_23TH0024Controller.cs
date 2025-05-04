using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoard_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext _db;
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
            var khachhangs = _db.KhachHangs.Count();
            return khachhangs;
        }
    }

}