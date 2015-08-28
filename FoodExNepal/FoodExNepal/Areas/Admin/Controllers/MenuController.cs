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
    public class MenuController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/Menu/

        public ActionResult Index()
        {
            return View(db.Menus.ToList());
        }

        //
        // GET: /Admin/Menu/Details/5

        public ActionResult Details(int id = 0)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // GET: /Admin/Menu/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Menu/Create

        [HttpPost]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Menus.Add(menu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menu);
        }

        //
        // GET: /Admin/Menu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // POST: /Admin/Menu/Edit/5

        [HttpPost]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menu);
        }

        //
        // GET: /Admin/Menu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        //
        // POST: /Admin/Menu/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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