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
    public class ShopController : Controller
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        // GET: Shop
        public ActionResult Index(int? crntPage, int? pageSize)
        {
            crntPage = (crntPage ?? 1);
            pageSize = (pageSize ?? 9);
            ViewBag.crntPage = crntPage;
            ViewBag.pageSize = pageSize;
            var model = new ShopViewModel(crntPage,pageSize);
            return View(model);
        }
        public ActionResult Indx(int? id,int? subCatId,int? minPrice,int? maxPrice,int? sortField, int? crntPage, int? pageSize)
        {
            minPrice = (minPrice ?? 1);
            maxPrice = (maxPrice ?? 999999);
            sortField = (sortField ?? 1);
            crntPage = (crntPage ?? 1);
            pageSize = (pageSize ?? 9);

            ViewBag.catId = id;
            ViewBag.subCatId = subCatId;
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.sortField = sortField;
            ViewBag.crntPage = crntPage;
            ViewBag.pageSize = pageSize;

            var model = new ShopViewModel(id, subCatId,minPrice,maxPrice,sortField,crntPage,pageSize);
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
            string CustomerId = User.Identity.GetUserId();
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
        public JsonResult RateProduct(decimal id,float value)
        {
            var result = new JsonResult()
            {
                Data = new
                {
                    success = false,
                    message = "There was an error processing your request."
                }
            };
            var success = 0;
            // save rate
            if(!User.Identity.IsAuthenticated)
            {
                result.Data = new
                {
                    success = false,
                    message = "يجب تسجيل الدخول أولاً حتى تتمكن من تقييم هذا المنتج"
                };
                return result;
            }
            var reviewerId = User.Identity.GetUserId();
            var commodityRating = new CommodityRating();
            commodityRating.ReviewerId = reviewerId;
            commodityRating.Rating = value;
            commodityRating.CreationDate = DateTime.Now;
            commodityRating.CommodityId = id;
            db.CommodityRatings.Add(commodityRating);
            db.SaveChanges();

            // update commodity rating
            var total = db.CommodityRatings.Where(c => c.CommodityId == id && c.Rating > 0).Average(c=>c.Rating);
            float avgRating = (float)Math.Round((double)total,1);
            var commidity = db.Commodities.Where(c => c.CommodityId == id).FirstOrDefault();
            if(commidity!=null)
            {
                commidity.Rating = avgRating;
                success = db.SaveChanges();
            }
            
            if (success > 0)
            {
                result.Data = new
                {
                    success = true,
                    message = "Rating saved successfully"
                };
            }
            return result;
        }

        [HttpPost]
        public JsonResult RateUser(string id, float value)
        {
            var result = new JsonResult()
            {
                Data = new
                {
                    success = false,
                    message = "There was an error processing your request."
                }
            };
            var success = 0;
            // save rate
            if (!User.Identity.IsAuthenticated)
            {
                result.Data = new
                {
                    success = false,
                    message = "يجب تسجيل الدخول أولاً حتى تتمكن من تقييم هذا البائع"
                };
                return result;
            }
            var reviewerId = User.Identity.GetUserId();
            var userRating = new UserRating();
            userRating.ReviewerId = reviewerId;
            userRating.Rating = value;
            userRating.CreationDate = DateTime.Now;
            userRating.UserId = id;
            db.UserRatings.Add(userRating);
            db.SaveChanges();

            // update commodity rating
            var total = db.UserRatings.Where(c => c.UserId == id && c.Rating > 0).Average(c => c.Rating);
            float avgRating = (float)Math.Round((double)total, 1);
            var user = db.AspNetUsers.Where(c => c.Id == id).FirstOrDefault();
            if (user != null)
            {
                user.Rating = avgRating;
                db.Configuration.ValidateOnSaveEnabled = false;
                success = db.SaveChanges();
            }

            if (success > 0)
            {
                result.Data = new
                {
                    success = true,
                    message = "Rating saved successfully"
                };
            }
            return result;
        }

        [HttpPost]
        public ActionResult AddToCart()
        {
            try
            {
                var CommodityId = int.Parse(Request.Form["hdn_CommodityId"].ToString());
                var commidity = db.Commodities.Where(c => c.CommodityId == CommodityId).FirstOrDefault();
                var Quantity = int.Parse(Request.Form["txt_Quantity"].ToString().Trim());
                string CustomerId = User.Identity.GetUserId();
                if(CustomerId == commidity.SellerId)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                if(string.IsNullOrEmpty(CustomerId))
                {
                    return RedirectToAction("Login", "Account",new { ReturnUrl ="/shop/index"});
                }
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
            catch(Exception ex)
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

        public ActionResult UserItems(string UserId)
        {
            var model = new UserItemsViewModel(UserId);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUserComment()
        {
            var UserId = Request.Form["hdn_UserId"].ToString();
            var comment = Request.Form["txt_comment"].ToString();
            string CustomerId = User.Identity.GetUserId();
            
            UserRating userRate = new UserRating()
            {
                UserId = UserId,
                Comment = comment,
                CreationDate = DateTime.Now,
                Rating = 4,
                ReviewerId = CustomerId
            };
            db.UserRatings.Add(userRate);
            db.SaveChanges();

            return RedirectToAction("UserItems", "Shop", new { UserId = UserId });
        }
        


    }
}