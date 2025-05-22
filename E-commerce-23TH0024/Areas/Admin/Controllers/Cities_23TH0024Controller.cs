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
using OfficeOpenXml;
using Microsoft.AspNetCore.Authorization;
using E_commerce_23TH0024.Models.Location;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Cities_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly FileTransfer_23TH0024Controller _fileTransfer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Cities_23TH0024Controller(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            db = context;
            _httpContextAccessor = httpContextAccessor;
            _fileTransfer = new FileTransfer_23TH0024Controller(db);
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
                var cities = new City
                {
                    CityName = row["CityName"].ToString(),
                };
                db.Cities.Add(cities);
            }
            db.SaveChanges();
        }

        #region ExportExcel
        public ActionResult ExportExcel()
        {
            var cities = db.Cities.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Tỉnh/ thành phố");
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "CityName";
                int row = 2;
                foreach (var city in cities)
                {
                    worksheet.Cells[row, 1].Value = city.Id;
                    worksheet.Cells[row, 2].Value = city.CityName;
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

        
        public ActionResult Index()
        {
            return View(db.Cities.ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,CityName")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(city);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,CityName")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
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
