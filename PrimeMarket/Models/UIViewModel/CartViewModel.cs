using PrimeMarket.Models;
using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
namespace PrimeMarket.Models.UIViewModel
{
    public class CartViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<Cart> cartItems = new List<Cart>();
        public CartViewModel()
        {
            var CustomerId = System.Web.HttpContext.Current.User.Identity.GetUserId(); // logged in user
            if (string.IsNullOrEmpty(CustomerId))
            {
                //return RedirectToAction("Login", "Account",new { ReturnUrl ="/shop/index"});
                var tempCustomerId = Helper.GetCookie("tempCustomerId");
                if (tempCustomerId == null)
                {
                    tempCustomerId = Guid.NewGuid().ToString();
                    Helper.SetCookie("tempCustomerId", tempCustomerId, DateTime.Now.AddDays(20));
                }
                CustomerId = tempCustomerId;
            }
            cartItems = db.Carts.Include(c => c.AspNetUser).Include(c => c.Commodity).Where(c => c.CustomerId == CustomerId).ToList();
        }
    }
}