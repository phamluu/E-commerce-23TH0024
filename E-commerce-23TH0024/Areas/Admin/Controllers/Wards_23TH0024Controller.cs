using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using OfficeOpenXml;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class Wards_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private FileTransfer_23TH0024Controller _fileTransfer = new FileTransfer_23TH0024Controller();

        #region Import tất cả tỉnh/ thành, quận/ huyện, phường/ xã
        public ActionResult ImportAll()
        {
            return View("ImportExcel");
        }
        [HttpPost]
        public async Task<ActionResult> ImportAllAsync(IFormFile file)
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
                SaveAllToDatabase(dt);
                ViewBag.Message = "File imported successfully!";
                return View("ImportExcel");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return View();
            }
        }
        private void SaveAllToDatabase(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                string CityName = row["CityName"].ToString();
                string DistricName = row["DistricName"].ToString();
                string WardName = row["WardName"].ToString();
                var city = db.Cities.FirstOrDefault(c => c.CityName == CityName);
                if (city == null)
                {
                    city = new City { CityName = CityName };
                    db.Cities.Add(city);
                    db.SaveChanges();
                }

                var district = db.Districts.FirstOrDefault(d => d.DistrictName == DistricName && d.CityID == city.Id);
                if (district == null)
                {
                    district = new District { DistrictName = DistricName, CityID = city.Id };
                    db.Districts.Add(district);
                    db.SaveChanges();
                }
                var ward = db.Wards.FirstOrDefault(w => w.WardName == WardName && w.Id == district.Id);
                if (ward == null)
                {
                    ward = new Ward { WardName = WardName, DistrictID = district.Id };
                    db.Wards.Add(ward);
                }
            }
            db.SaveChanges();
        }
        #endregion

        #region Import
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
                var ward = new Ward
                {
                    WardName = row["WardName"].ToString(),
                    Id = int.Parse(row["WardID"].ToString()),
                };
                db.Wards.Add(ward);
            }
            db.SaveChanges();
        }
        #endregion

        #region ExportExcel
        public ActionResult ExportExcel()
        {
            var wards = db.Wards.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quận/ huyện");
                worksheet.Cells[1, 1].Value = "WardID";
                worksheet.Cells[1, 2].Value = "WardName";
                int row = 2;
                foreach (var ward in wards)
                {
                    worksheet.Cells[row, 1].Value = ward.Id;
                    worksheet.Cells[row, 2].Value = ward.WardName;
                    row++;
                }
                var memoryStream = new MemoryStream();
                package.SaveAs(memoryStream);
                memoryStream.Position = 0;
                string fileName = "Products_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Index()
        {
            var wards = db.Wards.Include(w => w.District);
            return View(wards.ToList());
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return NotFound();
            }
            return View(ward);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Create()
        {
            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName");
            return View();
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("WardID,WardName,DistrictID")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Wards.Add(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", ward.DistrictID);
            return View(ward);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return NotFound();
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", ward.DistrictID);
            return View(ward);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("WardID,WardName,DistrictID")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", ward.DistrictID);
            return View(ward);
        }

        [Authorize(Roles = "admin,nhanvien")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ward ward = db.Wards.Find(id);
            if (ward == null)
            {
                return NotFound();
            }
            return View(ward);
        }

        [Authorize(Roles = "admin,nhanvien")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ward ward = db.Wards.Find(id);
            db.Wards.Remove(ward);
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
