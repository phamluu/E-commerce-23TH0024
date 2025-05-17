using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using E_commerce_23TH0024.Data;
using Microsoft.EntityFrameworkCore;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Controllers
{
    public class CustomerTypes_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        // GET: CustomerTypes_23TH0024
        public ActionResult Index()
        {
            return View(db.CustomerTypes.ToList());
        }

        // GET: CustomerTypes_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            CustomerType customerType = db.CustomerTypes.Find(id);
            if (customerType == null)
            {
                return NotFound();
            }
            return View(customerType);
        }

        // GET: CustomerTypes_23TH0024/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerTypes_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("CustomerTypeID,CustomerTypeName")] CustomerType customerType)
        {
            if (ModelState.IsValid)
            {
                db.CustomerTypes.Add(customerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerType);
        }

        // GET: CustomerTypes_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            CustomerType customerType = db.CustomerTypes.Find(id);
            if (customerType == null)
            {
                return NotFound();
            }
            return View(customerType);
        }

        // POST: CustomerTypes_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("CustomerTypeID,CustomerTypeName")] CustomerType customerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerType);
        }

        // GET: CustomerTypes_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            CustomerType customerType = db.CustomerTypes.Find(id);
            if (customerType == null)
            {
                return NotFound();
            }
            return View(customerType);
        }

        // POST: CustomerTypes_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerType customerType = db.CustomerTypes.Find(id);
            db.CustomerTypes.Remove(customerType);
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
