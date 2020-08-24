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
    public class VillagesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Villages
        public ActionResult Index()
        {
            var villages = db.Villages.Include(v => v.District);
            return View(villages.ToList());
        }

        // GET: Villages/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // GET: Villages/Create
        public ActionResult Create()
        {
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1");
            return View();
        }

        // POST: Villages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VillageId,Village1,DistrictId")] Village village)
        {
            if (ModelState.IsValid)
            {
                db.Villages.Add(village);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1", village.DistrictId);
            return View(village);
        }

        // GET: Villages/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1", village.DistrictId);
            return View(village);
        }

        // POST: Villages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VillageId,Village1,DistrictId")] Village village)
        {
            if (ModelState.IsValid)
            {
                db.Entry(village).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1", village.DistrictId);
            return View(village);
        }

        // GET: Villages/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Villages.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Village village = db.Villages.Find(id);
            db.Villages.Remove(village);
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
