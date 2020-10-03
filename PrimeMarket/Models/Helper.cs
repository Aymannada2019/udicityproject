using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PrimeMarket.Models
{
    public class Helper
    {
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

        public static double getCartTotal(out int CartItemsCount)
        {
            double total = 0;
            CartItemsCount = 0;
            PrimeMarketEntities context = new PrimeMarketEntities();
            var CustomerId = "8ac3f426-e76d-4ed8-94c1-835addf528bc";
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