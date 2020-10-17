using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PrimeMarket.Models.UIViewModel
{
    public class MyOrdersViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<Order> MyOrders = new List<Order>();
        public MyOrdersViewModel(string UserId)
        {
            MyOrders = db.Orders.Include(x=>x.OrderItems).Where(x => x.CustomerId == UserId).ToList();
        }
    }
}