using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
//using System.Windows.Forms;
using PagedList;

namespace FoodExNepal.Controllers
{
    public class RestaurantListController : Controller
    {
        //
        // GET: /Restaurant/
        FoodExEntities db = new FoodExEntities();
        public ActionResult Index(int? page, string txtKeyWord = "", string Location = "")
        {
            string Keyword = txtKeyWord; 
            
            if (Keyword != "")                
                ViewBag.Keyword = Keyword;                                    
            else                
                ViewBag.Keyword = String.Empty;                                             
            
                       
            ViewBag.Bread = "Restaurant";
            
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            List<FoodItem> food = db.FoodItems.ToList();
            List<Restaurant> restaurant = new List<Restaurant>();
           
            if (Keyword != "")
            {
                food = food.Where(a => a.FoodItemName.ToUpper().Contains(Keyword.ToUpper())).ToList();
                foreach (var f in food)
                {
                    Restaurant r = new Restaurant();
                    r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

                    if (!restaurant.Contains(r))
                        restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));                   
                }                
            }

            if (Location != "")
            {
                int loc = Convert.ToInt32(Location);
                restaurant = restaurant.Where(a => a.LocationId==loc).ToList();                
            }

            if (Keyword == "" && Location == "")
            {
                restaurant = db.Restaurants.ToList();
            }
            
            
           // ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName");
            
            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName", ViewBag.Location);
            return View(restaurant.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(int? page, string txtKeyword = "", string Location = "", string SortBy = "", string txtQuickRestaurantName="")
        {
            bool filterCheckStatus = false;
            string Keyword = txtKeyword;
            string Locations = Location;
            string SortBys = SortBy;
            string RestaurantName = txtQuickRestaurantName;

            ViewBag.Bread = "Restaurant";
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            List<FoodItem> food = db.FoodItems.ToList();
            List<Restaurant> restaurant = new List<Restaurant>();

            if (Keyword == "" && Locations == "" && SortBys == "-- Select Sort By --" && RestaurantName == "")
            {
                restaurant = db.Restaurants.ToList();
                filterCheckStatus = true;
            }
            else
            {
                if (Keyword != "")
                {
                    filterCheckStatus = true;
                    food = food.Where(a => a.FoodItemName.ToUpper().Contains(Keyword.ToUpper())).ToList();
                    foreach (var f in food)
                    {
                        Restaurant r = new Restaurant();
                        r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

                        if (!restaurant.Contains(r))
                            restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));
                    }
                    //restaurant = restaurant.Where(a => a.CusineInfo.ToLower().Contains(Keyword.ToLower())).ToList();
                }
                if (Locations != "")
                {
                    int loc = Convert.ToInt32(Location);
                    if (filterCheckStatus == false)
                    {
                        restaurant = db.Restaurants.ToList();
                        filterCheckStatus = true;
                    }

                    restaurant = restaurant.Where(a => a.LocationId == loc).ToList();
                }

                if (SortBys != "-- Select Sort By --")
                {
                    if (filterCheckStatus == false)
                    {
                        restaurant = db.Restaurants.ToList();
                        filterCheckStatus = true;
                    }
                    if (restaurant.Count > 0)
                    {
                        if (SortBys == "Restaurant Name")
                            restaurant = restaurant.OrderBy(a => a.RestaurantName).ToList();
                        else if (SortBys == "Minimum Order Amount")
                            restaurant = restaurant.OrderBy(a => a.MinOrderAmount).ToList();
                    }
                }

                if (RestaurantName != "")
                {
                    if (filterCheckStatus == false)
                    {
                        restaurant = db.Restaurants.ToList();
                        filterCheckStatus = true;
                    }
                    if (restaurant.Count > 0)
                    {
                        restaurant = restaurant.Where(a => a.RestaurantName.ToUpper().Contains(RestaurantName.ToUpper())).ToList();
                    }
                }
            }

            if (Keyword != "")
                ViewBag.Keyword = Keyword;
            else
                ViewBag.Keyword = "";

            if (Locations != "")
                ViewBag.Location = Locations;
            else
                ViewBag.Location = String.Empty;

