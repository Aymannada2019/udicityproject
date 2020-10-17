using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PrimeMarket.Models.UIViewModel
{
    public class UserOrdersViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public List<OrderItem> ClientsOrderItems = new List<OrderItem>();
        public List<Order> ClientsOrders = new List<Order>();
        public List<Order> ClientsOrders2 = new List<Order>();
        public UserOrdersViewModel(string UserId)
        {
            ClientsOrderItems = db.OrderItems.Include(x => x.Order).Where(x => x.Commodity.SellerId == UserId).ToList();
            ClientsOrders = db.Orders.Include(x => x.OrderItems).Where(x => x.OrderItems.Where(y=>y.Commodity.SellerId== UserId).Any()).ToList();
            ClientsOrders2 = (from o in ClientsOrderItems
                              where o.Commodity.SellerId == UserId
                              select o.Order).Distinct().ToList();
        }
    }
}