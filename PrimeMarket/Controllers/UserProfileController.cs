using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PrimeMarket.Models;
using System.Net;
using System.Data.Entity;

namespace PrimeMarket.Controllers
{
    public class UserProfileController : Controller
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();
        private string UserImagePath = "~/img/Users/";

        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }

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
            //ViewBag.VillageId = new SelectList(db.Villages, "VillageId", "Village1", account.VillageId, "-Select Village-");
            //ViewBag.VillageId = new SelectList(m=>m.Villages,SelectList(model.)
            return View(account);
        }

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
            return View(account);
        }
    }
}