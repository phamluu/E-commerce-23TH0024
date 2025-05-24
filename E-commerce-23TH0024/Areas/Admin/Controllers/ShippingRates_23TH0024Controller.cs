using E_commerce_23TH0024.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.Ecommerce;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingRates_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        public ShippingRates_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: ShippingRates_23TH0024
        public ActionResult Index()
        {
            var shippingRates = db.ShippingRates.Include(s => s.DeliveryMethod);
            return View(shippingRates.ToList());
        }

        // GET: ShippingRates_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingRate shippingRate = db.ShippingRates.Find(id);
            if (shippingRate == null)
            {
                return NotFound();
            }
            return View(shippingRate);
        }

        // GET: ShippingRates_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.IdDeliveryMethod = new SelectList(db.DeliveryMethods, "Id", "MethodName");
            ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,IdDeliveryMethod,FromDistance,ToDistance,FixedPrice,PricePerKm,IdCity,WeightLitmit,CreateAt")] ShippingRate shippingRate)
        {
            if (ModelState.IsValid)
            {
                db.ShippingRates.Add(shippingRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName", shippingRate.IdCity);
            ViewBag.IdDeliveryMethod = new SelectList(db.DeliveryMethods, "Id", "MethodName", shippingRate.IdDeliveryMethod);
            return View(shippingRate);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingRate shippingRate = db.ShippingRates.Find(id);
            if (shippingRate == null)
            {
                return NotFound();
            }
            ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName", shippingRate.IdCity);
            ViewBag.IdDeliveryMethod = new SelectList(db.DeliveryMethods, "Id", "MethodName", shippingRate.IdDeliveryMethod);
            return View(shippingRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,IdDeliveryMethod,FromDistance,ToDistance,FixedPrice,PricePerKm,IdCity,WeightLitmit,CreateAt")] ShippingRate shippingRate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingRate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCity = new SelectList(db.Cities, "Id", "CityName", shippingRate.IdCity);
            ViewBag.IdDeliveryMethod = new SelectList(db.DeliveryMethods, "Id", "MethodName", shippingRate.IdDeliveryMethod);
            return View(shippingRate);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingRate shippingRate = db.ShippingRates.Find(id);
            if (shippingRate == null)
            {
                return NotFound();
            }
            return View(shippingRate);
        }

        // POST: ShippingRates_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShippingRate shippingRate = db.ShippingRates.Find(id);
            db.ShippingRates.Remove(shippingRate);
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
