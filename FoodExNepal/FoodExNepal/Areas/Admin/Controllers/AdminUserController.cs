using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using FoodExNepal.Models;

namespace FoodExNepal.Areas.Admin.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class AdminUserController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/Admin/

        public ActionResult Index()
        {
            return View(db.AdminUsers.ToList());
        }

        //
        // GET: /Admin/Admin/Details/5

        public ActionResult Details(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // GET: /Admin/Admin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Admin/Create

        [HttpPost]
        public ActionResult Create(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                db.AdminUsers.Add(adminuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminuser);
        }

        //
        // GET: /Admin/Admin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // POST: /Admin/Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(AdminUser adminuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminuser);
        }

        //
        // GET: /Admin/Admin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            if (adminuser == null)
            {
                return HttpNotFound();
            }
            return View(adminuser);
        }

        //
        // POST: /Admin/Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminUser adminuser = db.AdminUsers.Find(id);
            db.AdminUsers.Remove(adminuser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Logout()
        {
            //string user = ((AdminUser)Session["AdmiUser"]).AdminUsername;

            FormsAuthentication.SignOut();
            Session["AdminUser"] = null;
            Session.Abandon();
            Session.Clear();

            return Redirect(FormsAuthentication.LoginUrl);
        }
        
    }
}