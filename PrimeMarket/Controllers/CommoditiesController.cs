using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrimeMarket.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;


namespace PrimeMarket.Controllers
{[Authorize]
    public class CommoditiesController : Controller
    {
       private PrimeMarketEntities db = new PrimeMarketEntities();
        /// <summary>
        /// For checking the user is logged in and he is an admin. 
        /// </summary>
        /// <returns></returns>

        public decimal GetParentGategory(decimal subCategoryId)
        {

            decimal CategoryId = 0;
            var subCategory = db.SubCategories.Where(x => x.SubCategoryId == subCategoryId);
            foreach (var item in subCategory)
            {
                CategoryId = (decimal)item.CategoryId;


            }
                return CategoryId;
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
               
                if (s.Count>0 && s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // GET: Commodities1
        public ActionResult Index(string search, int? page)
        {
            
            string UserId= User.Identity.GetUserId();
            var commodity = db.Commodities.Include(c => c.AspNetUser).Include(c => c.SubCategory).Include(c => c.PriceUnit);
            if (!isAdminUser())
                commodity = commodity.Where(x => x.SellerId== UserId);
           //ayman 23/11/2020 ToString test error 
            //string msg = null;
            //ViewBag.Message = msg.Length;
            if (search != null && search != "")
            {
                return View(commodity.Where(x => x.Title.StartsWith(search) || search == null).OrderByDescending(x=>x.CommodityId).ToList().ToPagedList(page ?? 1, 20));
            }
            else
            {

                return View(commodity.OrderByDescending(x => x.CommodityId).ToList().ToPagedList(page ?? 1, 20));
            }
            //var commodity = db.Commodities.Include(c => c.AspNetUsers).Include(c => c.SubCategory);
            //return View(commodity.ToList());
        }

        // GET: Commodities1/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
        }
        public JsonResult getSubCategoryList(decimal CategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SubCategory> SubCategoryList = db.SubCategories.Where(subcategory => subcategory.CategoryId == CategoryId).ToList();
            return Json(SubCategoryList, JsonRequestBehavior.AllowGet);
        }
        // GET: Commodities1/Create
        public ActionResult Create()
        {
            ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1");
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1");
            //.OrderBy(n => n.CategoryId)
            List<SelectListItem> categories = db.Categories.AsNoTracking()
                    
                         .Select(n =>
                         new SelectListItem
                         {
                             Value = n.CategoryId.ToString(),
                             Text = n.Category1
                         }).ToList();
            var categorytip = new SelectListItem()
            {
                Value = "0",
                Text = "--- اختر منتج ---"
            };
            categories.Insert(0, categorytip);
            ViewBag.CategoriesList = new SelectList(categories, "Value", "Text", "0");
           // ViewBag.CategoriesList = new SelectList(categories, "CategoryId", "Category1", "1");
           // if(isAdminUser)
            ViewBag.IsUserAdmin = isAdminUser();
/////////////////////////////////////////////////////////////////////////////////////////////

            //     if (User.Identity.IsAuthenticated)
            //     {
            //         var user = User.Identity;

            //    // ApplicationDbContext context = new ApplicationDbContext();
            //  //   var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ////     var result1 = UserManager.AddToRole(user.GetUserId(), "Admin");
            //     ViewBag.displayMenu = "No";

            //        if (isAdminUser())
            //       {
            //             ViewBag.displayMenu = "Yes";
            //       }
            //         return View();
            //     }
            //     else
            //     {
            //         ViewBag.Name = "Not Logged IN";
            //     }
            // return View();



            ////////////////////////////////////////////////////////////


            return View();
        }

        // POST: Commodities1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommodityId,Title,Details,SubCategoryId,SellerId,Quantity,Price,PriceUnitId,PriceNote,Available,Publish,CreationDate,ExpireDate,IsFeatured,OriginalPrice")] Commodity commodity)
        {//, IEnumerable<HttpPostedFileBase> files
            if (ModelState.IsValid)
            {

                if (commodity.CreationDate >= commodity.ExpireDate)
                {
                    ModelState.AddModelError("FinishDate", "Finish Date needs to be after the Start Date!");
                    return View(commodity);
                }
                DateTime today = DateTime.Now.Date;
                commodity.CreationDate = today;
                var user = User.Identity;
                commodity.SellerId = user.GetUserId();
                db.Commodities.Add(commodity);
                db.SaveChanges();
                //var i = 1;
                //foreach (var file1 in files)
                //{
                   
                //    if (file1 != null)
                //    {
                //        var filetext = file1.FileName.Split('.');
                //        file1.SaveAs(HttpContext.Server.MapPath("~/img/commodities/")
                //                                           +i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1]);
                //        CommodityImages comimages = new CommodityImages();
                //        comimages.ImagePath = i+"Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1];
                //        comimages.CommodityId = commodity.CommodityId;
                //       db.CommodityImages.Add(comimages);
                //        db.SaveChanges();
                       
                //    }
                //    i=i + 1;
                //}
                 
                return RedirectToAction("Index");
            }

           ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email", commodity.SellerId);
           ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", commodity.SubCategoryId);
          ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1", commodity.PriceUnitId);
            List<SelectListItem> categories = db.Categories.AsNoTracking()

                          .Select(n =>
                          new SelectListItem
                          {
                              Value = n.CategoryId.ToString(),
                              Text = n.Category1
                          }).ToList();
            var categorytip = new SelectListItem()
            {
                Value = "0",
                Text = "--- اختر منتج ---"
            };
            categories.Insert(0, categorytip);
            ViewBag.CategoriesList = new SelectList(categories, "Value", "Text", "0");
            ViewBag.IsUserAdmin = isAdminUser();
            return View(commodity);

        }
     
        // GET: Commodities1/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }

                  
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1",commodity.PriceUnitId);
            commodity.SubCategoryId = GetParentGategory((decimal)commodity.SubCategoryId);

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1", commodity.SubCategoryId.ToString());
            var subCategory = db.SubCategories.Where(x => x.CategoryId == commodity.SubCategoryId);
            ViewBag.SubCategoryId = new SelectList(subCategory, "SubCategoryId", "SubCategory1", commodity.SubCategoryId.ToString());

            ViewBag.SelectedSubCategoryId = commodity.SubCategoryId.ToString();
          
     
           ViewBag.IsUserAdmin = isAdminUser();
           return View(commodity);
        }

