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
    public class Menus_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        public ActionResult MenuListForGroup(string menuGroup, string viewName)
        {
            var menus = db.Menus.Include(m => m.LoaiSanPham).Include(m => m.NhomMenu1).Where(m => m.NhomMenu == menuGroup)
                .Select(x => new MenuViewModels
                {
                    LoaiMenu = x.LoaiMenu,
                    NhomMenu = x.NhomMenu,
                    NhomMenu1 = x.NhomMenu1,
                    LoaiSanPham = x.LoaiSanPham,
                });
            return View(viewName, menus.ToList());
        }
        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Index()
        {
            var menus = db.Menus.Include(m => m.LoaiSanPham).Include(m => m.NhomMenu1);
            return View(menus.ToList());
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Create()
        {
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP");
            ViewBag.NhomMenu = new SelectList(db.NhomMenus, "MaNhomMenu", "TenNhomMenu");
            var loaiMenuList = Enum.GetValues(typeof(EnumLoaiMenu))
                            .Cast<EnumLoaiMenu>()
                            .Select(e => new
                            {
                                Value = (int)e,
                                Text = e.ToString()
                            })
                            .ToList();

            // Tạo SelectList và truyền vào ViewBag
            ViewBag.LoaiMenu = new SelectList(loaiMenuList, "Value", "Text");
            return View();
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("MaMenu,NhomMenu,LoaiMenu,MaLoaiMenu")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", menu.MaLoaiMenu);
            ViewBag.NhomMenu = new SelectList(db.NhomMenus, "MaNhomMenu", "TenNhomMenu", menu.NhomMenu);
            var loaiMenuList = Enum.GetValues(typeof(EnumLoaiMenu))
                            .Cast<EnumLoaiMenu>()
                            .Select(e => new
                            {
                                Value = (int)e,
                                Text = e.ToString() 
                            })
                            .ToList();

            // Tạo SelectList và truyền vào ViewBag
            ViewBag.LoaiMenu = new SelectList(loaiMenuList, "Value", "Text", menu.LoaiMenu);

            return View(menu);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", menu.MaLoaiMenu);
            ViewBag.NhomMenu = new SelectList(db.NhomMenus, "MaNhomMenu", "TenNhomMenu", menu.NhomMenu);
            var loaiMenuList = Enum.GetValues(typeof(EnumLoaiMenu))
                            .Cast<EnumLoaiMenu>()
                            .Select(e => new
                            {
                                Value = ((int)e),
                                Text = e.ToString()
                            })
                            .ToList();
            ViewBag.LoaiMenu = new SelectList(loaiMenuList, "Value", "Text", menu.LoaiMenu);
            return View(menu);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("MaMenu,NhomMenu,LoaiMenu,MaLoaiMenu")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "MaLSP", "TenLSP", menu.MaLoaiMenu);
            ViewBag.NhomMenu = new SelectList(db.NhomMenus, "MaNhomMenu", "TenNhomMenu", menu.NhomMenu);
            return View(menu);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy menu để xóa!";
                return RedirectToAction("Index");
            }
            db.Menus.Remove(menu);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa menu thành công!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin,nhanvien")]
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
