using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Windows.Forms;
using FoodExNepal.Models;
namespace FoodExNepal.Controllers
{
    public class ShoppingCartController : Controller
    {
        FoodExEntities db = new FoodExEntities();
        //
        // GET: /ShoppingCart/

        public ActionResult Index(string message="") //Get Cart Collection
        {
            //var cart = new Cart();
            //if (Session["Cart"] != null)
            //{
            //    cart = (Cart)Session["Cart"];
            //}
           ViewData["message"]= message;
            return PartialView();
        }
        
        public ActionResult AddToCart(int id=0,int quantity=0) //Get Cart Collection
        {
            CartDetail cartdetail = new CartDetail();
            List<CartDetail> cartdetaillist = new List<CartDetail>();
            CartSummary cartsummary = new CartSummary();
            Cart cart=new Cart();
            FoodItem foodItem = db.FoodItems.Single(fi => fi.FoodItemId == id);
            Session["Restaurant"] = db.Restaurants.Single(f => f.RestaurantId == foodItem.RestaurantId);
            cartdetail.FoodItemId = id;
            cartdetail.FoodItem = foodItem.FoodItemName;
            cartdetail.Rate = (decimal)foodItem.FoodItemRate;
            cartdetail.Quantity = quantity;
            cartdetail.Total = (decimal)foodItem.FoodItemRate * quantity;
            bool isAdded = false;
            int servicePercent = 0;
            int vatpercent = 0;
            decimal subtotal=0;
            decimal service = 0;
            decimal vat = 0;
            decimal deliverycharge = 0;
            decimal gtotal = 0;
            Restaurant restaurant=(Restaurant)Session["Restaurant"];
            ViewBag.message = "Added";
            if(restaurant.IsServiceChargeApplicable==true)
            servicePercent=Convert.ToInt32(restaurant.ServiceChargePercent);
            if(restaurant.IsVatApplicable==true)
                vatpercent=Convert.ToInt32(restaurant.VATPercent);
            if (Session["Cart"] != null)
            {
                Restaurant sesrest = ((Cart)Session["Cart"]).restaurantsess;
                if (sesrest != null)
                {
                    if (sesrest.RestaurantId != restaurant.RestaurantId)
                    {
                        //MessageBox.Show("Alert");
                        Session["Cart"] = null;
                        cartdetaillist.Add(cartdetail);
                        cart.cartDetail = cartdetaillist;
                        ViewData["message"] = "RestaurantChanged";
                        //subtotal = cartdetail.Rate * cartdetail.Quantity;
                        //cartsummary.SubTotal = subtotal;
                        //service = (subtotal) * servicePercent / 100;
                        //cartsummary.ServiceCharge = service;
                        //vat = (service + subtotal) * vatpercent / 100;

                        //cartsummary.VAT = vat;
                        //cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
                        //cart.cartSummary = cartsummary;
                        //cartsummary.VATPercent = vatpercent;
                        //cartsummary.ServiceChargePercent = servicePercent;
                        //cart.cartSummary = cartsummary;
                       
                        cart.restaurantsess = (Restaurant)Session["Restaurant"];
                        Session["Cart"]=cart;
                        isAdded = true;
                    }
                    
                }
                else
                {
                    cart.restaurantsess = restaurant;
                }
                cart = (Cart)Session["Cart"];
                cartdetaillist = cart.cartDetail;
                foreach (var c in cartdetaillist)
                {
                    if(isAdded==false)
                    if (c.FoodItemId == id)
                    {
                        c.Quantity = c.Quantity + quantity;
                        c.Total = c.Rate * c.Quantity;
                        isAdded = true;
                    }
                    
                }
                if (isAdded == false)
                {
                    cartdetaillist.Add(cartdetail);
                 }
                foreach (var c in cartdetaillist)
                {
                    subtotal = subtotal+c.Rate * c.Quantity;
                }
                
                cartsummary.SubTotal = subtotal;
                service = (subtotal) * servicePercent / 100;
                cartsummary.ServiceCharge = service;
                vat = (service+subtotal) * vatpercent / 100;
                cartsummary.VAT = vat;
                cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
                cartsummary.ServiceChargePercent = servicePercent;
                cartsummary.VATPercent = vatpercent;
                cart.cartSummary = cartsummary;
            }
            else
            {
                cartdetaillist.Add(cartdetail);
                cart.cartDetail = cartdetaillist;
                subtotal=cartdetail.Rate * cartdetail.Quantity;
                cartsummary.SubTotal = subtotal;
                service = (subtotal) * servicePercent / 100;
                cartsummary.ServiceCharge = service;
                vat = (service + subtotal) * vatpercent / 100;
                
                cartsummary.VAT = vat;
                cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
                cart.cartSummary = cartsummary;
                cartsummary.VATPercent = vatpercent;
                cartsummary.ServiceChargePercent = servicePercent;
                cart.cartSummary = cartsummary;
            }
            cart.restaurantsess = restaurant;
            Session["Cart"] = cart;
            return RedirectToAction("Index", new { message = ViewData["message"] });
        }

