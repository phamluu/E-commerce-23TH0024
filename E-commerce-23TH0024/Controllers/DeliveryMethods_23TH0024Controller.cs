using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using E_commerce_23TH0024.Models;
using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using E_commerce_23TH0024.Areas.Admin.Controllers;

namespace E_commerce_23TH0024.Controllers
{
    public class DeliveryMethods_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private FileTransfer_23TH0024Controller _fileTransfer = new FileTransfer_23TH0024Controller();
        public DeliveryMethods_23TH0024Controller(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ActionResult ImportExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ImportExcelAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a file to upload.";
                return View();
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                DataTable dt = _fileTransfer.ReadExcelFile(filePath);
                SaveDataToDatabase(dt);
                ViewBag.Message = "File imported successfully!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View();
            }
        }
        private void SaveDataToDatabase(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                var district = new District{
                    CityID = int.Parse(row["CityID"].ToString()),
                    DistrictName = row["DistrictName"].ToString(),
                };
                db.Districts.Add(district);
            }
            db.SaveChanges();
        }
        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Index()
        {
            return View(db.DeliveryMethods.ToList());
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethods.Find(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }
            return View(deliveryMethod);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ShippingMethodID,MethodName,Description,Cost,DeliveryTime,ActiveStatus")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryMethods.Add(deliveryMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryMethod);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult (); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethods.Find(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }
            return View(deliveryMethod);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ShippingMethodID,MethodName,Description,Cost,DeliveryTime,ActiveStatus")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryMethod);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryMethod deliveryMethod = db.DeliveryMethods.Find(id);
            if (deliveryMethod == null)
            {
                return NotFound();
            }
            return View(deliveryMethod);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryMethod deliveryMethod = db.DeliveryMethods.Find(id);
            db.DeliveryMethods.Remove(deliveryMethod);
            db.SaveChanges();
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
