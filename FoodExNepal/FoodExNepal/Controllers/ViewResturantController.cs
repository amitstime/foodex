using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;

namespace FoodExNepal.Controllers
{
    public class ViewResturantController : Controller
    {
        //
        // GET: /ViewResturant/
        FoodExEntities db = new FoodExEntities();
        public ActionResult Index(int resturantId)
        {
            Restaurant r1 = db.Restaurants.Find(resturantId);

            return View(r1);
        }
        public ActionResult FoodMenuList(int resturantId)
        {
            Restaurant r1 = db.Restaurants.Find(resturantId);

            return View(r1);
        }

    }
}
