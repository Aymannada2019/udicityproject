using Microsoft.AspNet.Identity;
using PrimeMarket.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

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
    }
    
}