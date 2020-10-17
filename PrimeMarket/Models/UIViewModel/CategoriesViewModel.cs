using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PrimeMarket.Models.UIViewModel
{
    public class CategoriesViewModel
    {
        public List<Category> CategoryList = new List<Category>();
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public CategoriesViewModel()
        {
            CategoryList = db.Categories.Include(c => c.SubCategories).ToList();
        }
    }
}