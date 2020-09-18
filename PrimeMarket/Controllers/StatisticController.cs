using PrimeMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimeMarket.Controllers
{
    public class StatisticController : Controller
    {
     //   private PrimeMarketEntities db = new PrimeMarketEntities();
        // GET: Statistic
        [HttpGet]
        public ActionResult Index(decimal? id)
        {
            var db = new StatisticsModel();
          //  double CommodityQCount = new double();
            //List<Order> PurchasItems = new List<Order>();
         //List<Commodity> LatestItems = new List<Commodity>();
        // List<Commodity> Itemsr = new List<Commodity>();

         //List<Commodity> CommodityQ = new List<Commodity>();
   //           int CommodityCount1 = new int();
   //         int PurchaseCount = new int();
           
   //var  Items1 = (from u in db.ItemsResults
   //                             where (u.Available == true )
   //                             select u).OrderByDescending(u => u.CreationDate).FirstOrDefault(); 
   //       var  Itemslist = (from u in db.ItemsResults
   //                         where u.Available == true
   //                select u).OrderByDescending(u => u.CreationDate).ToList();
          //  PurchasItems = db.Orders.Include(u => u.AspNetUser).Include(u => u.PaymentMethod).Include(u => u.OrderStatu).Include(u => u.OrderItems).Where(u => u.isPaid == true).OrderByDescending(u => u.RequestDate).ToList();
           // LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).OrderByDescending(u => u.CreationDate).Take(9).ToList();
          //  CommodityQ = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).ToList();
           // CommodityCount1 = Itemslist.ToList().Count;
            //for (int i = 0; i < CommodityQ.Count; i++)
            //    CommodityQCount = CommodityQCount + CommodityQ[i].Quantity.Value;
            //PurchaseCount = PurchasItems.Count;
            var model = new StatisticsModel {  };
            return View(model);
        }

        // GET: Statistic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Statistic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statistic/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Statistic/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Statistic/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
