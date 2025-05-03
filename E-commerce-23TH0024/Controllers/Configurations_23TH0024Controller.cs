//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using E_commerce_23TH0024.Models;

//namespace E_commerce_23TH0024.Controllers
//{
//    public class Configurations_23TH0024Controller : Controller
//    {
//        private BanHang_23TH0024Entities db = new BanHang_23TH0024Entities();

//        // GET: Configurations_23TH0024
//        public ActionResult Index()
//        {
//            return View(db.Configurations.ToList());
//        }

//        // GET: Configurations_23TH0024/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Configuration configuration = db.Configurations.Find(id);
//            if (configuration == null)
//            {
//                return HttpNotFound();
//            }
//            return View(configuration);
//        }

//        // GET: Configurations_23TH0024/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Configurations_23TH0024/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ConfigId,ConfigCode,ConfigName,ConfigValue")] Configuration configuration)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Configurations.Add(configuration);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(configuration);
//        }

//        // GET: Configurations_23TH0024/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Configuration configuration = db.Configurations.Find(id);
//            if (configuration == null)
//            {
//                return HttpNotFound();
//            }
//            return View(configuration);
//        }

//        // POST: Configurations_23TH0024/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ConfigId,ConfigCode,ConfigName,ConfigValue")] Configuration configuration)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(configuration).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(configuration);
//        }

//        // GET: Configurations_23TH0024/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Configuration configuration = db.Configurations.Find(id);
//            if (configuration == null)
//            {
//                return HttpNotFound();
//            }
//            return View(configuration);
//        }

//        // POST: Configurations_23TH0024/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Configuration configuration = db.Configurations.Find(id);
//            db.Configurations.Remove(configuration);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
