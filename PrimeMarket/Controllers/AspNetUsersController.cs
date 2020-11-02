using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PrimeMarket.Models;
using System.Data.Entity.Validation;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PrimeMarket.Controllers
{
    [Authorize]
    public class AspNetUsersController : Controller
    {
        //https://www.youtube.com/watch?v=P_-zxQYPy5w
        private PrimeMarketEntities db = new PrimeMarketEntities();
        private string UserImagePath = "~/img/Users/";

        // GET: AspNetUsers
        public ActionResult Index()
        {
            if (!isAdminUser())  //certain user
            {
                string loginId = User.Identity.GetUserId();
                return View(db.AspNetUsers.Where(x => x.Id == loginId).ToList());
            }
            //admin user
            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //to check the user
            string loginId = User.Identity.GetUserId();
            if (id != loginId)
                return RedirectToAction("LogOff", "Account");
            AspNetUser account = db.AspNetUsers.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "AccountId,FullName,Address,Phones,Email,PassWord,ImagePath")] Account account)
        public ActionResult Create(AspNetUser account)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser account = db.AspNetUsers.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            //old code without village
            /*
            account.GovernoratetId = GetGovernorate((decimal)account.DistrictId);
            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", account.GovernoratetId.ToString());

            var District = db.Districts.Where(x => x.GovernorateId == account.DistrictId);
            ViewBag.DistrictId = new SelectList(District, "DistrictId", "District1", account.DistrictId.ToString());
            ViewBag.SelectedDistrictId = account.DistrictId.ToString();

            ViewBag.VillageId = new SelectList(db.Villages, "VillageId", "Village1", account.VillageId);
            */


            //new code with village
            account.DistrictId = GetDistrict((decimal)account.VillageId);
            account.GovernoratetId = GetGovernorate((decimal)account.DistrictId);

            ViewBag.DistrictId = new SelectList(db.Districts.Where(x=>x.GovernorateId == account.GovernoratetId) , "DistrictId", "District1", account.DistrictId.ToString());
            ViewBag.SelectedDistrictId = account.DistrictId.ToString();

            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", account.GovernoratetId.ToString());
            ViewBag.VillageId = new SelectList(db.Villages.Where(x => x.DistrictId == account.DistrictId) , "VillageId", "Village1", account.VillageId);
            

            ViewBag.IsUserAdmin = isAdminUser();
            return View(account);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,FullName,Address,Phones,Email,PassWord,ImagePath")] AspNetUser account)
        public ActionResult Edit(AspNetUser account)
        {
            if (ModelState.IsValid)
            {
                var tobe_UpdatedAccount = db.AspNetUsers.Where(u => u.Id == account.Id).FirstOrDefault(); //extendit fun.
                //var xx = (from u in db.AspNetUsers where u.Id == account.Id orderby u.Email select u).FirstOrDefault();//sql like
                db.Entry(tobe_UpdatedAccount).State = EntityState.Modified;
                
                tobe_UpdatedAccount.FullName = account.FullName;
                tobe_UpdatedAccount.Address = account.Address;
                tobe_UpdatedAccount.PhoneNumber = account.PhoneNumber;
                tobe_UpdatedAccount.VillageId = account.VillageId;
                tobe_UpdatedAccount.ImagePath = account.ImagePath;
                //perper the user image
                HttpPostedFileBase file = Request.Files["ImagePath"];

                if (file.FileName != "")  //there is file in view
                {
                    if (tobe_UpdatedAccount.ImagePath != "" && tobe_UpdatedAccount.ImagePath != null) //delete the exist image
                    {
                        string oldimage = Server.MapPath(UserImagePath + tobe_UpdatedAccount.ImagePath);
                        System.IO.File.Delete(oldimage);
                    }
                    //upload new file
                    var fileext = file.FileName.Split('.');
                    file.SaveAs(HttpContext.Server.MapPath(UserImagePath) + account.Id + "." + fileext[1]);
                    tobe_UpdatedAccount.ImagePath = account.Id + "." + fileext[1];
                }
              
                db.SaveChanges();
                ViewBag.VillageId = new SelectList(db.Villages, "VillageId", "Village1", account.VillageId, "-Select Village-");
                return RedirectToAction("Index");
            }
            
            //translate
            account.DistrictId = GetGovernorate((decimal)account.DistrictId);
            ViewBag.GovernorateId = new SelectList(db.Governorates, "GovernorateId", "Governorate1", account.DistrictId.ToString());
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictId", account.DistrictId);
            ViewBag.VillageId = new SelectList(db.Villages, "VillageId", "Village1", account.VillageId);
            ViewBag.IsUserAdmin = isAdminUser();
            return View(account);
        }

        public ActionResult ChangeImage(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser account = db.AspNetUsers.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,FullName,Address,Phones,Email,PassWord,ImagePath")] AspNetUser account)
        public ActionResult ChangeImage(AspNetUser account)
        {
            if (ModelState.IsValid)
            {
                var tobe_UpdatedAccount = db.AspNetUsers.Where(u => u.Id == account.Id).FirstOrDefault(); //extendit fun.
                db.Entry(tobe_UpdatedAccount).State = EntityState.Modified;

                //perper the user image
                HttpPostedFileBase file = Request.Files["ImagePath"];
                if (file.FileName != "")  //there is file in view
                {
                    if (tobe_UpdatedAccount.ImagePath != "" && tobe_UpdatedAccount.ImagePath != null) //delete the exist image
                    {
                        string oldimage = Server.MapPath(UserImagePath + tobe_UpdatedAccount.ImagePath);
                        System.IO.File.Delete(oldimage);
                    }
                    //upload new file
                    var fileext = file.FileName.Split('.');
                    file.SaveAs(HttpContext.Server.MapPath(UserImagePath) + account.Id + "." + fileext[1]);
                    tobe_UpdatedAccount.ImagePath = account.Id + "." + fileext[1];
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(account);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser account = db.AspNetUsers.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser account = db.AspNetUsers.Find(id);
            //delete User image (if exist)
            if (account.ImagePath != "" && account.ImagePath != null) //delete the exist image
            {
                string userimage = Server.MapPath(UserImagePath + account.ImagePath);
                System.IO.File.Delete(userimage);
            }

            db.AspNetUsers.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());

                if (s.Count > 0 && s[0].ToString() == "Admin")
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult getDistrictList(int GovernorateId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<District> DistrictList = db.Districts.Where(district => district.GovernorateId == GovernorateId).ToList();
            return Json(DistrictList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getVillageList(int DistrictId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Village> VillageList = db.Villages.Where(village => village.DistrictId == DistrictId).ToList();
            return Json(VillageList, JsonRequestBehavior.AllowGet);
        }

        //pub
        
        public decimal GetGovernorate(decimal DistrictId)  //public decimal GetParentGategory(decimal subCategoryId)
        {
            decimal GovernorateId = 0;
            var District = db.Districts.Where(x => x.DistrictId == DistrictId);
            foreach (var item in District)
            {
                GovernorateId = (decimal)item.GovernorateId;
            }
            return GovernorateId;
        }

        public decimal GetDistrict(decimal VillageId) 
        {
            decimal DistrictsId = 0;
            var Village = db.Villages.Where(x => x.VillageId == VillageId);
            foreach (var item in Village)
            {
                DistrictsId = (decimal)item.DistrictId;
            }
            return DistrictsId;
        }


    }
}
