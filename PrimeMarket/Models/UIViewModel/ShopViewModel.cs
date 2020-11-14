using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;
using PagedList;

namespace PrimeMarket.Models.UIViewModel
{

    public class ShopViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<Commodity> SaleOffItems = new List<Commodity>();
        public List<Commodity> LatestItems = new List<Commodity>();
        // public List<Commodity> Items = new List<Commodity>();
        public PagedList.IPagedList<Commodity> Items;
        public List<Category> CategoryList = new List<Category>();
        public Category SelectedCategory;
        public SubCategory SelectedSubCategory;
        public int MaxCommodityPrice = 1000;

        public ShopViewModel(int? crntPage, int? pageSize)
        {
            try
            {
                crntPage = (crntPage ?? 1);
                pageSize = (pageSize ?? 9);

                SaleOffItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.OriginalPrice>u.Price).OrderBy(u => u.CreationDate).Take(9).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
                MaxCommodityPrice = (int)db.Commodities.Max(c => c.Price);
                var results = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Include(u => u.PriceUnit).Where(u => u.Available == true && u.Publish == true).OrderByDescending(u => u.CreationDate);
                Items = results.ToPagedList((int)crntPage, (int)pageSize);

            }
            catch
            {

            }
        }
        public ShopViewModel(int? CategoryId, int? subCatId,int? minPrice, int? maxPrice, int? sortField, int? crntPage, int? pageSize, string searchText)
        {
            try
            {
                SaleOffItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.SubCategory.CategoryId == CategoryId && u.OriginalPrice > u.Price).OrderBy(u => u.CreationDate).Take(9).ToList();
                LatestItems = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.SubCategory.CategoryId == CategoryId).OrderByDescending(u => u.CreationDate).Take(9).ToList();
                CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
                MaxCommodityPrice = (int)db.Commodities.Max(c => c.Price);
                //------------------------------------------------------------
                minPrice = (minPrice ?? 1);
                maxPrice = (maxPrice ?? 999999);
                sortField = (sortField ?? 1);
                crntPage = (crntPage ?? 1);
                pageSize = (pageSize ?? 9);
                searchText = (searchText ?? "");
                //Items = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Include(u => u.PriceUnit).Where(u => u.Available == true && u.Publish == true && u.Title.ToLower().Contains(term)).OrderByDescending(u => u.CreationDate).ToList();
                var results = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.Price >= minPrice && u.Price <= maxPrice && u.Title.ToLower().Contains(searchText));
                if (CategoryId != null)
                {
                    SelectedCategory = db.Categories.Where(c => c.CategoryId == CategoryId).FirstOrDefault();
                    results = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.Price >= minPrice && u.Price <= maxPrice && u.SubCategory.CategoryId == CategoryId && u.Title.ToLower().Contains(searchText));
                }
                if(subCatId!=null)
                {
                    SelectedSubCategory = db.SubCategories.Where(s => s.SubCategoryId == subCatId).FirstOrDefault();
                    results = db.Commodities.Include(u => u.AspNetUser).Include(u => u.PriceUnit).Include(u => u.SubCategory).Include(u => u.CommodityImages).Include(u => u.CommodityRatings).Where(u => u.Available == true && u.Publish == true && u.Price >= minPrice && u.Price <= maxPrice && u.SubCategory.SubCategoryId == subCatId && u.Title.ToLower().Contains(searchText));
                }
                switch(sortField)
                {
                    case 1:
                        {
                            // وصل حديثاً
                            results = results.OrderByDescending(c => c.CreationDate);
                            break;
                        }
                    case 2:
                        {
                            // الأعلى تقييماً
                            results = results.OrderByDescending(c => c.Rating);
                            break;
                        }
                    case 3:
                        {
                            // السعر: من الأقل للأكثر
                            results = results.OrderBy(c => c.Price);
                            break;
                        }
                    case 4:
                        {
                            // السعر: من الأكثر للأقل
                            results = results.OrderByDescending(c => c.Price);
                            break;
                        }
                } // switch
                Items = results.ToPagedList((int)crntPage, (int)pageSize);

            }
            catch(Exception ex)
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