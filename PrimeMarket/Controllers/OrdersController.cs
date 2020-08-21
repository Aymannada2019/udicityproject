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
    public class OrdersController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Account).Include(o => o.Account1).Include(o => o.OrderStatu).Include(o => o.ShowCommodity);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.Accounts, "AccountId", "Full_name");
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name");
            ViewBag.OrderStatusId = new SelectList(db.OrderStatus, "OrderStatusId", "OrderStatus");
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderStatusId,CustomerId,SellerId,ShowCommodityId,Quantity,RequestDate,DeliveryDate,CancelDate,Notes,SellerRating,CustomerRating")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SellerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.SellerId);
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.CustomerId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatus, "OrderStatusId", "OrderStatus", order.OrderStatusId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", order.ShowCommodityId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.SellerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.SellerId);
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.CustomerId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatus, "OrderStatusId", "OrderStatus", order.OrderStatusId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", order.ShowCommodityId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderStatusId,CustomerId,SellerId,ShowCommodityId,Quantity,RequestDate,DeliveryDate,CancelDate,Notes,SellerRating,CustomerRating")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SellerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.SellerId);
            ViewBag.CustomerId = new SelectList(db.Accounts, "AccountId", "Full_name", order.CustomerId);
            ViewBag.OrderStatusId = new SelectList(db.OrderStatus, "OrderStatusId", "OrderStatus", order.OrderStatusId);
            ViewBag.ShowCommodityId = new SelectList(db.ShowCommodities, "ShowCommodityId", "Title", order.ShowCommodityId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
