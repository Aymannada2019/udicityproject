using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;

namespace PrimeMarket.Models.UIViewModel
{
    public class CartSummaryViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public int CartItemsCount = 0;
        public double CartTotal = 0;
        public CartSummaryViewModel()
        {
            CartTotal = Helper.getCartTotal(out CartItemsCount);
        }
    }
}