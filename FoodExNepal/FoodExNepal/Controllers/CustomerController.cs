using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;

namespace FoodExNepal.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        FoodExEntities db = new FoodExEntities();

        public ActionResult Login(string Returnurl = "")
        {
            //ViewBag.Bread = "Checkout";
            ViewBag.Returnurl = Returnurl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection login, string Returnurl = "")
        {
            string email = login["Email"].ToString();
            string password = login["Password"].ToString();

            Customer customer = db.Customers.Where(u => u.Email == email && u.Password == password).SingleOrDefault();
            if (customer == null)
           {
               ModelState.AddModelError("", "Invalid Email or password");
               return View(customer);
           }
           else
           {
               if (customer.IsActive == false)
               {
                   ModelState.AddModelError("", "Invalid username or password");
                   return View(customer);
               }
               Session["Customer"] = customer;
              

           }
            
            ViewBag.Returnurl = Returnurl;
             if (Url.IsLocalUrl(Returnurl))
                 return Redirect(Returnurl);
            return RedirectToAction("Index","Home");
        }
        public ActionResult Logout()
        {
            Session["Customer"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register(string Returnurl = "")

        {
            ViewBag.Returnurl = Returnurl;
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection customer, string Returnurl = "")
        {
            Customer c1 = new Customer();
            
            try
            {
                if (string.IsNullOrEmpty(customer["txtFName"]))
                {
                    ModelState.AddModelError("FullName", "Please enter your name");
                }
                if (string.IsNullOrEmpty(customer["txtEmail"]))
                {
                    ModelState.AddModelError("Email", "Please enter your Email");
                }
                else  if (!string.IsNullOrEmpty(customer["txtEmail"]))
                {
                     string email = customer["txtEmail"];
                    if(db.Customers.Where(e=>e.Email==email).Count()>0)
                        ModelState.AddModelError("Email", "Email Already Exists");
                }
                if (string.IsNullOrEmpty(customer["txtContact"]))
                {
                    ModelState.AddModelError("Contact", "Please enter your contact Number");
                }
                if (string.IsNullOrEmpty(customer["txtAddress"]))
                {
                    ModelState.AddModelError("Address", "Please enter your Address");
                }
                if (string.IsNullOrEmpty(customer["txtPassowrd"]))
                {
                    ModelState.AddModelError("Password", "Please enter your Password");
                }
                if (string.IsNullOrEmpty(customer["txtRePassowrd"]))
                {
                    ModelState.AddModelError("RePassword", "Please enter your Re-Password");
                }
                if (!string.IsNullOrEmpty(customer["txtPassowrd"]) && !string.IsNullOrEmpty(customer["txtRePassowrd"]))
                if (customer["txtRePassowrd"] != customer["txtPassowrd"])
                {
                    ModelState.AddModelError("RePassword", "Confirm password doesn't match");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.FName= customer["txtFName"];
                    ViewBag.Email = customer["txtEmail"];
                    ViewBag.Contact = customer["txtContact"];
                    ViewBag.Address = customer["txtAddress"];


                   // ViewBag.E = customer["txtEmail"];
                    ViewBag.Password = customer["txtPassowrd"];
                    ViewBag.RePassword = customer["txtRePassowrd"];
                    return View();
                }
                c1.Name = customer["txtFName"];
                c1.Email = customer["txtEmail"];
                c1.ContactNumber = customer["txtContact"];
                c1.Address = customer["txtAddress"];
                c1.Status = "Active";

                //c1.UserName = customer["txtEmail"];
                c1.Password = customer["txtPassowrd"];
               // c1.UserType = "Customer";
                c1.IsActive = true;
                
                db.Customers.Add(c1);
                db.SaveChanges();
              //  u1.CustomerId = c1.CustomerId;

               // db.Users.Add(u1);
               // db.SaveChanges();
                ViewBag.Returnurl = Returnurl;
                if (Url.IsLocalUrl(Returnurl))
                    return Redirect(Returnurl);
            }
            catch( Exception e)
            {
                throw e;
            }

            return RedirectToAction("Index","Home");
        }

        public ActionResult MyAccount()
        {
            if (Session["Customer"] == null)
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });
            
            }
            Customer customer=(Customer)Session["Customer"];
            return View(customer);
        }
        public ActionResult EditAccount()

        {
            if (Session["Customer"] == null )
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });

            }
            Customer customer = (Customer)Session["Customer"];
            return View(customer);
        }

        [HttpPost]
        public ActionResult EditAccount(FormCollection customer)
        {
            if (Session["Customer"] == null)
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });

            }
            if (string.IsNullOrEmpty(customer["txtEmail"]))
            {
                ModelState.AddModelError("Email", "Please enter your Email");
            }
            else if (!string.IsNullOrEmpty(customer["txtEmail"]))
            {
                string email = customer["txtEmail"];
                if (db.Customers.Where(e => e.Email == email).Count() > 0)
                {
                    Customer cust1 = db.Customers.Where(e => e.Email == email).FirstOrDefault();
                    if (Convert.ToInt32(customer["CustomerId"])!=cust1.CustomerId)
                    ModelState.AddModelError("Email", "Email Already Exists");
                }
            }
           
            Customer cust = (Customer)Session["Customer"];
            var c1 = db.Customers.Find(cust.CustomerId);
            //User usr = (User)Session["User"];
           // var u1 = db.Users.Find(usr.UserId);
            try
            {
                c1.Name = customer["txtFName"];
                c1.Email = customer["txtEmail"];
                c1.ContactNumber = customer["txtContact"];
                c1.Address = customer["txtAddress"];
                c1.Status = "Active";

                //u1.UserName = customer["txtEmail"];
                c1.Password = customer["txtPassowrd"];
                //u1.UserType = "Customer";
                c1.IsActive = true;

                //db.Customers.Add(c1);
                db.SaveChanges();
                //u1.CustomerId = c1.CustomerId;
              //  db.Users.Add(u1);
              //  db.SaveChanges();
                
            }
            catch (Exception e)
            {
                throw e;
            }
            Session["Customer"]= db.Customers.Find(cust.CustomerId);
           // Session["User"] = db.Users.Find(usr.UserId);
            
            return RedirectToAction("MyAccount");
        }

        public ActionResult OrderHistory(string Msg="")
        {
            if (Session["Customer"] == null)
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });

            }
           var customer= (Customer)Session["Customer"];
           List<Order> order = db.Orders.Where(o => o.CustomerId == customer.CustomerId).OrderByDescending(c=>c.DateTime).ToList();
           ViewBag.MSG = Msg;
            return View(order);
        }
        public ActionResult OrderDetails(int id=0)
        {
            var orderdetail = db.OrderDetails.Where(od=>od.OrderId==id).ToList();
            return PartialView(orderdetail);
        }
        [HttpPost]
        public ActionResult CheckUserEmail(string email,int id=0)
        {
            //var a = db.Users.Find(email);

            var userEmail = (from u in db.Customers.Where(p => p.Email == email) select u).FirstOrDefault();
            
                //userEmail = userEmail.Where(u => u.CustomerId == id).ToList();
            string isExists = "";
            if (userEmail!=null)
                if(id!=userEmail.CustomerId)
                isExists = "Exists";
            else
                isExists = "Available";

            return Json(new { IsExisting = isExists });

        }

    }
}
