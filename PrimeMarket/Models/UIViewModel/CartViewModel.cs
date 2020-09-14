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
            var UserId = "8ac3f426-e76d-4ed8-94c1-835addf528bc"; // logged in user
            cartItems = db.Carts.Include(c => c.AspNetUser).Include(c => c.Commodity).Where(c => c.CustomerId == UserId).ToList();
        }
    }
}