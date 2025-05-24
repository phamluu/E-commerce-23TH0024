using System.Data;
using Microsoft.AspNetCore.Mvc;
using E_commerce_23TH0024.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using E_commerce_23TH0024.Models.Location;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DeliveryMethods_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileTransfer_23TH0024Controller _fileTransfer;
        public DeliveryMethods_23TH0024Controller(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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
                var district = new District
                {
                    IdCity = int.Parse(row["IdCity"].ToString()),
                    DistrictName = row["DistrictName"].ToString(),
                };
                db.Districts.Add(district);
            }
            db.SaveChanges();
        }
        
        public ActionResult Index()
        {
            return View(db.DeliveryMethods.ToList());
        }

        
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

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,MethodName,Description,Cost,DeliveryTime,ActiveStatus")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryMethods.Add(deliveryMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryMethod);
        }

        
        public ActionResult Edit(int? id)
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,MethodName,Description,Cost,DeliveryTime,ActiveStatus")] DeliveryMethod deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryMethod);
        }

        
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryMethod deliveryMethod = db.DeliveryMethods.Find(id);
            db.DeliveryMethods.Remove(deliveryMethod);
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
