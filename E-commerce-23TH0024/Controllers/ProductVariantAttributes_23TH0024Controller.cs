using System;
using System.Collections.Generic;
using System.Data;
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

namespace E_commerce_23TH0024.Controllers
{
    [Authorize(Roles = "admin,nhanvien")]
    public class ProductVariantAttributes_23TH0024Controller : Controller
    {
        private readonly ApplicationDbContext db;

        // GET: ProductVariantAttributes_23TH0024
        public ActionResult Index()
        {
            var productVariantAttributes = db.ProductVariantAttributes.Include(p => p.AttributeValue).Include(p => p.ProductVariant).Include(p => p.ProductAttribute);
            return View(productVariantAttributes.ToList());
        }

        // GET: ProductVariantAttributes_23TH0024/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductVariantAttribute productVariantAttribute = db.ProductVariantAttributes.Find(id);
            if (productVariantAttribute == null)
            {
                return NotFound();
            }
            return View(productVariantAttribute);
        }

        // GET: ProductVariantAttributes_23TH0024/Create
        public ActionResult Create()
        {
            ViewBag.AttributeValueID = new SelectList(db.AttributeValues, "ValueID", "Value");
            ViewBag.VariantID = new SelectList(db.ProductVariants, "VariantID", "VariantID");
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName");
            return View();
        }

        // POST: ProductVariantAttributes_23TH0024/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("ID,AttributeID,VariantID,AttributeValueID")] ProductVariantAttribute productVariantAttribute)
        {
            if (ModelState.IsValid)
            {
                db.ProductVariantAttributes.Add(productVariantAttribute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AttributeValueID = new SelectList(db.AttributeValues, "ValueID", "Value", productVariantAttribute.IdAttributeValue);
            ViewBag.VariantID = new SelectList(db.ProductVariants, "VariantID", "VariantID", productVariantAttribute.IdProductVariant);
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", productVariantAttribute.IdProductAttribute);
            return View(productVariantAttribute);
        }

        // GET: ProductVariantAttributes_23TH0024/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult(); //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductVariantAttribute productVariantAttribute = db.ProductVariantAttributes.Find(id);
            if (productVariantAttribute == null)
            {
                return NotFound();
            }
            var valueList = db.AttributeValues.Where(x => x.IdProductAttribute == productVariantAttribute.IdProductAttribute);
            ViewBag.AttributeValueID = new SelectList(valueList, "ValueID", "Value", productVariantAttribute.IdAttributeValue);
            ViewBag.VariantID = new SelectList(db.ProductVariants, "VariantID", "VariantID", productVariantAttribute.IdProductVariant);
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", productVariantAttribute.IdProductAttribute);
            return View(productVariantAttribute);
        }

        // POST: ProductVariantAttributes_23TH0024/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("ID,AttributeID,VariantID,AttributeValueID")] ProductVariantAttribute productVariantAttribute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productVariantAttribute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AttributeValueID = new SelectList(db.AttributeValues, "ValueID", "Value", productVariantAttribute.IdAttributeValue);
            ViewBag.VariantID = new SelectList(db.ProductVariants, "VariantID", "VariantID", productVariantAttribute.IdProductVariant);
            ViewBag.AttributeID = new SelectList(db.ProductAttributes, "AttributeID", "AttributeName", productVariantAttribute.IdProductAttribute);
            return View(productVariantAttribute);
        }

        // GET: ProductVariantAttributes_23TH0024/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            ProductVariantAttribute productVariantAttribute = db.ProductVariantAttributes.Find(id);
            if (productVariantAttribute == null)
            {
                return NotFound();
            }
            return View(productVariantAttribute);
        }

        // POST: ProductVariantAttributes_23TH0024/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductVariantAttribute productVariantAttribute = db.ProductVariantAttributes.Find(id);
            db.ProductVariantAttributes.Remove(productVariantAttribute);
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
