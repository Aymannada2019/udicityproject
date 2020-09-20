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

namespace PrimeMarket.Controllers
{
    public class AccountsController : Controller
    {
        //https://www.youtube.com/watch?v=P_-zxQYPy5w
        private PrimeMarketEntities db = new PrimeMarketEntities();
        private string UserImagePath = "~/img/Users/";

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(string id)
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

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
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

        // GET: Accounts/Edit/5
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
            ViewBag.VillageId = new SelectList(db.Villages, "VillageId", "Village1", account.VillageId,"-Select Village-");
            //ViewBag.VillageId = new SelectList(m=>m.Villages,SelectList(model.)
            

            return View(account);
        }

        // POST: Accounts/Edit/5
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

        // GET: Accounts/Delete/5
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

        // POST: Accounts/Delete/5
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
