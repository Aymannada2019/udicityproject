using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrimeMarket.Models;

namespace PrimeMarket.Controllers
{
    public class ShowCommoditiesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: ShowCommodities
        public ActionResult Index()
        {
            var showCommodities = db.Commodities.Include(s => s.SubCategory);
            return View(showCommodities.ToList());
        }

        // GET: ShowCommodities/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity showCommodity = db.Commodities.Find(id);
            if (showCommodity == null)
            {
                return HttpNotFound();
            }
            return View(showCommodity);
        }

        // GET: ShowCommodities/Create
        public ActionResult Create()
        {
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1");
            return View();
        }

        // POST: ShowCommodities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShowCommodityId,Title,Details,SubCategoryId,SellerId,Price,PriceNote,ImagePath,Available,CreationDate,Rating,ExpireDate")] Commodity showCommodity)
        {
            if (ModelState.IsValid)
            {
                db.Commodities.Add(showCommodity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", showCommodity.SubCategoryId);
            return View(showCommodity);
        }

        // GET: ShowCommodities/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity showCommodity = db.Commodities.Find(id);
            if (showCommodity == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", showCommodity.SubCategoryId);
            return View(showCommodity);
        }

        // POST: ShowCommodities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowCommodityId,Title,Details,SubCategoryId,SellerId,Price,PriceNote,ImagePath,Available,CreationDate,Rating,ExpireDate")] Commodity showCommodity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(showCommodity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", showCommodity.SubCategoryId);
            return View(showCommodity);
        }

        // GET: ShowCommodities/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity showCommodity = db.Commodities.Find(id);
            if (showCommodity == null)
            {
                return HttpNotFound();
            }
            return View(showCommodity);
        }

        // POST: ShowCommodities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Commodity showCommodity = db.Commodities.Find(id);
            db.Commodities.Remove(showCommodity);
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
