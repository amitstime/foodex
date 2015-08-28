using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FoodExNepal.Models;

namespace FoodExNepal.Controllers
{
    public class RAdminController : Controller
    {
        FoodExEntities db = new FoodExEntities();
        //
        // GET: /RAdmin/
        
        public ActionResult Index(string ReturnUrl="")
        {
            if(ReturnUrl!=string.Empty)
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // Post: /Web_Admin/
        [HttpPost]
        public ActionResult Index(Restaurant restaurant, string ReturnUrl = "")
        {            
                var user = db.Restaurants.Where(au => au.Email == restaurant.Email && au.Password == restaurant.Password).SingleOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Login data is incorrect");
                    return View(user);
                }
                FormsAuthentication.SetAuthCookie(user.Email, false);
                Session["RestaurantUser"] = user;
                var sesId = user.RestaurantId;
                //Session["RestaurantUserId"] = user.Email;
                //return Redirect(ReturnUrl);
                if (Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction( "Details", "admin/Restaurant", new { id=((Restaurant)Session["RestaurantUser"]).RestaurantId });            
        }

        public ActionResult Logout()
        {
            //string user = ((Restaurant)Session["RestaurantUser"]).Email;
            
            FormsAuthentication.SignOut();
            Session["RestaurantUser"] = null;
            Session.Abandon();
            Session.Clear();
            
            return RedirectToAction("Index","RAdmin");
        }

        

        

    }
}