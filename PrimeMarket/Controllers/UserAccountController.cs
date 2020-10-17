using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PrimeMarket.Controllers
{
    [Authorize]
    public class UserAccountController : Controller
    {
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
            var model = new UserOrdersViewModel(UserId);
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
    }
}