using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrimeMarket.Models;
using PagedList;
using PagedList.Mvc;
namespace PrimeMarket.Controllers
{
    public class SubCategoriesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: SubCategories
        public ActionResult Index(int? page)
        {
            var subCategories =db.SubCategories.Include(s => s.Category);
            return View(subCategories.ToList().ToPagedList(page ?? 1, 20));
        }
        public ActionResult catfilter(decimal CategoryId, int? page)
        {
           
            var subCategories =db.SubCategories.Where(rs => rs.CategoryId == CategoryId);
            if (subCategories == null)
            {
                return HttpNotFound();
            }

            return View(subCategories.ToList().ToPagedList(page ?? 1, 20));
        }
        // GET: SubCategories/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory =db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryId,CategoryId,SubCategory1,ImagePath")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ImagePath"];
                if (file != null)
                {
                    var fileext = file.FileName.Split('.');
                    file.SaveAs(HttpContext.Server.MapPath("~/img/categories/")
                                                        + "SubCat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1]);
                    subCategory.ImagePath = "SubCat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];
                  
                }
               db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory =db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryId,CategoryId,SubCategory1,ImagePath")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                HttpPostedFileBase file = Request.Files["ImagePath"];
                if (file != null)
                {
                    //delete old  image file
                    string oldimageFilePath = Server.MapPath(@"~/img/categories/" + subCategory.ImagePath);
                    System.IO.File.Delete(oldimageFilePath);
                    //upload new file
                    var fileext = file.FileName.Split('.');
                    file.SaveAs(HttpContext.Server.MapPath("~/img/categories/")
                                                        + "catid" + subCategory.CategoryId + "SubCat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1]);
                    subCategory.ImagePath = "catid" + subCategory.CategoryId + "SubCat" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];
                    
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory =db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            SubCategory subCategory =db.SubCategories.Find(id);
            //delete old  image file
            string oldimageFilePath = Server.MapPath(@"~/img/categories/" + subCategory.ImagePath);
            System.IO.File.Delete(oldimageFilePath);
           db.SubCategories.Remove(subCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
