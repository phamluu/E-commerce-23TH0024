using System.Data;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.SystemSetting;
using E_commerce_23TH0024.Service;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Menus_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly MenuService _service;
        public Menus_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
            _service = new MenuService(db);
        }
        public ActionResult MenuListForGroup(string menuGroup, string viewName)
        {
            var menus = db.Menu.Include(m => m.NhomMenu).Where(m => m.IdNhomMenu == menuGroup)
                .Select(x => new MenuViewModels
                {
                    LoaiMenu = x.LoaiMenu,
                    NhomMenu = x.NhomMenu,
                    //NhomMenu1 = x.NhomMenu1,
                    //LoaiSanPham = x.LoaiSanPham,
                });
            return View(viewName, menus.ToList());
        }
        
        public ActionResult Index()
        {
            var menus = _service.GetMenus();
            return View(menus.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

       
        public ActionResult Create()
        {
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "Id", "TenLSP");
            ViewBag.IdNhomMenu = new SelectList(db.NhomMenu, "Id", "TenNhomMenu");
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,IdNhomMenu,LoaiMenu,MaLoaiMenu")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menu.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "Id", "TenLSP", menu.MaLoaiMenu);
            ViewBag.IdNhomMenu = new SelectList(db.NhomMenu, "Id", "TenNhomMenu", menu.IdNhomMenu);
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

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "Id", "TenLSP", menu.MaLoaiMenu);
            ViewBag.NhomMenu = new SelectList(db.NhomMenu, "Id", "TenNhomMenu", menu.NhomMenu);
            var loaiMenuList = Enum.GetValues(typeof(EnumLoaiMenu))
                            .Cast<EnumLoaiMenu>()
                            .Select(e => new
                            {
                                Value = (int)e,
                                Text = e.ToString()
                            })
                            .ToList();
            ViewBag.LoaiMenu = new SelectList(loaiMenuList, "Value", "Text", menu.LoaiMenu);
            return View(menu);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdNhomMenu,LoaiMenu,MaLoaiMenu")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiMenu = new SelectList(db.LoaiSanPham, "Id", "TenLSP", menu.MaLoaiMenu);
            ViewBag.NhomMenu = new SelectList(db.NhomMenu, "MaNhomMenu", "TenNhomMenu", menu.NhomMenu);
            return View(menu);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menu.Find(id);
            if (menu == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy menu để xóa!";
                return RedirectToAction("Index");
            }
            db.Menu.Remove(menu);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa menu thành công!";
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
