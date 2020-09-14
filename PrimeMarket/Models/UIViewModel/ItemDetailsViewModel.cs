using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;

namespace PrimeMarket.Models.UIViewModel
{
    public class ItemDetailsViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public Commodity ThisCommodity;
        public List<Commodity> RelatedItems = new List<Commodity>();
        public List<CommodityRating> ThisCommodityRatings = new List<CommodityRating>();
        public List<CommodityRating> ThisCommodityReviews = new List<CommodityRating>();
        double Rank = 0;
        public ItemDetailsViewModel(int CommodityId)
        {
            ThisCommodity = db.Commodities.Include(c => c.AspNetUser).Include(c => c.CommodityImages).Include(c => c.CommodityRatings).Include(c => c.PriceUnit).Include(c => c.SubCategory).Where(c => c.CommodityId == CommodityId).FirstOrDefault();
            RelatedItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.SubCategoryId==ThisCommodity.SubCategoryId).OrderByDescending(u => u.CreationDate).Take(4).ToList();
            ThisCommodityRatings = ThisCommodity.CommodityRatings.Where(r => r.Rating != null).ToList();
            ThisCommodityReviews = ThisCommodity.CommodityRatings.Where(r => r.Comment != null).OrderByDescending(r=>r.CreationDate).ToList();
            Rank = (double)ThisCommodityRatings.Sum(r => r.Rating) / (double)ThisCommodityRatings.Count;
        }
    }
}