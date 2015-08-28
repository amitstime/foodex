using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
namespace FoodExNepal.Areas.Admin.Controllers
{
   [Authorize]
   [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class OrderListController : Controller
    {
        FoodExEntities db = new FoodExEntities();
        //
        // GET: /Admin/OrderList/
        public ActionResult Index()
        {
            var orderlist = db.Orders.Where(a => a.IsOrderConfirmed == false && a.DeliveryStatus == "Not Confirmed").OrderBy(s => s.Take_awayDateTime);
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }
       [HttpPost]
        public ActionResult Index(FormCollection Search)
        {
           var orderlist = db.Orders.OrderBy(s => s.Take_awayDateTime).ToList();
           if(!string.IsNullOrEmpty(Search["Restaurant"]))
           {
               //string Rest1 = Search["Restaurant"];
               int Rest=Convert.ToInt32(Search["Restaurant"]);
               orderlist = orderlist.Where(o => o.RestaurantId == Rest).ToList();
           }
           if (!string.IsNullOrEmpty(Search["type"]))
           {
               string Rest = Search["type"];
               orderlist = orderlist.Where(a=>a.DeliveryMeOrSelfPickup.ToLower().Contains(Rest.ToLower())).ToList();
           }
         
           if (!string.IsNullOrEmpty(Search["orderno"]))
           {
               int Rest = Convert.ToInt32(Search["orderno"]);
               orderlist = orderlist.Where(o => o.OrderId == Rest).ToList();
           }
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }
       public ActionResult ConfirmedList()
        {
            var orderlist = db.Orders.Where(a => a.IsOrderConfirmed == true).OrderByDescending(s => s.OrderId);
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }

       public ActionResult DeliveredList()
        {
            var orderlist = db.Orders.Where(a => a.DeliveryStatus == "Delivered" ).OrderByDescending(s => s.OrderId);
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }
       public ActionResult CancelledList()
       {
           var orderlist = db.Orders.Where(a => a.DeliveryStatus == "Cancelled").OrderByDescending(s => s.OrderId);
           ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
           return View(orderlist);
       }
       [HttpPost]
        public ActionResult Edits()
        {

            return View();
        }
        
        public ActionResult Details(int id = 0)
        {
            Order Order = db.Orders.Where(or => or.OrderId == id).FirstOrDefault();
            ViewBag.OrderDetailList = db.OrderDetails.Where(o => o.OrderId == id).ToList();
            return View(Order);
        }
    public ActionResult Confirmed(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.IsOrderConfirmed = true;
            order.DeliveryStatus = "Confirmed";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       public ActionResult Cancelled(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.IsOrderConfirmed = false;
            order.DeliveryStatus = "Cancelled";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult OrderDetail(int id=0)
        {
            var ol = db.OrderDetails.Where(or => or.OrderDetailId == id);

            return PartialView(ol);

        }

        public ActionResult SalesHistory(int id = 0)
        {
            var orderlist = db.Orders.Where(a => a.DeliveryStatus == "Delivered").OrderByDescending(s => s.OrderId);
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }

        [HttpPost]
        public ActionResult SalesHistory(FormCollection Search)
        {
            var orderlist = db.Orders.Where(a => a.DeliveryStatus == "Delivered").OrderByDescending(s => s.OrderId).ToList();
            if (!string.IsNullOrEmpty(Search["Restaurant"]))
            {
                //string Rest1 = Search["Restaurant"];
                int Rest = Convert.ToInt32(Search["Restaurant"]);
                orderlist = orderlist.Where(o => o.RestaurantId == Rest).ToList();
            }
            if (!string.IsNullOrEmpty(Search["dateFrom"]))
            {
                DateTime Rest = Convert.ToDateTime(Search["dateFrom"]);
                orderlist = orderlist.Where(a => a.DateTime >= Rest).ToList();
            }

            if (!string.IsNullOrEmpty(Search["dateTo"]))
            {
                DateTime Rest = Convert.ToDateTime(Search["dateTo"]);
                orderlist = orderlist.Where(o => o.DateTime <= Rest).ToList();
            }
            ViewBag.Restaurant = new SelectList(db.Restaurants, "RestaurantId", "RestaurantName", string.Empty);
            return View(orderlist);
        }

    }
}
