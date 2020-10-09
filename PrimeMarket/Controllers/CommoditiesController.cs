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
    public class CommoditiesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Commodities1
        public ActionResult Index(string search, int? page)
        {
            if (search != null && search != "")
            {
                var commodity = db.Commodities.Include(c => c.AspNetUser).Include(c => c.SubCategory).Include(c=>c.PriceUnit);
                return View(commodity.Where(x => x.Title.StartsWith(search) || search == null).ToList().ToPagedList(page ?? 1, 20));
            }
            else
            {
                var commodity1 = db.Commodities.Include(c => c.AspNetUser).Include(c => c.SubCategory).Include(c => c.PriceUnit);
                return View(commodity1.ToList().ToPagedList(page ?? 1, 20));
            }
            //var commodity = db.Commodities.Include(c => c.AspNetUsers).Include(c => c.SubCategory);
            //return View(commodity.ToList());
        }

        // GET: Commodities1/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
        }
        public JsonResult getSubCategoryList(decimal CategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubCategory> SubCategoryList = db.SubCategories.Where(subcategory => subcategory.CategoryId == CategoryId).ToList();
            return Json(SubCategoryList, JsonRequestBehavior.AllowGet);
        }
        // GET: Commodities1/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1");
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1");
            ViewBag.CategoriesList = new SelectList(db.Categories, "CategoryId", "Category1", "1");
            return View();
        }

        // POST: Commodities1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommodityId,Title,Details,SubCategoryId,SellerId,Price,PriceUnitId,PriceNote,Available,Publish,CreationDate,ExpireDate,IsFeatured,OriginalPrice")] Commodity commodity)
        {//, IEnumerable<HttpPostedFileBase> files
            if (ModelState.IsValid)
            {

                if (commodity.CreationDate >= commodity.ExpireDate)
                {
                    ModelState.AddModelError("FinishDate", "Finish Date needs to be after the Start Date!");
                    return View(commodity);
                }
      
                db.Commodities.Add(commodity);
                db.SaveChanges();
                //var i = 1;
                //foreach (var file1 in files)
                //{
                   
                //    if (file1 != null)
                //    {
                //        var filetext = file1.FileName.Split('.');
                //        file1.SaveAs(HttpContext.Server.MapPath("~/img/commodities/")
                //                                           +i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1]);
                //        CommodityImages comimages = new CommodityImages();
                //        comimages.ImagePath = i+"Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1];
                //        comimages.CommodityId = commodity.CommodityId;
                //       db.CommodityImages.Add(comimages);
                //        db.SaveChanges();
                       
                //    }
                //    i=i + 1;
                //}
                 
                return RedirectToAction("Index");
            }

            ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email", commodity.SellerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", commodity.SubCategoryId);
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1", commodity.PriceUnitId);
            return View(commodity);
        }
     
        // GET: Commodities1/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email", commodity.SellerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1");
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1", commodity.PriceUnitId);
            ViewBag.CategoriesList = new SelectList(db.Categories, "CategoryId", "Category1", "1");
            return View(commodity);
        }

        // POST: Commodities1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommodityId,Title,Details,SubCategoryId,SellerId,Price,PriceUnitId,PriceNote,Available,Publish,CreationDate,ExpireDate,IsFeatured,OriginalPrice")] Commodity commodity)
        {//, IEnumerable<HttpPostedFileBase> files
            if (ModelState.IsValid)
            {
                if (commodity.CreationDate >= commodity.ExpireDate)
                {
                    ModelState.AddModelError("FinishDate", "Finish Date needs to be after the Start Date!");
                    return View(commodity);
                }
                db.Entry(commodity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email", commodity.SellerId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", commodity.SubCategoryId);
            //var i = 1;
            //foreach (var file1 in files)
            //{

            //    if (file1 != null)
            //    {
            //        var filetext = file1.FileName.Split('.');
            //        file1.SaveAs(HttpContext.Server.MapPath("~/img/commodities/")
            //                                           + i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1]);
            //        CommodityImages comimages = new CommodityImages();
            //        comimages.ImagePath = i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1];
            //        comimages.CommodityId = commodity.CommodityId;
            //       db.CommodityImages.Add(comimages);
            //        db.SaveChanges();

            //    }
            //    i = i + 1;
            //}
            return View(commodity);
        }
        public ActionResult DeleteImage(string search)
        {
            if (search == null && search == "")
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityImage commodityimages = new CommodityImage();
            commodityimages= db.CommodityImages.Find(search);
            if (commodityimages == null)
            { return HttpNotFound(); }
            else
               db.CommodityImages.Remove(commodityimages);
            db.SaveChanges();

           return HttpNotFound();
        }
        // GET: Commodities1/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
        }

        // POST: Commodities1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Commodity commodity = db.Commodities.Find(id);
            db.Commodities.Remove(commodity);
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
