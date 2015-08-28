using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;

namespace FoodExNepal.Areas.Admin.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class DeliveryStaffController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/DeliveryStaff/

        public ActionResult Index()
        {
            return View(db.DeliveryStaffs.ToList());
        }

        //
        // GET: /Admin/DeliveryStaff/Details/5

        public ActionResult Details(int id = 0)
        {
            DeliveryStaff deliverystaff = db.DeliveryStaffs.Find(id);
            if (deliverystaff == null)
            {
                return HttpNotFound();
            }
            return View(deliverystaff);
        }

        //
        // GET: /Admin/DeliveryStaff/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/DeliveryStaff/Create

        [HttpPost]
        public ActionResult Create(DeliveryStaff deliverystaff)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryStaffs.Add(deliverystaff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliverystaff);
        }

        //
        // GET: /Admin/DeliveryStaff/Edit/5

        public ActionResult Edit(int id = 0)
        {
            DeliveryStaff deliverystaff = db.DeliveryStaffs.Find(id);
            if (deliverystaff == null)
            {
                return HttpNotFound();
            }
            return View(deliverystaff);
        }

        //
        // POST: /Admin/DeliveryStaff/Edit/5

        [HttpPost]
        public ActionResult Edit(DeliveryStaff deliverystaff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliverystaff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliverystaff);
        }

        //
        // GET: /Admin/DeliveryStaff/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DeliveryStaff deliverystaff = db.DeliveryStaffs.Find(id);
            if (deliverystaff == null)
            {
                return HttpNotFound();
            }
            return View(deliverystaff);
        }

        //
        // POST: /Admin/DeliveryStaff/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryStaff deliverystaff = db.DeliveryStaffs.Find(id);
            db.DeliveryStaffs.Remove(deliverystaff);
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