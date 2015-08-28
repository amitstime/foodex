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
    public class CustomerUserController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/CustomerUser/

        public ActionResult Index(int? page)
        {
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            var users = db.Customers.Where(a=>a.Status!="Thrashed").ToList();
            return View((users.ToList()).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThrashedUsers()
        {
            var users = db.Customers.Where(a => a.Status == "Thrashed").ToList();
            return View(users);
        }

        //
        // GET: /Admin/CustomerUser/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        public ActionResult Thrash(int id = 0)
        {
            Customer c1 =  db.Customers.Find(id);
            c1.Status = "Thrashed";
            
                c1.IsActive = false;
                db.Entry(c1).State = EntityState.Modified;
                db.SaveChanges();
            

            return RedirectToAction("index");
        }
        public ActionResult Restore(int id = 0)
        {
            Customer c1 = db.Customers.Find(id);
            c1.Status = "Active";
            c1.IsActive = true;

            db.Entry(c1).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ThrashedUsers");
        }
        public ActionResult ActiveInactive(int id)
        {
            Customer c1 = db.Customers.Find(id);

            if (c1.IsActive == true)
            {
                c1.IsActive = false;
            }
            else if (c1.IsActive == false)
            {
                c1.IsActive = true;
            }
            db.Entry(c1).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Delete(int id = 0)
        {
            Customer c1 = db.Customers.Find(id);
            if (c1 == null)
            {
                return HttpNotFound();
            }
            return View(c1);
        }

        //
        // POST: /Admin/CustomerUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
           /* User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();*/
            //User firstuser = db.Users.Find(id);

            Customer c1 = db.Customers.Find(id);
            
            if (c1.Orders.ToList() == null) { }
            else
            {
                foreach (var u in c1.Orders.ToList())
                {
                    if (u.OrderDetails.ToList() == null) { }
                    else
                    {
                        foreach (var o in u.OrderDetails.ToList())
                        {
                            db.OrderDetails.Remove(o);
                            db.SaveChanges();
                        }
                    }
                    db.Orders.Remove(u);
                    db.SaveChanges();
                }
            }
            db.Customers.Remove(c1);
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