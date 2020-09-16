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
    public class DistrictsController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Districts
        public ActionResult Index(int? page)
        {
            var districts = db.Districts.Include(d => d.Governorate);
            return View(districts.ToList().ToPagedList(page ?? 1, 20));
        }
        public ActionResult Districtfilter(decimal GovernorateId, int? page)
        {

            var districts = db.Districts.Where(rs => rs.GovernorateId == GovernorateId);
            if (districts == null)
            {
                return HttpNotFound();
            }

            return View(districts.ToList().ToPagedList(page ?? 1, 10));
        }
        // GET: Districts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // GET: Districts/Create
        public ActionResult Create(decimal GovernorateId)
        {
            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", GovernorateId);
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DistrictId,District1,GovernorateId")] District district)
        {
            if (ModelState.IsValid)
            {// get last record
                int max = db.Districts.Max(p => p.DistrictId);
                // add 1 to get new record id                         
                district.DistrictId = (short)(max + 1);

                db.Districts.Add(district);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", district.GovernorateId);
            return View(district);
        }

        // GET: Districts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", district.GovernorateId);
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DistrictId,District1,GovernorateId")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", district.GovernorateId);
            return View(district);
        }

        // GET: Districts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
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
