using System.Data;
using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using E_commerce_23TH0024.Models.Location;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Wards_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly FileTransfer_23TH0024Controller _fileTransfer;

        public Wards_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
            _fileTransfer = new FileTransfer_23TH0024Controller(db);
        }
        //private FileTransfer_23TH0024Controller _fileTransfer = new FileTransfer_23TH0024Controller(db);

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

                var district = db.Districts.FirstOrDefault(d => d.DistrictName == DistricName && d.IdCity == city.Id);
                if (district == null)
                {
                    district = new District { DistrictName = DistricName, IdCity = city.Id };
                    db.Districts.Add(district);
                    db.SaveChanges();
                }
                var ward = db.Wards.FirstOrDefault(w => w.WardName == WardName && w.Id == district.Id);
                if (ward == null)
                {
                    ward = new Ward { WardName = WardName, IdDistrict = district.Id };
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
                worksheet.Cells[1, 1].Value = "Id";
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

        
        public ActionResult Index()
        {
            var wards = db.Wards.Include(w => w.District);
            return View(wards.ToList());
        }

        
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

       
        public ActionResult Create()
        {
            ViewBag.IdDistrict = new SelectList(db.Districts, "Id", "DistrictName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,WardName,IdDistrict")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Wards.Add(ward);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdDistrict = new SelectList(db.Districts, "Id", "DistrictName", ward.IdDistrict);
            return View(ward);
        }

       
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
            ViewBag.IdDistrict = new SelectList(db.Districts, "IdDistrict", "DistrictName", ward.IdDistrict);
            return View(ward);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,WardName,IdDistrict")] Ward ward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdDistrict = new SelectList(db.Districts, "Id", "DistrictName", ward.IdDistrict);
            return View(ward);
        }

       
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ward ward = db.Wards.Find(id);
            db.Wards.Remove(ward);
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
