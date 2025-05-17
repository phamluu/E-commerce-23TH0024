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

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    [Area("Admin")]
    public class NhanViens_23TH0024Controller : BaseController
    {
        private readonly ApplicationDbContext db;
        public NhanViens_23TH0024Controller(ApplicationDbContext context) : base(context)
        {
        }
        // GET: NhanViens_23TH0024
        public ActionResult Index()
        {
            var nhanviens = db.NhanViens.ToList();
            var userIds = nhanviens
                .Where(nv => nv.IdAspNetUsers != null)
                .Select(nv => nv.IdAspNetUsers)
                .Distinct()
                .ToList();

            var users = db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u);

            var viewModel = nhanviens.Select(nv => new NhanVienViewModel
            {
                NhanVien = nv,
                AspNetUser = nv.IdAspNetUsers != null && users.ContainsKey(nv.IdAspNetUsers)
                    ? users[nv.IdAspNetUsers]
                    : null,
                UserName = nv.IdAspNetUsers != null && users.ContainsKey(nv.IdAspNetUsers)
                    ? users[nv.IdAspNetUsers].UserName
                    : null
            }).ToList();

            return View(viewModel);
        }


        // GET: NhanViens_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: NhanViens_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("MaNV,HoTen,SoDienThoai,DiaChi,UserID")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewBag.IdAspNetUsers = new SelectList(db.Users, "Id", "UserName", nhanVien.IdAspNetUsers);
            return View(nhanVien);
        }

        // POST: NhanViens_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,HoTen,SoDienThoai,DiaChi,IdAspNetUsers")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserName", nhanVien.IdAspNetUsers);
            return View(nhanVien);
        }

        // GET: NhanViens_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
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
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
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
    }
}
