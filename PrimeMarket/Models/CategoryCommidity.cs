using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PrimeMarket.Models;
using System.ComponentModel.DataAnnotations;

namespace PrimeMarket.Models
{
    public class CategoryCommidity
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        [ScaffoldColumn(false)]
        public decimal CommidityId { get; set; }
        public List<Commodity> CategoryCommidities { get; set; }
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategory { get; set; }
        public Commodity Commodity { get; set; }
        public Category Category { get; set; }
        public decimal SubCategoryId { get; set; }
        public decimal CategoryId { get; set; }



        public CategoryCommidity()
        {
            try
            {
                CategoryCommidities = db.Commodities.Include(u => u.AspNetUser).Include(u =>u.SubCategory ).Include(u=>db.Categories).OrderByDescending(u => u.CreationDate).ToList();

            }
            catch { }
        }
    }
    //public class Category
    //{

    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //}
    //public class SubCategory
    //{
    //    public string ID { get; set; }
    //    public string Name { get; set; }
    //    public string CategoryID { get; set; }
    //}

}
  