using PrimeMarket.Models;
using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PrimeMarket.Controllers
{
    public class ShopController : Controller
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        // GET: Shop
        public ActionResult Index()
        {
            var model = new ShopViewModel();
            return View(model);
        }
        public ActionResult Indx(int id)
        {
            var model = new ShopViewModel(id);
            return View("Index",model);
        }

        public ActionResult ItemDetails(int CommodityId)
        {
            var model = new ItemDetailsViewModel(CommodityId);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddComment()
        {
            var CommodityId = Request.Form["hdn_CommodityId"].ToString();
            var comment = Request.Form["txt_comment"].ToString();
            CommodityRating comRate = new CommodityRating()
            {
                CommodityId = int.Parse(CommodityId),
                Comment = comment,
                CreationDate = DateTime.Now,
                Rating = 4,
                ReviewerId = "0a9d78ba-edfc-4cd3-b100-7dde0c987024"
            };
            db.CommodityRatings.Add(comRate);
            db.SaveChanges();

            return RedirectToAction("ItemDetails", "Shop", new { CommodityId = CommodityId });
        }

        [HttpPost]
        public ActionResult AddToCart()
        {
            var CommodityId = Request.Form["hdn_CommodityId"].ToString();
            var Quantity = Request.Form["txt_quantity"].ToString();
            Cart cart = new Cart()
            {
                CommodityId = int.Parse(CommodityId),
                Quantity = int.Parse(Quantity),
                CustomerId = "0a9d78ba-edfc-4cd3-b100-7dde0c987024",
                CartDate=DateTime.Now,
                CartStatusId=1
            };
            db.Carts.Add(cart);
            db.SaveChanges();

            return RedirectToAction("Index", "ShoppingCart");
        }
        
    }
}