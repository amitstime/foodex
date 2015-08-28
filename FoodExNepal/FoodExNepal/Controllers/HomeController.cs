using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;

namespace FoodExNepal.Controllers
{
    public class HomeController : Controller
    {
        FoodExEntities db=new FoodExEntities();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewData["Location"] = new SelectList(db.Locations.OrderBy(a => a.LocationName), "LocationId", "LocationName");
            return View();
        }
        public ActionResult LoadPopup()
        {
            WebsiteProfile websiteprofile = db.WebsiteProfiles.First();
            ViewBag.popup = websiteprofile.HomePageImage;
            return PartialView();
        }
        public ActionResult LoadBanner()
        {
            var banner=db.Banners.Where(a=>a.isActive==true);
            ViewData["Location"] = new SelectList(db.Locations.OrderBy(a => a.LocationName), "LocationId", "LocationName");
            return PartialView(banner);

        }
        public ActionResult LoadWelcome()
        {
            WebsiteProfile websiteprofile = db.WebsiteProfiles.First();
            return PartialView(websiteprofile);
        }

        public ActionResult LoadWhyFoodex()
        {
            return PartialView();
        }
        public ActionResult LoadRecent()
        {
           var restaurant = db.Restaurants.Where(a=>a.IsActive==true).OrderByDescending(a => a.RestaurantId).ToList();
           return PartialView(restaurant);
        }
        public ActionResult LoadHotDeal()
        {
            var hotDeal = db.FoodItems.Where(a => a.IsHotDeal == true).OrderByDescending(a => a.FoodItemName).ToList();
            return PartialView(hotDeal);
        }

        public ActionResult WelcomeDetails()
        {
            var welcome = db.WebsiteProfiles.First();
          ViewBag.welcome= welcome.WelcometoFoodexNepal;
            return View();
        }

        public ActionResult WhyFoodexDetails()
        {
            var whyFoodex = db.WebsiteProfiles.First();
            ViewBag.WhyFoodex = whyFoodex.WhyFoodExNepal;
            return View();
        }
        [HttpGet]
        public IEnumerable<FoodItem> GetProducts(string query = "")
        {
            
                return String.IsNullOrEmpty(query) ? db.FoodItems.ToList() :
                db.FoodItems.Where(p => p.FoodItemName.Contains(query)).ToList();
            
        }

        public ActionResult FindRestaurant()
        {
            ViewData["Location"] = new SelectList(db.Locations.OrderBy(a => a.LocationName), "LocationId", "LocationName");
            
            return PartialView();
        }
    }
}
