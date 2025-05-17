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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using E_commerce_23TH0024.Models.Location;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    public class Districts_23TH0024Controller : BaseController
    {
        private readonly FileTransfer_23TH0024Controller _fileTransfer;
        public Districts_23TH0024Controller(ApplicationDbContext context) : base(context)
        {
            _fileTransfer = new FileTransfer_23TH0024Controller(db);
        }
        //private readonly ApplicationDbContext db;
        //public Districts_23TH0024Controller(ApplicationDbContext context)
        //{
        //    db = context;
        //}

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
                var distric = new District
                {
                    DistrictName = row["DistricName"].ToString(),
                    Id = int.Parse(row["DistrictID"].ToString()),
                };
                db.Districts.Add(distric);
            }
            db.SaveChanges();
        }

        #region ExportExcel
        public ActionResult ExportExcel()
        {
            var districs = db.Districts.ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quận/ huyện");
                worksheet.Cells[1, 1].Value = "DistricID";
                worksheet.Cells[1, 2].Value = "DistricName";
                int row = 2;
                foreach (var distric in districs)
                {
                    worksheet.Cells[row, 1].Value = distric.Id;
                    worksheet.Cells[row, 2].Value = distric.DistrictName;
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
        // GET: Districts_23TH0024
        public ActionResult Index()
        {
            var districts = db.Districts.Include(d => d.City);
            return View(districts.ToList());
        }

        // GET: Districts_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        // GET: Districts_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.Cities, "Id", "CityName");
            return View();
        }

        // POST: Districts_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("DistrictID,DistrictName,CityID")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Districts.Add(district);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName", district.IdCity);
            return View(district);
        }

        // GET: Districts_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", district.IdCity);
            return View(district);
        }

        // POST: Districts_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,DistrictName,IdCity")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityID = new SelectList(db.Cities, "IdCity", "CityName", district.IdCity);
            return View(district);
        }

        // GET: Districts_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        // POST: Districts_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
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
