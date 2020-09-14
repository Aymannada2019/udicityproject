using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;

namespace PrimeMarket.Models.UIViewModel
{

    public class ShopViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<Commodity> SaleOffItems = new List<Commodity>();
        public List<Commodity> LatestItems = new List<Commodity>();
        public List<Commodity> Items = new List<Commodity>();
        public List<Category> CategoryList = new List<Category>();

        public ShopViewModel()
        {
            try
            {
                Items = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u=>u.Available==true).OrderByDescending(u => u.CreationDate).ToList();
                SaleOffItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.OriginalPrice>u.Price).OrderBy(u => u.CreationDate).Take(9).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
            }
            catch
            {

            }
        }
        public ShopViewModel(int SubCategoryId)
        {
            try
            {
                Items = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.SubCategoryId== SubCategoryId).OrderByDescending(u => u.CreationDate).ToList();
                SaleOffItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.SubCategoryId == SubCategoryId && u.OriginalPrice > u.Price).OrderBy(u => u.CreationDate).Take(9).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.SubCategoryId == SubCategoryId).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
            }
            catch
            {

            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommodityVM
    {
        public decimal CommodityId { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public Nullable<decimal> SubCategoryId { get; set; }
        public Nullable<decimal> SubCategoryName { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public Nullable<double> Price { get; set; }

        public Nullable<byte> PriceUnitId { get; set; }
        public Nullable<byte> PriceUnitName { get; set; }

        public string PriceNote { get; set; }

        public bool Available { get; set; }

        public Nullable<bool> Publish { get; set; }

        public Nullable<System.DateTime> CreationDate { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }

        public Nullable<bool> IsFeatured { get; set; }

        public Nullable<double> OriginalPrice { get; set; }
        public string MainImageURL { get; set; }
    }
}