using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PrimeMarket.Models.UIViewModel
{
    public class ProductDetailsViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public Commodity ThisCommodity;
        public List<CommodityRating> ThisCommodityRatings = new List<CommodityRating>();
        public List<CommodityRating> ThisCommodityReviews = new List<CommodityRating>();
        double Rank = 0;
        public ProductDetailsViewModel(int CommodityId)
        {
            ThisCommodity = db.Commodities.Include(c => c.AspNetUser).Include(c => c.CommodityImages).Include(c => c.CommodityRatings).Include(c => c.PriceUnit).Include(c => c.SubCategory).Where(c => c.CommodityId == CommodityId).FirstOrDefault();
            ThisCommodityRatings = ThisCommodity.CommodityRatings.Where(r => r.Rating != null).ToList();
            ThisCommodityReviews = ThisCommodity.CommodityRatings.Where(r => r.Comment != null).OrderByDescending(r => r.CreationDate).ToList();
            Rank = (double)ThisCommodityRatings.Sum(r => r.Rating) / (double)ThisCommodityRatings.Count;
        }
    }
}