        public ActionResult RemoveFromCart(int id = 0)
        {
            int servicePercent = 0;
            int vatpercent = 0;
            decimal subtotal = 0;
            decimal service = 0;
            decimal vat = 0;
            decimal deliverycharge = 0;
            decimal gtotal = 0;
            bool isRemoved = false;
            var cart = new Cart();
            Restaurant restaurant = (Restaurant)Session["Restaurant"];
            if (restaurant.IsServiceChargeApplicable == true)
                servicePercent = Convert.ToInt32(restaurant.ServiceChargePercent);
            if (restaurant.IsVatApplicable == true)
                vatpercent = Convert.ToInt32(restaurant.VATPercent);
            
            CartDetail od = new CartDetail();
            cart = (Cart)Session["Cart"];
            List<CartDetail> listorderdetail = cart.cartDetail;
            CartSummary cartsummary = cart.cartSummary;
            //Order order = cart.Orders;

            foreach (CartDetail c in listorderdetail)
            {
                if (c.FoodItemId == id)
                {
                    listorderdetail.Remove(c);
                    break;
                }
            }
            foreach (var c in listorderdetail)
            {
                subtotal = subtotal + c.Rate * c.Quantity;
            }

            cartsummary.SubTotal = subtotal;
            service = (subtotal) * servicePercent / 100;
            cartsummary.ServiceCharge = service;
            vat = (service+subtotal) * vatpercent / 100;
            cartsummary.VAT = vat;
            cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
            cart.cartSummary = cartsummary;
            cartsummary.ServiceChargePercent = servicePercent;
            cartsummary.VATPercent = vatpercent;
            cart.cartSummary = cartsummary;
            Session["Cart"] = cart;
            isRemoved = false;
            ViewBag.message = "Removed";
            return RedirectToAction("Index",new { message = ViewData["message"] });
        }
        public ActionResult ChangeCart(int id = 0, int quantity = 0)
        {
            CartDetail cartdetail = new CartDetail();
            List<CartDetail> cartdetaillist = new List<CartDetail>();
            CartSummary cartsummary = new CartSummary();
            Cart cart = new Cart();
            FoodItem foodItem = db.FoodItems.Single(fi => fi.FoodItemId == id);
            cartdetail.FoodItemId = id;
            cartdetail.FoodItem = foodItem.FoodItemName;
            cartdetail.Rate = (decimal)foodItem.FoodItemRate;
            cartdetail.Quantity = quantity;
            cartdetail.Total = (decimal)foodItem.FoodItemRate * quantity;
            bool isAdded = false;
            int servicePercent = 0;
            int vatpercent = 0;
            decimal subtotal = 0;
            decimal service = 0;
            decimal vat = 0;
            decimal deliverycharge = 0;
            decimal gtotal = 0;
            Restaurant restaurant = (Restaurant)Session["Restaurant"];
            if (restaurant.IsServiceChargeApplicable == true)
                servicePercent = Convert.ToInt32(restaurant.ServiceChargePercent);
            if (restaurant.IsVatApplicable == true)
                vatpercent = Convert.ToInt32(restaurant.VATPercent);
            if (Session["Cart"] != null)
            {
                cart = (Cart)Session["Cart"];
                cartdetaillist = cart.cartDetail;
                foreach (var c in cartdetaillist)
                {
                    if (c.FoodItemId == id && quantity == 0)
                    {
                        c.Quantity = 1;
                    }
                    else
                    {
                        if(c.FoodItemId==id)
                        {
                        c.Quantity = quantity;
                        c.Total = c.Rate * c.Quantity;
                        isAdded = true;
                        }
                    }

                }
                
                foreach (var c in cartdetaillist)
                {
                    subtotal = subtotal + c.Rate * c.Quantity;
                }

                cartsummary.SubTotal = subtotal;
                service = (subtotal) * servicePercent / 100;
                cartsummary.ServiceCharge = service;
                vat = (service + subtotal) * vatpercent / 100;
                cartsummary.VAT = vat;
                cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
                cartsummary.ServiceChargePercent = servicePercent;
                cartsummary.VATPercent = vatpercent;
                cart.cartSummary = cartsummary;
            }
            else
            {
                cartdetaillist.Add(cartdetail);
                cart.cartDetail = cartdetaillist;
                subtotal = cartdetail.Rate * cartdetail.Quantity;
                cartsummary.SubTotal = subtotal;
                service = (subtotal) * servicePercent / 100;
                cartsummary.ServiceCharge = service;
                vat = (service + subtotal) * vatpercent / 100;

                cartsummary.VAT = vat;
                cartsummary.GrossTotal = subtotal + service + vat + deliverycharge;
                cart.cartSummary = cartsummary;
                cartsummary.VATPercent = vatpercent;
                cartsummary.ServiceChargePercent = servicePercent;
                cart.cartSummary = cartsummary;
            }
            Session["Cart"] = cart;
            ViewBag.message = "Updated";
            return RedirectToAction("Index", new { message = ViewData["message"] });
        }
        public ActionResult CheckOut()
        {
            if (Session["Customer"] == null)
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });
            }
            ViewBag.Bread = "Checkout";
            ViewData["Location"] = new SelectList(db.Locations, "LocationName", "LocationName");
            
            return View();
        }
        [HttpPost]
        public ActionResult CheckOut(System.Web.Mvc.FormCollection checkout)
        {
            ViewBag.Bread = "Checkout";
            if (Session["Customer"] == null)
            {
                string returnurl = Request.Url.LocalPath.ToString();
                return RedirectToAction("Login", "Customer", new { Returnurl = returnurl });
            }
            Order order = new Order();
            order.CustomerId = ((Customer)Session["Customer"]).CustomerId;
            order.RestaurantId = ((Cart)Session["Cart"]).restaurantsess.RestaurantId;
            order.Amount = (double)((Cart)Session["Cart"]).cartSummary.GrossTotal;
            order.DeliveryStatus = "Not Confirmed";
            order.IsOrderConfirmed = false;
            order.VATPercent = ((Cart)Session["Cart"]).cartSummary.VATPercent;
            order.RestaurantServiceChargePercent = ((Cart)Session["Cart"]).cartSummary.ServiceChargePercent;
            order.DeliveryCharge = 0;

            string a = checkout["rdbcheckout"].ToString();
            if (checkout["rdbcheckout"].ToString() == "Delivertome")
            {
                order.FullName = checkout["txtFName"];
                order.DeliveryAddress = checkout["txtAddress"];
                order.Phone = checkout["txtPhone"];
                order.NearestLocation = checkout["Location"];
                order.DeliveryMeOrSelfPickup = "Delivery To Me";
            }
            else
            {
                order.FullName = "NA";
                order.DeliveryAddress = "NA";
                order.Phone = "NA";
                order.NearestLocation = "NA";
                order.DeliveryMeOrSelfPickup = "Pick up";
            }
            if (checkout["rdbAsSoonAs"] == "AsSoonAsPossible")
            {
                order.Take_awayDateTime = "As soon as possible";
            }
            else
            {
                order.Take_awayDateTime = checkout["DeliveryDate"].ToString()+" - " + checkout["DeliveryTime"];
            }
            order.DateTime = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();
            int orderid = order.OrderId;
            List<CartDetail> orderlist=((Cart)Session["Cart"]).cartDetail;
            foreach (CartDetail cd in orderlist)
            {
                OrderDetail orderdetail = new OrderDetail();
                orderdetail.FoodItemId = cd.FoodItemId;
                orderdetail.Quantity = cd.Quantity;
                orderdetail.Rate = (double)cd.Rate;
                orderdetail.OrderId = orderid;
                db.OrderDetails.Add(orderdetail);
                db.SaveChanges();
            }
            //MessageBox.Show("Your order has been placed for confirmation");
            ViewData["Location"] = new SelectList(db.Locations, "LocationName", "LocationName");
            Session["Cart"] = null;
            string msg="Order placed successfully";
            return RedirectToAction("OrderHistory", "Customer", new { Msg = msg });
             

        }
    }
}
