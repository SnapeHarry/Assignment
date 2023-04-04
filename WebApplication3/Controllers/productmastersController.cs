using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class productmastersController : Controller
    {
        private assignmentEntities db = new assignmentEntities();

        // GET: productmasters
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var products = db.productmasters.Include(p => p.categorymaster).OrderBy(p => p.productid)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize).ToList();
            ViewBag.TotalPages = Math.Ceiling((double)db.productmasters.Count() / pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productmaster productmaster = db.productmasters.Find(id);
            if (productmaster == null)
            {
                return HttpNotFound();
            }
            return View(productmaster);
        }
        public ActionResult Create()
        {
            ViewBag.productid = new SelectList(db.categorymasters, "categoryId", "categoryname");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productid,productname")] productmaster productmaster)
        {
            if (ModelState.IsValid)
            {
                db.productmasters.Add(productmaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productid = new SelectList(db.categorymasters, "categoryId", "categoryname", productmaster.productid);
            return View(productmaster);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productmaster productmaster = db.productmasters.Find(id);
            if (productmaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.productid = new SelectList(db.categorymasters, "categoryId", "categoryname", productmaster.productid);
            return View(productmaster);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productid,productname")] productmaster productmaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productmaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productid = new SelectList(db.categorymasters, "categoryId", "categoryname", productmaster.productid);
            return View(productmaster);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productmaster productmaster = db.productmasters.Find(id);
            if (productmaster == null)
            {
                return HttpNotFound();
            }
            return View(productmaster);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productmaster productmaster = db.productmasters.Find(id);
            db.productmasters.Remove(productmaster);
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
