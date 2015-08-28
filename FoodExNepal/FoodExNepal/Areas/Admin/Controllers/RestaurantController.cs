using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
using System.IO;
using PagedList;
namespace FoodExNepal.Areas.Admin.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class RestaurantController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/Restaurant/

        public ActionResult Index(int? page, string Keyword = "", int Location = 0)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);
          //  List<Restaurant> restaurant = db.Restaurants.Where(a=>a.IsActive == true).ToList();
            List<Restaurant> restaurant = db.Restaurants.ToList();
            if (Keyword != "" )
            {
                string KeyWord = Keyword;
                
                restaurant = db.Restaurants.Where(a => a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();              
            }
            if (Location != 0)
            {
                restaurant = db.Restaurants.Where(a => a.LocationId == Location).ToList();
            }
            if (Keyword != "")
                ViewBag.Keyword = Keyword;
            else
                ViewBag.Keyword = "";
            if (Location != 0)
                ViewBag.LocationSelectedId = Location;
            else
                ViewBag.LocationSelectedId = String.Empty;
            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName");
           
            return View((restaurant).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(int? page, System.Web.Mvc.FormCollection Search)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            List<Restaurant> restaurant = db.Restaurants.Where(a => a.IsActive == true).ToList();
            if (Search["KeyWord"].ToString() != "")
                
            {
                string KeyWord = Search["KeyWord"].ToString();

                restaurant = db.Restaurants.Where(a => a.RestaurantName.ToUpper().Contains(KeyWord.ToUpper())).ToList();
            }
            if (Search["Location"].ToString() != "")
            {
                int loc = Convert.ToInt32(Search["Location"]);
                restaurant = db.Restaurants.Where(a => a.LocationId == loc).ToList();
            }
   
            if (Search["KeyWord"].ToString() != "")
                ViewBag.Keyword = Search["KeyWord"];
            else
                ViewBag.Keyword = "";
            if (Search["Location"].ToString() != "")
                ViewBag.LocationSelectedId = Search["Location"];
            else
                ViewBag.LocationSelectedId = String.Empty;
            ViewBag.Location = new SelectList(db.Locations, "LocationId", "LocationName", ViewBag.LocationSelectedId);
            return View((restaurant).ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Admin/Restaurant/Details/5

        public ActionResult Details(int id = 0)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);
        } 

        public ActionResult Foodmenu(int id=0)
        {
            Restaurant r1 = db.Restaurants.Find(id);
            return View(r1);
        }
        //
        // GET: /Admin/Restaurant/Create

        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations.OrderBy(a => a.LocationName), "LocationId", "LocationName");
            return View();
        }

        //
        // POST: /Admin/Restaurant/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Restaurant restaurant, HttpPostedFileBase fileArticle, HttpPostedFileBase RestaurantImage)
        {
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {
                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    restaurant.RestaurantLogo = fileName;
                }
                if (RestaurantImage != null)
                {
                    string fileName = checkfile(RestaurantImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    RestaurantImage.SaveAs(filePath);
                    restaurant.RestaurantImageInDetailPage = fileName;
                }
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
            return View(restaurant);
        }

        //
        // GET: /Admin/Restaurant/Edit/5

        public ActionResult Edit(int id = 0)
        {
            
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName",restaurant.LocationId);
            return View(restaurant);
        }

        //
        // POST: /Admin/Restaurant/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Restaurant restaurant, HttpPostedFileBase fileArticle, HttpPostedFileBase RestaurantImage)
        {
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {
                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    restaurant.RestaurantLogo = fileName;

                }
                if (RestaurantImage != null)
                {
                    string fileName = checkfile(RestaurantImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/restaurant"), Path.GetFileName(fileName));
                    RestaurantImage.SaveAs(filePath);
                    restaurant.RestaurantImageInDetailPage = fileName;
                }
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", restaurant.LocationId);
            return View(restaurant);
        }

        //
        // GET: /Admin/Restaurant/Delete/5

        public ActionResult Delete(int id = 0)
        {
            //Restaurant restaurant = db.Restaurants.Find(id);
            
            //if (restaurant == null)
            //{
            //    return HttpNotFound();
            //}
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            restaurant.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // POST: /Admin/Restaurant/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Deactive(int id = 0)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            restaurant.IsActive = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Restaurant_FoodCategory

        public ActionResult FoodCategoryAdd(int id = 0)
        {
            int restaurantId = id;
            if (restaurantId == 0)
            {
                return RedirectToAction("index");
            }
            //var restaurantObj = db.Restaurants.Find(restaurantId);
            ViewData["Restaurant"] = db.Restaurants.Find(restaurantId);
            
            return View();           
        }

      /*  [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodCategoryAdd(FoodCategory foodCategory)
        {
            if (ModelState.IsValid)
            {
                db.FoodCategories.Add(foodCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("FoodCategoryList", foodCategory.RestaurantId);
        }  */

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodCategoryAdd(FormCollection fc)
        {            
            FoodCategory f1 = new FoodCategory();                       
            
            f1.FoodCategoryName = fc["txtFoodCategory"];
            f1.FoodCategoryDescription = fc["txtDesc"];
            f1.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]);                      
            db.FoodCategories.Add(f1);
            db.SaveChanges();

            return RedirectToAction("FoodCategoryList", new { id = f1.RestaurantId });
        }

        public ActionResult FoodCategoryEdit(int id = 0)
        {
            FoodCategory foodcategory = db.FoodCategories.Find(id);
            if (foodcategory == null)
            {
                return HttpNotFound();
            }            
            return View(foodcategory);
        }
        public ActionResult FoodCategoryDetails(int id=0)
        {
            var foodcategory= db.FoodCategories.Find(id);
               
            return View(foodcategory);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodCategoryEdit(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FoodCategoryList", new { id = foodcategory.RestaurantId }); 
            }
            return View(foodcategory);
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult FoodCategoryEdit(FormCollection fc)
        //{
        //    FoodCategory f1 = new FoodCategory();

        //    f1.FoodCategoryName = fc["txtFoodCategory"];
        //    f1.FoodCategoryDescription = fc["txtDesc"];
        //    f1.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]);            
        //    db.SaveChanges();
        //    return RedirectToAction("FoodCategoryList", new { id = f1.RestaurantId });           
        //}

        public ActionResult FoodCategoryDelete(int id = 0)
        {
            FoodCategory foodCategory = db.FoodCategories.Find(id);
            if (foodCategory == null)
            {
                return HttpNotFound();
            }
            return View(foodCategory);
        }

         [HttpPost, ActionName("FoodCategoryDelete")]
        public ActionResult FoodCategoryDeleteConfirmed(int id)
        {
            FoodCategory foodCategory = db.FoodCategories.Find(id);
            int restaurantId = (int)foodCategory.RestaurantId;
            db.FoodCategories.Remove(foodCategory);
            db.SaveChanges();
            return RedirectToAction("FoodCategoryList", new { id = restaurantId });
        }

        public ActionResult FoodCategoryList(int id = 0)
        {
            int restaurantId = id;
            if (restaurantId == 0)
            {
                return RedirectToAction("index");
            }
            var restaurantObj = db.Restaurants.Find(restaurantId);
            ViewData["FoodCategory"] = db.FoodCategories.ToList();
            return View(restaurantObj);
        }

        public ActionResult GetRestaurantById(int id = 0)
        {
            Restaurant r1 = db.Restaurants.Find(id);
            return View(r1);
        }

       
        #endregion Restaurant_FoodCategory


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        #region Restaurant_FoodCategory_FoodItem

        public ActionResult FoodItemList(int id = 0)
        {
            int categoryId = id;
            if (categoryId == 0)
            {
                return RedirectToAction("index");
            }
            var categoryObj = db.FoodCategories.Find(categoryId);
            ViewData["FoodItem"] = db.FoodItems.ToList();
            return View(categoryObj);
        }

        public ActionResult FoodItemAdd(int id = 0)
        {
            int foodCategoryId = id;
            if (foodCategoryId == 0)
            {
                return RedirectToAction("index");
            }
            //var restaurantObj = db.Restaurants.Find(restaurantId);
            ViewData["FoodCategory"] = db.FoodCategories.Find(foodCategoryId);
            return View();
        }       

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodItemAdd(FormCollection fc, HttpPostedFileBase fileHotDealImage,HttpPostedFileBase fileFoodItemImage)
        {
            FoodItem f1 = new FoodItem();

            f1.FoodItemName = fc["txtFoodItemName"];
            f1.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]);
            f1.FoodCategoryId = Convert.ToInt32(fc["hddFoodCategoryId"]);
            f1.FoodDescription = fc["txtFoodDescription"];
            f1.FoodItemRate = Convert.ToDouble(fc["txtFoodItemRate"]);
            if (fileFoodItemImage != null)
            {
                string fileName = checkfile(fileFoodItemImage.FileName);
                string filePath = Path.Combine(Server.MapPath("~/images/Uploads"), Path.GetFileName(fileName));
                fileFoodItemImage.SaveAs(filePath);
                f1.FoodItemImage = fileName;
            }
            String hotDeals =  fc["chkHotDeals"];

            if (hotDeals == "on")
            {
                f1.IsHotDeal = true;
                f1.HotDealDescription = fc["txtHotDealDescription"];
                if (fileHotDealImage != null)
                {
                    string fileName = checkfile(fileHotDealImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/images/HotDeals"), Path.GetFileName(fileName));
                    fileHotDealImage.SaveAs(filePath);
                    f1.HotDealImage = fileName;
                }

            }
            db.FoodItems.Add(f1);
            db.SaveChanges();

            return RedirectToAction("FoodItemList", new { id = f1.FoodCategoryId });
        }

        public ActionResult FoodItemEdit(int id = 0)
        {
            FoodItem foodItem = db.FoodItems.Find(id);            
            ViewData["FoodCatergory"] = db.FoodCategories.OrderBy(a => a.FoodCategoryName).ToList();
            if (foodItem == null)
            {
                return HttpNotFound();
            }
            return View(foodItem);
        }

      /*  [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodItemEdit(FoodCategory foodcategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("FoodCategoryList", new { id = foodcategory.RestaurantId });
            }
            return View(foodcategory);
        } */                 

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodItemEdit(FormCollection fc, HttpPostedFileBase fileHotDealImage,HttpPostedFileBase fileFoodItemImage)
        {
            int id = Convert.ToInt32(fc["FoodItemId"]);
            FoodItem f1 = db.FoodItems.Find(id);
            f1.FoodItemId = Convert.ToInt32(fc["hddFoodItemId"]);
            f1.FoodItemName = fc["txtFoodItemName"];
            f1.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]);
            f1.FoodCategoryId = Convert.ToInt32(fc["hddFoodCategoryId"]);
            f1.FoodDescription = fc["txtFoodDescription"];
            f1.FoodItemRate = Convert.ToDouble(fc["txtFoodItemRate"]);
            if (fileFoodItemImage != null)
            {
                string fileName = checkfile(fileFoodItemImage.FileName);
                string filePath = Path.Combine(Server.MapPath("~/images/Uploads"), Path.GetFileName(fileName));
                fileFoodItemImage.SaveAs(filePath);
                f1.FoodItemImage = fileName;
            }
            String hotDeals = fc["chkHotDeals"];

            if (hotDeals == "on")
            {
                f1.IsHotDeal = true;
                f1.HotDealDescription = fc["txtHotDealDescription"];
                if (fileHotDealImage != null)
                {
                    string fileName = checkfile(fileHotDealImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/images/HotDeals"), Path.GetFileName(fileName));
                    fileHotDealImage.SaveAs(filePath);
                    f1.HotDealImage = fileName;
                }

            }
            else {
                f1.IsHotDeal = false;
                f1.HotDealDescription = "";
                f1.HotDealImage = "";
            }
            db.SaveChanges();
            return RedirectToAction("FoodItemList", new { id = f1.FoodCategoryId });
        }

        public ActionResult FoodItemDetails(int id = 0)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            if (foodItem == null)
            {
                return HttpNotFound();
            }
            return View(foodItem);
        }

        public ActionResult FoodItemDelete(int id = 0)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            if (foodItem == null)
            {
                return HttpNotFound();
            }
            return View(foodItem);
        }

        [HttpPost, ActionName("FoodItemDelete")]
        public ActionResult FoodItemDeleteConfirmed(int id)
        {
            FoodItem foodItem = db.FoodItems.Find(id);
            int restaurantId = (int)foodItem.FoodItemId;
            db.FoodItems.Remove(foodItem);
            db.SaveChanges();
            return RedirectToAction("FoodItemList", new { id = foodItem.FoodCategoryId });
        }

        public ActionResult FoodItemHotDeals(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            //ViewData["HotDealsItem"] = db.FoodItems.Where(f=> f.IsHotDeal == true).ToList();
            var hotDeal = db.FoodItems.Where(f => f.IsHotDeal == true).ToList();
            return View(hotDeal.ToPagedList(pageNumber, pageSize));
        }




        #endregion Restaurant_FoodCategory_FoodItem




        public ActionResult FoodAdd(int id=0)
        {
            if (id == 0)
            {
                return RedirectToAction("index");
            }
            ViewBag.RestaurantId = id;
            ViewData["FoodCatergory"] = db.FoodCategories.OrderBy(a => a.FoodCategoryName).ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodAdd(FormCollection fc, HttpPostedFileBase fileFoodItem)
        {
            

            int restaurantId = Convert.ToInt32(fc["hddRestaurantId"]);
            FoodItem f1 = new FoodItem();
            RestaurantFoodCategory rfcate = new RestaurantFoodCategory();
            rfcate.FoodCategoryId = Convert.ToInt32(fc["ddlFoodCategory"]);
            rfcate.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]); ;
            db.RestaurantFoodCategories.Add(rfcate);
            db.SaveChanges();
            f1.FoodItemName = fc["txtFoodItemName"];
            f1.FoodDescription = fc["txtDesc"];
            f1.RestaurantId = Convert.ToInt32(fc["hddRestaurantId"]);
            f1.FoodCategoryId = Convert.ToInt32(fc["ddlFoodCategory"]);
            f1.FoodItemRate = Convert.ToDouble(fc["txtRate"]);
            if (fileFoodItem != null)
            {
                string fileName = checkfile(fileFoodItem.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(fileName));
                fileFoodItem.SaveAs(filePath);
                f1.FoodItemImage = fileName;

            }
            db.FoodItems.Add(f1);
            db.SaveChanges();

            return RedirectToAction("Foodmenu", new { id = restaurantId});
        }
      /*  public ActionResult FoodItemEdit(int id = 0)
        {
            FoodItem fi = db.FoodItems.Find(id);
            var resfoodcat = db.RestaurantFoodCategories.SingleOrDefault(a => a.RestaurantId == fi.RestaurantId && a.FoodCategoryId == fi.FoodCategoryId);
            ViewBag.Rfcid=resfoodcat.RestaurantFoodCategoryId;
            ViewData["FoodCatergory"] = db.FoodCategories.OrderBy(a => a.FoodCategoryName).ToList();
            if (fi == null)
            {
                return HttpNotFound();
            }
            return View(fi);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FoodItemEdit(FormCollection fc, HttpPostedFileBase fileFooditem)
        {
            int id = Convert.ToInt16(fc["FoodItemId"]);
            int restfoodcat = Convert.ToInt32(fc["RestaurantFoodCategoryId"]);
            RestaurantFoodCategory rfc = db.RestaurantFoodCategories.Find(restfoodcat);
            rfc.RestaurantId = Convert.ToInt32(fc["RestaurantId"]);
            rfc.FoodCategoryId = Convert.ToInt32(fc["ddlFoodCategory"]);
            db.SaveChanges();
            FoodItem f = db.FoodItems.Where(a => a.FoodItemId == id).FirstOrDefault();
            FoodItem f1 = db.FoodItems.Find(id);
           int restaurantId = Convert.ToInt32(fc["RestaurantId"]);
           // FoodItem f1 = new FoodItem();
            f1.FoodItemName = fc["FoodItemName"];
            f1.FoodDescription = fc["FoodDescription"];
            f1.RestaurantId = restaurantId;
            f1.FoodCategoryId = Convert.ToInt32(fc["ddlFoodCategory"]);
            f1.FoodItemRate = Convert.ToDouble(fc["FoodItemRate"]);

           // int restaurantId = f1.RestaurantId;
            if (fileFooditem != null)
            {
                string fileName = checkfile(fileFooditem.FileName);
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(fileName));
                fileFooditem.SaveAs(filePath);
                f1.FoodItemImage = fileName;

            }
            else
            {
                f1.FoodItemImage = f.FoodItemImage;
            }
            //db.FoodItems.Add(f1);
            db.SaveChanges();

            return RedirectToAction("Foodmenu", new { id = restaurantId });
        }  */
        private string checkfile(String filename)
        {
            string filecount = (DateTime.Now.ToString()).Replace('/', '-');
            filecount = (filecount).Replace(' ', '-');
            filecount = (filecount).Replace(':', '-');
            return filecount + filename;

        }
        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}