using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodExNepal.Models;
using System.IO;
namespace FoodExNepal.Areas.Admin.Controllers
{
    [Authorize]
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class BannerController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        //
        // GET: /Admin/Banner/
        
        public ActionResult Index()
        {
            //if (Session["AdminUser"] == null)
            //    return RedirectToAction("Index", "Web_admin", new { area = "" });
            return View(db.Banners.ToList());
        }

        //
        // GET: /Admin/Banner/Details/5

        public ActionResult Details(int id = 0)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        //
        // GET: /Admin/Banner/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Banner/Create

        [HttpPost]
        public ActionResult Create(Banner banner, HttpPostedFileBase fileArticle)
        {
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {


                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/Banner"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    banner.BannerImage = fileName;

                }
                db.Banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        //
        // GET: /Admin/Banner/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        //
        // POST: /Admin/Banner/Edit/5

        [HttpPost]
        public ActionResult Edit(Banner banner,HttpPostedFileBase fileArticle)
        {
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {


                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/Banner"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    banner.BannerImage = fileName;

                }
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        //
        // GET: /Admin/Banner/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        //
        // POST: /Admin/Banner/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
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