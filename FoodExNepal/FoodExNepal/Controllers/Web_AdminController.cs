using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FoodExNepal.Models;
//using WebMatrix.WebData;
namespace FoodExNepal.Areas
{
    public class Web_AdminController : Controller
    {

        FoodExEntities db = new FoodExEntities();
        //
        // GET: /Web_Admin/
        
        public ActionResult Index(string ReturnUrl="")
        {
            if(ReturnUrl!=string.Empty)
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        // Post: /Web_Admin/
        [HttpPost]
        public ActionResult Index(AdminUser adminuser, string ReturnUrl = "")
        {            
                var user = db.AdminUsers.Where(au => au.AdminUsername == adminuser.AdminUsername && au.AdminPassword == adminuser.AdminPassword).SingleOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "Login data is incorrect");
                    return View(user);
                }
                FormsAuthentication.SetAuthCookie(user.AdminUsername, false);
                Session["AdminUser"] = user;
                //return Redirect(ReturnUrl);
                if (Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);
                else
                    return RedirectToAction("Index", "Admin/Banner");            
        }

        public ActionResult Logout()
        {
            string user = ((AdminUser)Session["AdmiUser"]).AdminUsername;
            
            FormsAuthentication.SignOut();
            Session["AdminUser"] = null;
            Session.Abandon();
            Session.Clear();
            
            return Redirect(FormsAuthentication.LoginUrl);
        }

        

        

    }
}