            if (RestaurantName != "")
                ViewBag.QuickRestaurantName = RestaurantName;
            else
                ViewBag.QuickRestaurantName = String.Empty;

            if (SortBys != "")
                ViewBag.SortBy = SortBys;
            else
                ViewBag.SortBy = "-- Select Sort By --";

            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName", ViewBag.Location);
            return View(restaurant.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult FoodMenu(int id = 0, string txtSearchRes="",int  FoodCategoryid=0)
        {
            ViewBag.Bread = "Food";
            ViewBag.Restaurantid = id;
            ViewBag.Searchtxt = txtSearchRes;
            List<int> categoryid = new List<int>();
            List<FoodCategory> foodCategory =foodCategory = db.FoodCategories.Where(f => f.RestaurantId == id).ToList();
            if (txtSearchRes != "")
            {
                foodCategory.Clear();
                categoryid=(from f in  db.FoodItems.Where(c=>c.FoodItemName.ToUpper().Contains(txtSearchRes.ToUpper())) select f.FoodCategoryId).Distinct().ToList();
                foreach (int fcid in categoryid)
                {
                    FoodCategory fc = db.FoodCategories.Where(a=>a.FoodCategoryId==fcid).FirstOrDefault();
                    foodCategory.Add(fc);
                }
            }
            
            if (FoodCategoryid != 0)
            {
                foodCategory = foodCategory.Where(c => c.FoodCategoryId == FoodCategoryid).ToList();
            }
           
            if ((db.FoodItems.Where(r => r.RestaurantId == id && r.IsHotDeal == true).Count()) > 0)
            {
                FoodCategory hotdeal = new FoodCategory();
                hotdeal.FoodCategoryName = "Hot Deal";
                hotdeal.FoodCategoryId = 0;
                foodCategory.Insert(0, hotdeal);
            }
            ViewBag.FoodCategoryid = new SelectList(foodCategory, "FoodCategoryId", "FoodCategoryName", string.Empty);
            Session["Restaurant"] = db.Restaurants.Single(f => f.RestaurantId == id);
            return View(foodCategory);
        }
        public ActionResult LoadFoodItem(int id = 0, int Restaurantid=0,string searchtxt="")
        {
            Cart cart = (Cart)Session["Cart"];

            if (cart != null)
            {
                if (cart.cartDetail.Count != 0 && cart.cartSummary != null)
               
                {
                    int cartrestid = ((Cart)Session["Cart"]).restaurantsess.RestaurantId;
                    ViewBag.Restaurantid = cartrestid;
                }
            }
            var foodItem = db.FoodItems.Where(fi => fi.FoodCategoryId == id && fi.RestaurantId == Restaurantid);
            if (searchtxt != "")
            {
                foodItem = foodItem.Where(c=>c.FoodItemName.ToUpper().Contains(searchtxt.ToUpper()));
            }
            return PartialView(foodItem);
        }
        public ActionResult LoadHotDealFoodItem( int Restaurantid = 0)
        {
            Cart cart = (Cart)Session["Cart"];

            if (cart != null)
            {
                if (cart.cartDetail.Count != 0 && cart.cartSummary != null)
                {
                    int cartrestid = ((Cart)Session["Cart"]).restaurantsess.RestaurantId;
                    ViewBag.Restaurantid = cartrestid;
                }
            }
            var foodItem = db.FoodItems.Where(fi=>fi.RestaurantId == Restaurantid && fi.IsHotDeal==true);
            return PartialView(foodItem);
        }
        public ActionResult FoodItemDetail(int id=0)
        {
            ViewBag.Bread = "Food";
            FoodItem fooditem = db.FoodItems.Where(fi=>fi.FoodItemId==id).FirstOrDefault();
            return View(fooditem);
        }
        public ActionResult RestaurantDetail(int id=0)
        {
            ViewBag.Bread = "Restaurant";
            ViewData["Location"] = new SelectList(db.Locations.OrderBy(a => a.LocationName), "LocationId", "LocationName");
           
            Restaurant restaurant = db.Restaurants.Where(a => a.RestaurantId == id).FirstOrDefault();
            return View(restaurant);
        }
        public ActionResult FoodMenuMobile(int id = 0)
        {
            var food = db.FoodItems.Where(a => a.FoodCategoryId == id).ToList();
            return PartialView(food);
        }
        public ActionResult GetFoodItemList(int? page, int id = 0, string txtSearchRes = "", int FoodCategoryid = 0)
        {
            ViewBag.Bread = "Food";
            ViewBag.Restaurantid = id;
            ViewBag.Searchtxt = txtSearchRes;
            List<FoodCategory> foodCategory = foodCategory = db.FoodCategories.Where(f => f.RestaurantId == id).ToList();
            
            ViewBag.FoodCategoryid = new SelectList(foodCategory, "FoodCategoryId", "FoodCategoryName", string.Empty);
            
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            List<FoodItem> food = db.FoodItems.Where(fi=>fi.RestaurantId==id).ToList();
            if (FoodCategoryid != 0)
            {
                food = food.Where(fi => fi.FoodCategoryId == FoodCategoryid).ToList();
                ViewBag.FoodCategoryid = new SelectList(foodCategory, "FoodCategoryId", "FoodCategoryName", FoodCategoryid);
            }
            if (txtSearchRes != "")
            {
                food = food.Where(fi => fi.FoodItemName.ToUpper().Contains(txtSearchRes.ToUpper())).ToList();
            }
            return View(food.ToPagedList(pageNumber, pageSize));
        }


        
        public ActionResult FoodItemSearch(int? page, string txtCuisines = "", string RestLocation = "", string SortBy = "", string txtRestaurantName="")
        {
            ViewBag.Bread = "Food";
             string Keyword = txtCuisines;
            string Location = RestLocation;
            if (Keyword != "")
                ViewBag.Keyword = Keyword;
            else
                ViewBag.Keyword = "";
            if (Location != "")
                ViewBag.RestLocation = new SelectList(db.Locations, "LocationId", "LocationName", Location);
            else
                ViewBag.RestLocation = new SelectList(db.Locations, "LocationId", "LocationName", string.Empty);
            if (SortBy != "")
                ViewBag.SortBy = SortBy;
            else
                ViewBag.SortBy = "";
            if (txtRestaurantName != "")
                ViewBag.Restaurant = txtRestaurantName;
            else
                ViewBag.Restaurant = String.Empty;
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            List<FoodItem> food = db.FoodItems.ToList();
            List<Restaurant> restaurant = new List<Restaurant>();
            if (Keyword != "" && Location != "")
            {
                string KeyWord = Keyword;
                int LocationId = Convert.ToInt32(Location);
                food = food.Where(a => a.FoodItemName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.RestaurantName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.LocationId == LocationId || a.FoodCategory.FoodCategoryName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
                Restaurant r = new Restaurant();
                foreach (var f in food)
                {
                    r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

                    if (!restaurant.Contains(r))
                        restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));
                }
                //restaurant = restaurant.Where(a =>a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();

            }
            if (Keyword != "" && Location == "")
            {
                string KeyWord = Keyword;
                //int LocationId = Convert.ToInt32(Search["Location"].ToString());
                food = food.Where(a => a.FoodItemName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.RestaurantName.ToUpper().Contains(KeyWord.ToUpper()) || a.FoodCategory.FoodCategoryName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
                Restaurant r = new Restaurant();
                foreach (var f in food)
                {
                    r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

                    if (!restaurant.Contains(r))
                        restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));
                }
                List<Restaurant> restaurant1 = db.Restaurants.Where(a => a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
                foreach (var a in restaurant1)
                {
                    if (!restaurant.Contains(a))
                        restaurant.Add(a);
                }
            }
            if (Keyword == "" && Location != "")
            {
                restaurant = db.Restaurants.ToList();
                int LocationId = Convert.ToInt32(Location);
                restaurant = restaurant.Where(l => l.LocationId == LocationId).ToList();
            }
            if (Keyword == "" && Location == "")
            {
                restaurant = db.Restaurants.ToList();

            }

            ViewBag.RestaurantSelected = "Checked";
            ViewBag.FoodItemsSelected = "";

            if (Keyword != "")
                ViewBag.Keyword = Keyword;
            else
                ViewBag.Keyword = "";
            if (Location != "")
                ViewBag.LocationSelectedId = Location;
            else
                ViewBag.LocationSelectedId = String.Empty;
            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName");
           return View(restaurant.ToPagedList(pageNumber, pageSize));
        }
//        [HttpPost]
//        public ActionResult FoodItemSearch2222(int? page, System.Web.Mvc.FormCollection Search)
//        {
//            ViewBag.Bread = "Food";
//            int pageSize = 5;
//            int pageNumber = (page ?? 1);
//            List<FoodItem> food = db.FoodItems.ToList();
//            List<Restaurant> restaurant = new List<Restaurant>();
//            if (Search["Location"].ToString()!="")
//                ViewBag.RestLocation = new SelectList(db.Locations, "LocationId", "LocationName", Search["Location"].ToString());
//            else
//                ViewBag.RestLocation = new SelectList(db.Locations, "LocationId", "LocationName", string.Empty);

//            if (Search["KeyWord"].ToString() != "" && Search["Location"].ToString() != "")
//            {
//                string KeyWord = Search["KeyWord"].ToString();
//                int LocationId = Convert.ToInt32(Search["Location"].ToString());
//                food = food.Where(a => a.FoodItemName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.RestaurantName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.LocationId == LocationId || a.FoodCategory.FoodCategoryName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
//                Restaurant r = new Restaurant();
//                foreach (var f in food)
//                {
//                    r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

//                    if (!restaurant.Contains(r))
//                        restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));
//                }
//                //restaurant = restaurant.Where(a =>a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();

//            }
//            if (Search["KeyWord"].ToString() != "" && Search["Location"].ToString() == "")
//            {
                
//                string KeyWord = Search["KeyWord"].ToString();
//                //int LocationId = Convert.ToInt32(Search["Location"].ToString());
//                food = food.Where(a => a.FoodItemName.ToUpper().Contains(KeyWord.ToUpper()) || a.Restaurant.RestaurantName.ToUpper().Contains(KeyWord.ToUpper()) || a.FoodCategory.FoodCategoryName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
//                food = food.OrderBy(a => a.FoodItemRate).ToList();
               
//                /* Restaurant r = new Restaurant();
//                foreach (var f in food)
//                {
//                    r = db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId);

//                    if (!restaurant.Contains(r))
//                        restaurant.Add(db.Restaurants.Single(a => a.RestaurantId == f.RestaurantId));
//                }
//                List<Restaurant> restaurant1 = db.Restaurants.Where(a => a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
//                foreach (var a in restaurant1)
//                {
//                    if (!restaurant.Contains(a))
//                        restaurant.Add(a);
//                } */

//                ViewBag.Food = food;
//            }

//            if (Search["KeyWord"].ToString() == "" && Search["Location"].ToString() != "")
//            {
//                restaurant = db.Restaurants.ToList();
//                int LocationId = Convert.ToInt32(Search["Location"].ToString());
//                restaurant = restaurant.Where(l => l.LocationId == LocationId).ToList();
//            }
//            if (Search["KeyWord"].ToString() == "" && Search["Location"].ToString() == "")
//            {
//                restaurant = db.Restaurants.ToList();

//            }

//            if (ViewBag.RestaurantSelected == "Checked")
//            {
//                ViewBag.RestaurantSelected = "Checked";
//                ViewBag.FoodItemsSelected = "";
//            }
//            else
//            {
//                ViewBag.RestaurantSelected = "";
//                ViewBag.FoodItemsSelected = "Checked";
//            }

//            if (Search["KeyWord"].ToString() != "")
//                ViewBag.Keyword = Search["KeyWord"];
//            else
//                ViewBag.Keyword = "";
//            if (Search["Location"].ToString() != "")
//                ViewBag.LocationSelectedId = Search["Location"];
//            else
//                ViewBag.LocationSelectedId = String.Empty;
//            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName", ViewBag.LocationSelectedId);
//            if (Search["rdoRestaurantFoodItem"].ToString() == "Restaurant")
//            {
//                return RedirectToAction("Index", new { txtRestaurantName = Search["KeyWord"], RestLocation = Search["Location"] });
//            } 
//            else
//            {
//return View((restaurant.OrderBy(a => a.RestaurantName)).ToPagedList(pageNumber, pageSize));
//            }
            
//        }

    }
}
