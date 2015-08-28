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
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class FoodCategoryController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/FoodCategory/

        public ActionResult Index(int restaurantID)
        {            
            return View(db.FoodCategories.Where(a => a.RestaurantId == restaurantID).ToList());
        }  

       /*    public ActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View((db.FoodCategories.ToList()).ToPagedList(pageNumber, pageSize));
        }*/

        //
        // GET: /Admin/FoodCategory/Details/5

        public ActionResult Details(int id = 0)
        {
            FoodCategory foodcategory = db.FoodCategories.Find(id);
            if (foodcategory == null)
            {
                return HttpNotFound();
            }
            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/FoodCategory/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                db.FoodCategories.Add(foodcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            FoodCategory foodcategory = db.FoodCategories.Find(id);
            if (foodcategory == null)
            {
                return HttpNotFound();
            }
            return View(foodcategory);
        }

        //
        // POST: /Admin/FoodCategory/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FoodCategory foodcategory = db.FoodCategories.Find(id);
            if (foodcategory == null)
            {
                return HttpNotFound();
            }
            return View(foodcategory);
        }

        //
        // POST: /Admin/FoodCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodCategory foodcategory = db.FoodCategories.Find(id);
            db.FoodCategories.Remove(foodcategory);
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