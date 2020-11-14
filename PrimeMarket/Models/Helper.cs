using PrimeMarket.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNet.Identity;


namespace PrimeMarket.Models
{
    public class Helper
    {
        // ApplicationDbContext db = new ApplicationDbContext();
        public static void SendEmail(string To,string Subject, string Body)
        {
            try
            {
                string From = "Souq.prime.arc@gmail.com";
                string PWD = "S0uQ.9R!M3Pa55";
                MailMessage mail = new MailMessage(From, To);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                
                SmtpClient client = new SmtpClient();
                client.Port = 587;// 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(From, PWD);
                client.Host = "smtp.gmail.com";
                client.Send(mail);
            }
            catch
            {

            }
        }

        public static bool isInRole(string UserName, string rolename)
        {
            PrimeMarketEntities db = new PrimeMarketEntities();
            AspNetRole R = db.AspNetRoles.Where(x => x.Name == rolename).FirstOrDefault();
            if(R!=null)
            {
                var RoleId = R.Id;
                //var userRole = db.AspNetUser
            }
            return false;
        }
        public static string getUserFullName(string UserName)
        {
            PrimeMarketEntities db = new PrimeMarketEntities();
            AspNetUser user = db.AspNetUsers.Where(x => x.UserName == UserName).FirstOrDefault();
            if (user != null)
                return user.FullName;
            return string.Empty;
        }


        public static double getCartTotal(out int CartItemsCount)
        {
            double total = 0;
            CartItemsCount = 0;
            PrimeMarketEntities context = new PrimeMarketEntities();
            var CustomerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(CustomerId))
            {
                //return RedirectToAction("Login", "Account",new { ReturnUrl ="/shop/index"});
                var tempCustomerId = Helper.GetCookie("tempCustomerId");
                if (tempCustomerId == null)
                {
                    tempCustomerId = Guid.NewGuid().ToString();
                    Helper.SetCookie("tempCustomerId", tempCustomerId, DateTime.Now.AddDays(20));
                }
                CustomerId = tempCustomerId;
            }
            try
            {
                var cart = context.Carts.Where(c => c.CustomerId == CustomerId).ToList();
                CartItemsCount = cart.Count;
                foreach (var item in cart)
                {
                    total += (double)item.Commodity.Price * (double)item.Quantity;
                }
            }
            catch
            { }
            return total;
        }

        public static string GetCookie(string CookieField)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieField];
            if (cookie != null)
                return cookie.Value;
            return null;
        }
        public static void SetCookie(string CookieField, string value, DateTime expireDate)
        {
            HttpContext.Current.Response.Cookies[CookieField].Value=value;
            HttpContext.Current.Response.Cookies[CookieField].Expires = expireDate;
        }
        public static void MigrateCart(string LoggedInUserId)
        {
            PrimeMarketEntities db = new PrimeMarketEntities();
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return;
            var CustomerId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            if (string.IsNullOrEmpty(CustomerId))
                CustomerId = LoggedInUserId;
            var tempCustomerId = GetCookie("tempCustomerId");
            if(tempCustomerId!=null)
            {
                var CustomerCartItems = db.Carts.Where(c => c.CustomerId == CustomerId).ToList();
                var tempCustomerCartItems = db.Carts.Where(c => c.CustomerId == tempCustomerId).ToList();
                foreach(var tempItem in tempCustomerCartItems)
                {
                    // check if temp item in the temp cart already exist in user cart, then update the old quantity else add the temp item
                    var item = CustomerCartItems.Where(c => c.CommodityId == tempItem.CommodityId).FirstOrDefault();
                    if (item!=null)
                    {
                        item.Quantity += tempItem.Quantity;
                        db.Carts.Remove(tempItem);
                    }
                    else
                    {
                        tempItem.CustomerId = CustomerId;
                    }
                }
                db.SaveChanges();
                // delete cookies
                SetCookie("tempCustomerId", tempCustomerId, DateTime.Now.AddDays(-1));
            }


        }

    }
    
}