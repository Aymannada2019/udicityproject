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
    public class GovernoratesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Governorates
        public ActionResult Index(string search, int? page)
        {
            // return View(this.GetGovernorates(1));
            if (search != null && search !="") 
            return View(db.Governorates.Where(x => x.Governorate1.StartsWith(search) || search==null). ToList().ToPagedList(page ?? 1, 5));
            else
                return View(db.Governorates.ToList().ToPagedList(page ?? 1, 5));
        }
       // [HttpPost]
        //public ActionResult Index(int currentPageIndex)
        //{
        //    return View(this.GetGovernorates(currentPageIndex));
        //}
        //private GovernorateModel GetGovernorates(int currentPage)
        //{
        //    int maxRows = 2;
        //    using (PrimeMarketEntities entities = new PrimeMarketEntities())
        //    {
        //        GovernorateModel GovModel = new GovernorateModel();

        //        GovModel.Governorates = (from governorates in entities.Governorates
        //                                   select governorates)
        //                    .OrderBy(governorates => governorates.GovernorateId)
        //                    .Skip((currentPage - 1) * maxRows)
        //                    .Take(maxRows).ToList();

        //        double pageCount = (double)((decimal)entities.Governorates.Count() / Convert.ToDecimal(maxRows));
        //        GovModel.PageCount = (int)Math.Ceiling(pageCount);

        //        GovModel.CurrentPageIndex = currentPage;

        //        return GovModel;
        //    }
        //}
        // GET: Governorates/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Governorate governorate = db.Governorates.Find(id);
            if (governorate == null)
            {
                return HttpNotFound();
            }
            return View(governorate);
        }

        // GET: Governorates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Governorates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GovernorateId,Governorate1")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                
                // get last record
                int max = db.Governorates.Max(p => p.GovernorateId);
 // add 1 to get new record id                         
                governorate.GovernorateId = (short)(max + 1);
db.Governorates.Add(governorate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(governorate);
        }

        // GET: Governorates/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Governorate governorate = db.Governorates.Find(id);
            if (governorate == null)
            {
                return HttpNotFound();
            }
            return View(governorate);
        }

        // POST: Governorates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GovernorateId,Governorate1")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(governorate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(governorate);
        }

        // GET: Governorates/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Governorate governorate = db.Governorates.Find(id);
            if (governorate == null)
            {
                return HttpNotFound();
            }
            return View(governorate);
        }

        // POST: Governorates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Governorate governorate = db.Governorates.Find(id);
            db.Governorates.Remove(governorate);
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
