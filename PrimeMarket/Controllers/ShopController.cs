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
            string CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
            CommodityRating comRate = new CommodityRating()
            {
                CommodityId = int.Parse(CommodityId),
                Comment = comment,
                CreationDate = DateTime.Now,
                Rating = 4,
                ReviewerId = CustomerId
            };
            db.CommodityRatings.Add(comRate);
            db.SaveChanges();

            return RedirectToAction("ItemDetails", "Shop", new { CommodityId = CommodityId });
        }

        [HttpPost]
        public ActionResult AddToCart()
        {
            try
            {
                var CommodityId = int.Parse(Request.Form["hdn_CommodityId"].ToString());
                var commidity = db.Commodities.Where(c => c.CommodityId == CommodityId).FirstOrDefault();
                var Quantity = int.Parse(Request.Form["txt_Quantity"].ToString().Trim());
                string CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
                var cart = db.Carts.Where(c => c.CommodityId == CommodityId && c.CustomerId == CustomerId).FirstOrDefault();
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        CommodityId = CommodityId,
                        Quantity = Quantity,
                        CustomerId = CustomerId,
                        CartDate = DateTime.Now,
                        CartStatusId = 1
                    };
                    db.Carts.Add(cart);
                }
                else
                {
                    cart.Quantity += Quantity;
                }
                if (cart.Quantity <= commidity.Quantity)
                {
                    db.SaveChanges();
                }
            }
            catch
            { }
            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        public ActionResult DeleteFromCart()
        {
            try
            {
                var CartId = int.Parse(Request.Form["hdn_CartId"].ToString());
                var cart = db.Carts.Where(c => c.CartId == CartId).FirstOrDefault();
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                }
                db.SaveChanges();
            }
            catch
            { }
            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        public ActionResult UpdateCart()
        {
            try
            {
                var CartId = int.Parse(Request.Form["hdn_CartId"].ToString());
                var Quantity = int.Parse(Request.Form["txt_Quantity"].ToString().Trim());
                var cart = db.Carts.Where(c => c.CartId == CartId).FirstOrDefault();
                var commidity = db.Commodities.Where(c => c.CommodityId == cart.CommodityId).FirstOrDefault();
                if (cart != null)
                {
                    cart.Quantity = Quantity;
                }
                if (cart.Quantity <= commidity.Quantity)
                {
                    db.SaveChanges();
                }
            }
            catch
            { }
            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        public JsonResult GetCommodities(string Prefix)
        {

            var commodities = (from c in db.Commodities
                             where c.Title.StartsWith(Prefix)
                             select new { c.Title}).Distinct();
            return Json(commodities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            if (Request["txt_search"] == null)
            {
                return RedirectToAction("Index", "Shop");
            }
            
            var term = Request["txt_search"].ToString().ToLower().Replace("#", "");
            var model = new SearchViewModel(term);
            return View(model);
        }
    }
}