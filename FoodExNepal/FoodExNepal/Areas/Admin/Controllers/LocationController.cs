using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
using PagedList;

namespace FoodExNepal.Areas.Admin.Controllers
{
    public class LocationController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/Location/        

        public ActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View((db.Locations.ToList()).ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Admin/Location/Details/5

        public ActionResult Details(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Admin/Location/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Location/Create

        [HttpPost]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        //
        // GET: /Admin/Location/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Admin/Location/Edit/5

        [HttpPost]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Admin/Location/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Admin/Location/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}