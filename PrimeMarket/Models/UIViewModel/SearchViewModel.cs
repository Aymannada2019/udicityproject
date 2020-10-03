using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;

namespace PrimeMarket.Models.UIViewModel
{
    public class SearchViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        // public List<Commodity> SaleOffItems = new List<Commodity>();
        public List<Commodity> LatestItems = new List<Commodity>();
        public List<Commodity> Items = new List<Commodity>();
        public List<Category> CategoryList = new List<Category>();
        public SearchViewModel(string term)
        {
            try
            {
                Items = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Include(u => u.PriceUnit).Where(u => u.Available == true && u.Publish == true && u.Title.ToLower().Contains(term)).OrderByDescending(u => u.CreationDate).ToList();
                // SaleOffItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.OriginalPrice > u.Price).OrderBy(u => u.CreationDate).Take(9).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
            }
            catch
            {

            }
        }
    }
}