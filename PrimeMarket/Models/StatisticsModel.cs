using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PrimeMarket.Models;
namespace PrimeMarket.Models
{

    public class StatisticsModel
    {
       public PrimeMarketEntities db = new PrimeMarketEntities();
        public Order PurchasItems { get; set; }
        public List<Order> PurchasItemsResults { get; set; }
        public decimal CommodityId { get; set; }
        public Commodity Items { get; set; }
        public List<Commodity> ItemsResults { get; set; }
        public List<Commodity> CommodityQ { get; set; }
        public List<Commodity> LatestItems { get; set; }
        public int CommodityCount { get; set; }
        public double CommodityQCount { get; set; }
        public int PurchaseCount { get; set; }


        public StatisticsModel()
        {
            try
            {
                ItemsResults = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).OrderByDescending(u => u.CreationDate).ToList();
                PurchasItemsResults = db.Orders.Include(u => u.AspNetUser).Include(u => u.PaymentMethod).Include(u => u.OrderStatu).Include(u => u.OrderItems).Where(u => u.isPaid == true).OrderByDescending(u => u.RequestDate).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CommodityQ = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).ToList();
                CommodityCount = ItemsResults.ToList().Count;
             //   for (int i = 0; i < CommodityQ.Count; i++)
             //       CommodityQCount = CommodityCount + CommodityQ[i].Quantity.Value;
                PurchaseCount = PurchasItemsResults.Count;
            }
            catch { }
        }
    }
}