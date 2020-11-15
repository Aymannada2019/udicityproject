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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace PrimeMarket.Controllers
{
    public class VillagesController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Villages
        public ActionResult Index(int? page)
        {
            //var District = db.Districts.Where(r => r.DistrictId == DistrictId).FirstOrDefault();
            // var villages = db.Villages.Where(rs => rs.DistrictId == DistrictId && rs.GovernoratetId == District.GovernorateId);
            var villages = db.Villages.Include(v => v.District).ToList();       
            ViewBag.GovernoratesList = new SelectList(db.Governorates, "GovernorateId", "Governorate1", "1");
            return View(villages.ToPagedList(page ?? 1, 20));
        }
        public ActionResult Villagesfilter(decimal DistrictId, int? page)
        {
            var District = db.Districts.Where(r => r.DistrictId == DistrictId).FirstOrDefault();
            var villages = db.Villages.Include(v => v.District).Where(rs => rs.DistrictId == DistrictId );
            if (villages == null)
            {
                return HttpNotFound();
            }
            ViewBag.GovernoratesList = new SelectList(db.Governorates, "GovernorateId", "Governorate1", "1");
            return View(villages.ToList().ToPagedList(page ?? 1, 20));
        }
        // GET: Villages/Details/5
        public ActionResult Details(decimal? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           //Village village = db.Villages.Include(x => x.District).Include(g => g.Governorate).Where(x => x.VillageId == id && x.GovernoratetId==x.Governorate.GovernorateId).FirstOrDefault();
            //var GovernorateId = village2.District.GovernorateId;
            //var DistrictId = village2.DistrictId;
            Village village = db.Villages.Find(id);
            //List<string> vv = new List<string>();
            //Village village2 = new Village();
            //foreach (District d in db.Districts)
            //{
            //    if (d.DistrictId == id)
            //    {
            //        var gid = d.GovernorateId;
            //        foreach (Governorate g in db.Governorates.Where(h => h.GovernorateId == gid))
            //        {
            //            village2.Village1 = village.Village1;
            //            village2.Governorate.Governorate1 = g.Governorate1;
            //            village2.District.District1 = d.District1;
            //            village2.VillageId = village.VillageId;
            //        }
            //    }
            //}

            if (village == null)
            {
                return HttpNotFound();
            }
            ViewBag.Details = village;
            return View(village);
        }

        // GET: Villages/Create
        public ActionResult Create(decimal? DistrictId)
           //  public ActionResult Create()
        {

            var dist =  db.Districts.Where(d => d.DistrictId == DistrictId).ToList();
           // var dist = db.Districts.ToList();
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1");
            // var villages = db.Villages.Where(rs => rs.DistrictId == DistrictId);
            List<SelectListItem> governorates = db.Governorates.AsNoTracking()

                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.GovernorateId.ToString(),
                            Text = n.Governorate1
                        }).ToList();
            var categorytip = new SelectListItem()
            {
                Value = "0",
                Text = "--- اختر المحافظة ---"
            };
            governorates.Insert(0, categorytip);
            ViewBag.GovernoratesList = new SelectList(governorates, "Value", "Text", "0");
           // ViewBag.GovernoratesList = new SelectList(db.Governorates,"GovernorateId","Governorate1", "0");
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
                if(village.DistrictId>0 && village.Village1 != "") { 
                Int64 max = (Int64)db.Villages.Max(p => p.VillageId)+1;
             
                village.VillageId = max;
                db.Villages.Add(village);
                db.SaveChanges();
                }
                //return RedirectToAction("Index");
                var village2 = db.Villages.Include(x=>x.District).Where(x=>x.VillageId==village.VillageId).FirstOrDefault();
                var GovernorateId = village2.District.GovernorateId;
                var DistrictId = village2.DistrictId;
                return RedirectToAction("Villagesfilter", "Villages", new { DistrictId = DistrictId, GovernorateId = GovernorateId });
            }
            
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "District1", village.DistrictId);
           // http://localhost:58090/Villages/Villagesfilter?DistrictId=206&GovernorateId=2
            return View(village);
            //return RedirectToAction("Villagesfilter", "Villages", new { DistrictId= ViewBag.DistrictId, GovernorateId = GovernorateId });
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
            var village2 = db.Villages.Include(x => x.District).Where(x => x.VillageId == village.VillageId).FirstOrDefault();
            var GovernorateId = village2.District.GovernorateId;
            var DistrictId = village2.DistrictId;
            List<SelectListItem> governorates = db.Governorates.AsNoTracking()

                       .Select(n =>
                       new SelectListItem
                       {
                           Value = n.GovernorateId.ToString(),
                           Text = n.Governorate1
                       }).ToList();
            var categorytip = new SelectListItem()
            {
                Value = "0",
                Text = "--- اختر المحافظة ---"
            };
            governorates.Insert(0, categorytip);
            ViewBag.Govs = new SelectList(governorates, "Value", "Text", GovernorateId);
          //  ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", GovernorateId);
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

        public JsonResult getDistrictList(int GovernorateId)
        {
            
            db.Configuration.ProxyCreationEnabled = false;
            List<District> DistrictList = db.Districts.Where(district => district.GovernorateId == GovernorateId).ToList();
            return Json(DistrictList,JsonRequestBehavior.AllowGet);
        }
        public JsonResult getGovName(int DistrictId)
        {

            db.Configuration.ProxyCreationEnabled = false;
            var village2 = db.Villages.Include(x => x.District).Where(x => x.DistrictId == DistrictId).FirstOrDefault();
            var GovernorateId = village2.District.GovernorateId;
            var GovName = db.Governorates.Where(g => g.GovernorateId == GovernorateId).FirstOrDefault();
            return Json(GovName.Governorate1.ToString());
        }
    }
}
