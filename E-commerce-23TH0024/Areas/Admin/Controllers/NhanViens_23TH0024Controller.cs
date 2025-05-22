using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.Users;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Models.Identity;
using E_commerce_23TH0024.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using E_commerce_23TH0024.Lib.Enums;
namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class NhanViens_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public NhanViens_23TH0024Controller(
                                ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager,
                                UserService service
                ) 
        {
            db = context;
            _service = new UserService(context);
            _userManager = userManager;
        }
        // GET: NhanViens_23TH0024
        public ActionResult Index()
        {
            var nhanviens = _service.GetNhanViens();
            return View(nhanviens);
        }


        // GET: NhanViens_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.IdAspNetUsers = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: NhanViens_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(
                [Bind("Id,HoTen,SoDienThoai,DiaChi,IdAspNetUsers")] NhanVien nhanVien, string Email, string Password)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdAspNetUsers = new SelectList(db.Users, "Id", "UserName", nhanVien.IdAspNetUsers);
                return View(nhanVien);
            }

            var checkResult = _service.CheckNhanVien(Email);

            switch (checkResult.Item1)
            {
                case CheckNhanVienStatus.LaNhanVien:
                    ModelState.AddModelError("Email", "Email đã tồn tại và thuộc về nhân viên.");
                    TempData["ErrorMessage"] = $"{Email} đã là nhân viên.";
                    break;

                case CheckNhanVienStatus.ChuaCoTaiKhoan:
                    var newUser = new ApplicationUser
                    {
                        UserName = Email,
                        Email = Email,
                        PhoneNumber = nhanVien.SoDienThoai
                    };

                    var createResult = await _userManager.CreateAsync(newUser, Password);
                    if (createResult.Succeeded)
                    {
                        nhanVien.IdAspNetUsers = newUser.Id;
                        db.NhanVien.Add(nhanVien);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Thêm nhân viên thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                            ModelState.AddModelError("", error.Description);

                        TempData["ErrorMessage"] = "Không thể tạo tài khoản người dùng.";
                    }
                    break;

                case CheckNhanVienStatus.CoTaiKhoanChuaPhaiNhanVien:
                    var existingUser = await _userManager.FindByEmailAsync(Email);
                    if (existingUser != null)
                    {
                        nhanVien.IdAspNetUsers = existingUser.Id;
                        db.NhanVien.Add(nhanVien);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Thêm nhân viên thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy tài khoản người dùng.";
                    }
                    break;

                default:
                    TempData["ErrorMessage"] = "Trạng thái không xác định.";
                    break;
            }

            ViewBag.IdAspNetUsers = new SelectList(db.Users, "Id", "UserName", nhanVien.IdAspNetUsers);
            return View(nhanVien);
        }


        // GET: NhanViens_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = _service.GetNhanVien(id.Value);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,HoTen,SoDienThoai,DiaChi")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                bool result = _service.UpdateNhanVien(nhanVien);
                if (result)
                {
                    TempData["SuccessMessage"] = "Cập nhật nhân viên thành công.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật nhân viên không thành công.";
                }
                
            }
            return View(nhanVien);
        }

        // GET: NhanViens_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            NhanVien nhanVien = db.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanVien nhanVien = db.NhanVien.Find(id);
            db.NhanVien.Remove(nhanVien);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa nhân viên thành công.";
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
