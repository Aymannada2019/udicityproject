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
    public class VillagesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Villages
        public ActionResult Index(int? page)
        {
            var villages = db.Villages.Include(v => v.District);
           // var model = new VillageGov();
          //  return View(model);
            //var villages1 = (from d in db.Districts
            //                 join g in db.Governorates on d.GovernorateId equals g.GovernorateId
            //                 join v in db.Villages on d.DistrictId equals v.DistrictId
            //                 select new
            //                 {
            //                     Governorate1 = g.Governorate1,
            //                     GovernorateId = g.GovernorateId,
            //                     District1 = d.District1,
            //                     DistrictId = d.DistrictId,
            //                     Village1 = v.Village1,
            //                     VillageId = v.VillageId,
            //                     District= v.District
            //                 }).Include(v => v.District);

        //    var villages = villages1.Include(v => v.District);

        //}

            return View(villages.ToList().ToPagedList(page ?? 1, 20));
        }
        public ActionResult Villagesfilter(decimal DistrictId, int? page)
        {

            var villages = db.Villages.Where(rs => rs.DistrictId == DistrictId);
            if (villages == null)
            {
                return HttpNotFound();
            }

            return View(villages.ToList().ToPagedList(page ?? 1, 20));
        }
        // GET: Villages/Details/5
        public ActionResult Details(decimal? id)
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
        public ActionResult Create(decimal DistrictId)
        {

            var dist =  db.Districts.Where(d => d.DistrictId == DistrictId).ToList();
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1", DistrictId);
           // var villages = db.Villages.Where(rs => rs.DistrictId == DistrictId);
         
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
                // get last record
               // decimal max = db.Villages.Max(p => p.VillageId);
                // add 1 to get new record id                         
               // village.VillageId = (decimal)(max + 1);
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
