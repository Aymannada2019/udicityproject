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
    public class CommodityImagesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: CommodityImages
        public ActionResult Index(decimal CommodityId, int? page)
        {
            var commodityImages =db.CommodityImages.Where(rs => rs.CommodityId == CommodityId);
            if (commodityImages == null)
            {
                return HttpNotFound();
            }

            return View(commodityImages.ToList().ToPagedList(page ?? 1, 20));
            //var commodityImages =db.CommodityImages.Include(c => c.Commodity);
            //return View(commodityImages.ToList());
        }

        // GET: CommodityImages/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityImage commodityImages =db.CommodityImages.Find(id);
            if (commodityImages == null)
            {
                return HttpNotFound();
            }
            return View(commodityImages);
        }

        // GET: CommodityImages/Create
        public ActionResult Create()
        {
            decimal ComId = Int64.Parse(Request["CommodityId"].ToString());
            ViewBag.CommodityId = new SelectList(db.Commodities.Where(x=> x.CommodityId== ComId), "CommodityId", "Title", Request["CommodityId"].ToString());
            //ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "Title");
            return View();
        }

        // POST: CommodityImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommodityImageId,ImagePath,CommodityId")] CommodityImage commodityImages)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ImagePath"];
                if (file != null)
                {
                    var fileext = file.FileName.Split('.');
                    string imgpath = "/img/commodities/Commodity"+ DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];
                    file.SaveAs(HttpContext.Server.MapPath(imgpath));
                                                       // + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1]);
                    commodityImages.ImagePath = imgpath; //"Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];

                }
              // commodityImages.CommodityId =decimal.Parse(Request["CommodityId"].ToString());
                db.CommodityImages.Add(commodityImages);
                db.SaveChanges();
                //   Response.Redirect("/CommodityImages?CommodityId="+ commodityImages.CommodityId);
                //  Response.RedirectToRoute("commodityImages",new { CommodityId = commodityImages.CommodityId});
                return RedirectToRoute("Default", new { controller = "commodityImages", action = "Index", CommodityId = commodityImages.CommodityId  });
                //  return View(commodityImages);
            }

          //  ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "Title", commodityImages.CommodityId);
            return View(commodityImages);
        }

        // GET: CommodityImages/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityImage commodityImages =db.CommodityImages.Find(id);
            if (commodityImages == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "Title", commodityImages.CommodityId);
            return View(commodityImages);
        }

        // POST: CommodityImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommodityImageId,ImagePath,CommodityId")] CommodityImage commodityImages)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["ImagePath"];
                if (file != null)
                {
                    //delete old  image file
                    string oldimageFilePath = Server.MapPath(@commodityImages.ImagePath);
                    System.IO.File.Delete(oldimageFilePath);
                    //upload new file
                    var fileext = file.FileName.Split('.');
                    string imgpath = "/img/commodities/Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];

                    file.SaveAs(HttpContext.Server.MapPath(imgpath));
                    // + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1]);
                    commodityImages.ImagePath = imgpath;// "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + fileext[1];

                }
                db.Entry(commodityImages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "Title", commodityImages.CommodityId);
            return View(commodityImages);
        }

        // GET: CommodityImages/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityImage commodityImages =db.CommodityImages.Find(id);
            if (commodityImages == null)
            {
                return HttpNotFound();
            }
            return View(commodityImages);
        }

        // POST: CommodityImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            CommodityImage commodityImages =db.CommodityImages.Find(id);
           db.CommodityImages.Remove(commodityImages);
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
