using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using System.Data.Entity;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        assignmentEntities obje = new assignmentEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var categorys = obje.categorymasters.OrderBy(c => c.categoryId)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize).ToList();
            ViewBag.TotalPages = Math.Ceiling((double)obje.categorymasters.Count() / pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(categorys);

        }
        [HttpGet]
        public ActionResult create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult create([Bind(Include = "categoryId,categoryname")] categorymaster category)
        {
            if (ModelState.IsValid)
            {
                obje.categorymasters.Add(category);
                obje.SaveChanges();
                ViewBag.message = "Dtaa Inserted sucessfully";
                return RedirectToAction("Index");
            }
           
            return View(category);
        }
        [HttpGet]
        public ActionResult edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            categorymaster category = obje.categorymasters.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        public ActionResult edit([Bind(Include = "categoryId,categoryname")] categorymaster category)
        {
            if (ModelState.IsValid)
            {
                obje.Entry(category).State = EntityState.Modified;
                obje.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    
        public ActionResult Detail(int id)
        {
        if (id == null)
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }
        categorymaster category = obje.categorymasters.Find(id);
        if (category == null)
        {
            return HttpNotFound();
        }
        return View(category);
    }
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            categorymaster category = obje.categorymasters.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            categorymaster category = obje.categorymasters.Find(id);
            obje.categorymasters.Remove(category);
            obje.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                obje.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
