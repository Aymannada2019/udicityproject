using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrimeMarket.Models;

namespace PrimeMarket.Areas.front_end.Controllers
{
    public class CartsController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: front_end/Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Account).Include(c => c.ShowCommodity);
            return View(carts.ToList());
        }

        // GET: front_end/Carts/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: front_end/Carts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name");
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title");
            return View();
        }

        // POST: front_end/Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartId,CustomerId,ShowCommodityId,CartDate,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", cart.CustomerId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", cart.ShowCommodityId);
            return View(cart);
        }

        // GET: front_end/Carts/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", cart.CustomerId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", cart.ShowCommodityId);
            return View(cart);
        }

        // POST: front_end/Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId,CustomerId,ShowCommodityId,CartDate,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", cart.CustomerId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", cart.ShowCommodityId);
            return View(cart);
        }

        // GET: front_end/Carts/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: front_end/Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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
