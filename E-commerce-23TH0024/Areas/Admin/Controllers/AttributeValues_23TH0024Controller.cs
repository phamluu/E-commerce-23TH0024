using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using E_commerce_23TH0024.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_23TH0024.Models.Ecommerce;

namespace E_commerce_23TH0024.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin,nhanvien")]
    public class AttributeValues_23TH0024Controller : Controller
    {
        private ApplicationDbContext db;

        public AttributeValues_23TH0024Controller(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: AttributeValues_23TH0024
        public ActionResult Index()
        {
            var attributeValues = db.AttributeValues.Include(a => a.ProductAttribute);
            return View(attributeValues.ToList());
        }
        public ActionResult ValueListForAttribute(string AttributeID)
        {
            var data = db.AttributeValues.Where(x => x.IdProductAttribute == AttributeID).ToList();
            if (data != null && data.Count() > 0)
            {
                return new JsonResult(data.Select(v => new { Value = v.Id, Text = v.Value }));
            }
            return new JsonResult(new List<object>());
        }
        // GET: AttributeValues_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            AttributeValue attributeValue = db.AttributeValues.Find(id);
            if (attributeValue == null)
            {
                return NotFound();
            }
            return View(attributeValue);
        }

        // GET: AttributeValues_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName");
            return View();
        }

        // POST: AttributeValues_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ValueID,AttributeID,Value")] AttributeValue attributeValue)
        {
            if (ModelState.IsValid)
            {
                db.AttributeValues.Add(attributeValue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", attributeValue.IdProductAttribute);
            return View(attributeValue);
        }

        // GET: AttributeValues_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributeValue attributeValue = db.AttributeValues.Find(id);
            if (attributeValue == null)
            {
                return NotFound();
            }
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", attributeValue.IdProductAttribute);
            return View(attributeValue);
        }

        // POST: AttributeValues_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ValueID,AttributeID,Value")] AttributeValue attributeValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attributeValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", attributeValue.IdProductAttribute);
            return View(attributeValue);
        }

        // GET: AttributeValues_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributeValue attributeValue = db.AttributeValues.Find(id);
            if (attributeValue == null)
            {
                return NotFound();
            }
            return View(attributeValue);
        }

        // POST: AttributeValues_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttributeValue attributeValue = db.AttributeValues.Find(id);
            db.AttributeValues.Remove(attributeValue);
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
