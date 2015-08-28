using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
using PagedList;
using System.IO;
using System.Web.Security;

namespace FoodExNepal.Controllers
{
    public class RestaurantUserController : Controller
    {
        FoodExEntities db = new FoodExEntities();

        public ActionResult Dashboard()
        {
            return View();
        }
        
        //
        // GET: /RestaurantControl/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Restaurant restaurant, string ReturnUrl = "")
        {
            var resuser = db.Restaurants.Where(au => au.Username == restaurant.Username && au.Password == restaurant.Password).SingleOrDefault();
            if (resuser == null)
            {
                ModelState.AddModelError("", "Login data is incorrect");
                return View(restaurant);
            }
            //FormsAuthentication.SetAuthCookie(user.UserName, false);

          //  Restaurant restaurant=db.Restaurants.Find(resuser.RestaurantId);
            Session["RestaurantUser"] = restaurant;
            //return Redirect(ReturnUrl);
            if (Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);
            else
                if (resuser.RestaurantId == null)
                    return RedirectToAction("Index", "Home");
                return RedirectToAction("Dashboard");
        }
        public ActionResult EditRestaurant()
        {
            Restaurant restaurant=null;
            if (Session["RestaurantUser"]!=null)
                 restaurant=(Restaurant)Session["RestaurantUser"];
            
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", restaurant.LocationId);
            return View(restaurant);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRestaurant(Restaurant restaurant, HttpPostedFileBase fileArticle, HttpPostedFileBase RestaurantImage)
        {
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {
                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    restaurant.RestaurantLogo = fileName;

                }
                if (RestaurantImage != null)
                {
                    string fileName = checkfile(RestaurantImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    RestaurantImage.SaveAs(filePath);
                    restaurant.RestaurantImageInDetailPage = fileName;
                }
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", restaurant.LocationId);
            return View(restaurant);
        }


        public ActionResult ListCategory(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
            return View((db.FoodCategories.Where(c=>c.RestaurantId==restaurantid).ToList()).ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Admin/FoodCategory/Details/5

        public ActionResult DetailsCategory(int id = 0)
        {
            int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
            FoodCategory foodcategory = db.FoodCategories.Where(c => c.RestaurantId == restaurantid && c.FoodCategoryId==id).FirstOrDefault();
            if (foodcategory == null)
            {
                return HttpNotFound();
            }
            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Create

        public ActionResult CreateCategory()
        {
            return View();
        }

        //
        // POST: /Admin/FoodCategory/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateCategory(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
                foodcategory.RestaurantId = restaurantid;
                db.FoodCategories.Add(foodcategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Edit/5

        public ActionResult EditCategory(int id = 0)
        {
            int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
            FoodCategory foodcategory = db.FoodCategories.Where(c => c.RestaurantId == restaurantid && c.FoodCategoryId == id).FirstOrDefault();
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
        public ActionResult EditCategory(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
                foodcategory.RestaurantId = restaurantid;
                
                db.Entry(foodcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foodcategory);
        }

        //
        // GET: /Admin/FoodCategory/Delete/5

        public ActionResult DeleteCategory(int id = 0)
        {
            //FoodCategory foodcategory = db.FoodCategories.Find(id);
            int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
            FoodCategory foodcategory = db.FoodCategories.Where(c => c.RestaurantId == restaurantid && c.FoodCategoryId == id).FirstOrDefault();
            if (foodcategory == null)
            {
                return HttpNotFound();
            }
            return View(foodcategory);
        }

        //
        // POST: /Admin/FoodCategory/Delete/5

        [HttpPost, ActionName("DeleteCategory")]
        public ActionResult DeleteCategoryConfirmed(int id)
        {
            int restaurantid = ((Restaurant)Session["RestaurantUser"]).RestaurantId;
            FoodCategory foodcategory = db.FoodCategories.Where(c => c.RestaurantId == restaurantid && c.FoodCategoryId == id).FirstOrDefault();
            db.FoodCategories.Remove(foodcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private string checkfile(String filename)
        {
            string filecount = (DateTime.Now.ToString()).Replace('/', '-');
            filecount = (filecount).Replace(' ', '-');
            filecount = (filecount).Replace(':', '-');
            return filecount + filename;

        }
      
    }
}
