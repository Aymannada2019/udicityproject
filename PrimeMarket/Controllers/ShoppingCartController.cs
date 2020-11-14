using PrimeMarket.Models;
using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
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
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/ShoppingCart/Checkout" });
            }
            var model = new CartViewModel();
            return View(model);
        }

        public ActionResult OrderDetails(int OrderId)
        {
            var model = db.Orders.Where(x => x.OrderId == OrderId).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            var userOrder = new Order();
            try
            {
                var ShippingAddress = Request.Form["txt_ShippingAddress"].ToString();
                var ShippingNotes = Request.Form["txt_ShippingNotes"].ToString();
                var Notes = Request.Form["txt_Notes"].ToString();
                var phone = Request.Form["txt_phone"].ToString();
                // mahmoud Bakr
                var CustomerId = User.Identity.GetUserId();
                var cart = db.Carts.Where(c => c.CustomerId == CustomerId).ToList();
                double total = getCartTotal();
                
                
                userOrder.CustomerId = CustomerId;
                userOrder.isPaid = false;
                userOrder.OrderStatusId = 1;
                userOrder.PaymentMethodId = 1;
                userOrder.RequestDate = DateTime.Now;
                userOrder.Total = total;
                userOrder.Notes = Notes;
                userOrder.ShippingAddress = ShippingAddress;
                userOrder.ShippingNotes = ShippingNotes;
                userOrder.Phone = phone;
                db.Orders.Add(userOrder);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    var orderItem = new OrderItem();
                    orderItem.CommodityId = item.Commodity.CommodityId;
                    orderItem.OrderId = userOrder.OrderId;
                    orderItem.PaymentMethodId = 1;
                    orderItem.Quantity = item.Quantity;
                    orderItem.UnitPrice = item.Commodity.Price;
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
                ClearCart();
            }
            catch(Exception ex)
            { }
            ViewBag.OrderId = userOrder.OrderId;
            return RedirectToAction("OrderDetails", "UserAccount",new { OrderId= userOrder.OrderId });
            //return RedirectToAction("Index", "Shop");
        }

        public static void ClearCart()
        {
            PrimeMarketEntities context = new PrimeMarketEntities();
            var CustomerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
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
            var CustomerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
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