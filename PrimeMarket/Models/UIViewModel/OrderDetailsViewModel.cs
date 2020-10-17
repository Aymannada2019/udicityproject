using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PrimeMarket.Models.UIViewModel
{
    public class OrderDetailsViewModel
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public Order ThisOrder;
        public OrderDetailsViewModel(decimal OrderId)
        {
            ThisOrder = db.Orders.Include(x => x.AspNetUser).Include(x => x.OrderItems).Include(x => x.OrderStatu).Include(x => x.PaymentMethod).Where(x => x.OrderId == OrderId).FirstOrDefault();
        }
    }
}