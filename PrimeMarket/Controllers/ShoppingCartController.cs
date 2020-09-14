using PrimeMarket.Models;
using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimeMarket.Controllers
{
    public class ShoppingCartController : Controller
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var model = new CartViewModel();
            return View(model);
        }

        public ActionResult Checkout()
        {
            var model = new CartViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult PlaceOrder()
        {
            var userOrder = new Order();
            try
            {
                // mahmoud Bakr
                var CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
                var cart = db.Carts.Where(c => c.CustomerId == CustomerId).ToList();
                double total = getCartTotal();
                
                
                userOrder.CustomerId = CustomerId;
                userOrder.isPaid = false;
                userOrder.OrderStatusId = 1;
                userOrder.PaymentMethodId = 1;
                userOrder.RequestDate = DateTime.Now;
                userOrder.Total = total;
                db.Orders.Add(userOrder);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    var orderItem = new OrderItem();
                    orderItem.CommodityId = item.Commodity.CommodityId;
                    orderItem.OrderId = userOrder.OrderId;
                    orderItem.PaymentMethodId = 1;
                    orderItem.Quantity = item.Quantity;
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
                ClearCart();
            }
            catch(Exception ex)
            { }
            ViewBag.OrderId = userOrder.OrderId;
            //return RedirectToAction("OrderDetails", "ShoppingCart");
            return RedirectToAction("Index", "Shop");
        }

        public static void ClearCart()
        {
            PrimeMarketEntities context = new PrimeMarketEntities();
            var CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
            try
            {
                var cart = context.Carts.RemoveRange(context.Carts.Where(c => c.CustomerId == CustomerId).ToList());
                context.SaveChanges();
            }
            catch
            { }
        }
        public static double getCartTotal()
        {
            double total = 0;
            PrimeMarketEntities context = new PrimeMarketEntities();
            var CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
            try
            {
                var cart = context.Carts.Where(c => c.CustomerId == CustomerId).ToList();
                foreach (var item in cart)
                {
                    total += (double)item.Commodity.Price * (double)item.Quantity;
                }
            }
            catch
            { }
            return total;
        }





    }
}