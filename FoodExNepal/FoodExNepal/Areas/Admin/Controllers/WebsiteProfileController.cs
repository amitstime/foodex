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
    public class WebsiteProfileController : Controller
    {
        private FoodExEntities db = new FoodExEntities();

        

        public ActionResult Edit(int id = 0)
        {
            WebsiteProfile websiteprofile = db.WebsiteProfiles.First();
            if (websiteprofile == null)
            {
                return HttpNotFound();
            }
            return View(websiteprofile);
        }

        //
        // POST: /Admin/WebsiteProfile/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(WebsiteProfile websiteprofile, HttpPostedFileBase fileArticle, HttpPostedFileBase fileArticle1)
        {
            //var a = db.WebsiteProfiles.First();
            if (ModelState.IsValid)
            {
                if (fileArticle != null)
                {
                    string fileName = checkfile(fileArticle.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/homepageimage"), Path.GetFileName(fileName));
                    fileArticle.SaveAs(filePath);
                    websiteprofile.HomePageImage = fileName;

                }
                
                if (fileArticle1 != null)
                {
                    string fileName = checkfile(fileArticle1.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Images/homepageimage"), Path.GetFileName(fileName));
                    fileArticle1.SaveAs(filePath);
                    websiteprofile.Logo = fileName;
                 }
               
                db.Entry(websiteprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(websiteprofile);
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