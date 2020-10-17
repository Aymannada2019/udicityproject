using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PrimeMarket.Models;

namespace PrimeMarket.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        public ActionResult UserProfile(string UserId)
        {
            if (User.Identity.GetUserId() != UserId)
                return RedirectToAction("index", "Shop");

            return View();
        }
        public ActionResult UserOrders(string UserId)
        {
            if (User.Identity.GetUserId() != UserId)
                return RedirectToAction("index", "Shop");
            var model = new UserOrdersViewModel(UserId);
            return View(model);
        }
        public ActionResult MyOrders(string UserId)
        {
            if (User.Identity.GetUserId() != UserId)
                return RedirectToAction("index", "Shop");
            var model = new MyOrdersViewModel(UserId);
            return View(model);
        }
        public ActionResult UserProducts(string UserId)
        {
            if (User.Identity.GetUserId() != UserId)
                return RedirectToAction("index", "Shop");
            var model = new UserProductsViewModel(UserId);
            return View(model);
        }
        public ActionResult ProductDetails(int CommodityId)
        {
            var model = new ProductDetailsViewModel(CommodityId);
            if(model.ThisCommodity.SellerId != User.Identity.GetUserId())
                return RedirectToAction("index", "Shop");
            return View(model);
        }
        
        // 1	تم عمل الطلب من المشتري
        // 2	تم استلام الطلب من البائع
        // 3	جاري تنفيذ الطلب
        // 4	تم إلغاء الطلب من المشتري
        // 5	تم إلغاء الطلب من البائع
        // 6	تم تنفيذ الطلب

        [HttpPost]
        public ActionResult CancelOrder()
        {
            var OrderId = decimal.Parse(Request.Form["OrderId"].ToString());
            var CustomerId = Request.Form["CustomerId"].ToString();
            var order = db.Orders.Where(x => x.OrderId == OrderId).FirstOrDefault();
            if(order!=null)
            {
                if(order.CustomerId!= User.Identity.GetUserId())
                    return RedirectToAction("Index", "Shop");
                if (order.OrderStatusId < 3)
                {
                    order.OrderStatusId = 4;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("MyOrders", "UserAccount", new { UserId = CustomerId });
        }

        public ActionResult OrderDetails(decimal OrderId)
        {
            var model = new OrderDetailsViewModel(OrderId);
            if(model.ThisOrder.CustomerId != User.Identity.GetUserId())
                return RedirectToAction("index", "Shop");
            return View(model);
        }

    }
}