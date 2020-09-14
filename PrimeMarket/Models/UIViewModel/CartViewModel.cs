using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PrimeMarket.Models;

namespace PrimeMarket.Models.UIViewModel
{
    public class CartViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<Cart> cartItems = new List<Cart>();
        public CartViewModel()
        {
            var UserId = "0a9d78ba-edfc-4cd3-b100-7dde0c987024"; // logged in user
            cartItems = db.Carts.Include(c => c.AspNetUser).Include(c => c.Commodity).Where(c => c.CustomerId == UserId).ToList();
        }
    }
}