        // POST: Commodities1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommodityId,Title,Details,SubCategoryId,SellerId,Quantity,Price,PriceUnitId,PriceNote,Available,Publish,CreationDate,ExpireDate,IsFeatured,OriginalPrice")] Commodity commodity)
        {//, IEnumerable<HttpPostedFileBase> files
            if (ModelState.IsValid)
            {
                if (commodity.CreationDate >= commodity.ExpireDate)
                {
                    ModelState.AddModelError("FinishDate", "Finish Date needs to be after the Start Date!");
                    return View(commodity);
                }
                db.Entry(commodity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            // ViewBag.SellerId = new SelectList(db.AspNetUsers, "Id", "Email", commodity.SellerId);
             ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategory1", commodity.SubCategoryId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Category1", commodity.SubCategoryId.ToString());
            ViewBag.PriceUnitId = new SelectList(db.PriceUnits, "PriceUnitId", "PriceUnit1", commodity.PriceUnitId);
            ViewBag.IsUserAdmin = isAdminUser();
            //var i = 1;
            //foreach (var file1 in files)
            //{

            //    if (file1 != null)
            //    {
            //        var filetext = file1.FileName.Split('.');
            //        file1.SaveAs(HttpContext.Server.MapPath("~/img/commodities/")
            //                                           + i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1]);
            //        CommodityImages comimages = new CommodityImages();
            //        comimages.ImagePath = i + "Commodity" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + "." + filetext[1];
            //        comimages.CommodityId = commodity.CommodityId;
            //       db.CommodityImages.Add(comimages);
            //        db.SaveChanges();

            //    }
            //    i = i + 1;
            //}
            return View(commodity);
        }
        public ActionResult DeleteImage(string search)
        {
            if (search == null && search == "")
            {
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommodityImage commodityimages = new CommodityImage();
            commodityimages= db.CommodityImages.Find(search);
            if (commodityimages == null)
            { return HttpNotFound(); }
            else
               db.CommodityImages.Remove(commodityimages);
            db.SaveChanges();

           return HttpNotFound();
        }
        // GET: Commodities1/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commodity commodity = db.Commodities.Find(id);
            if (commodity == null)
            {
                return HttpNotFound();
            }
            return View(commodity);
        }

        // POST: Commodities1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Commodity commodity = db.Commodities.Find(id);
            db.Commodities.Remove(commodity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
