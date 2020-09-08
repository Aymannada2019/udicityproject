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
        private PrimeMarketEntities db = new PrimeMarketEntities();

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
                var tobeupdatedAccount = db.AspNetUsers.Where(u => u.Id == account.Id).FirstOrDefault(); //extendit fun.
                //var xx = (from u in db.AspNetUsers where u.Id == account.Id orderby u.Email select u).FirstOrDefault();//sql like

                tobeupdatedAccount.FullName = account.FullName;
                tobeupdatedAccount.Address = account.Address;
                tobeupdatedAccount.PhoneNumber = account.PhoneNumber;

                db.Entry(tobeupdatedAccount).State = EntityState.Modified;
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
