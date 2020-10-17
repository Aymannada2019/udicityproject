using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrimeMarket.Models.UIViewModel
{
    public class UserProductsViewModel
    {
        private PrimeMarketEntities db = new PrimeMarketEntities();
        public AspNetUser ThisUser;
        public List<UserRating> ThisUserRatings = new List<UserRating>();
        public List<UserRating> ThisUserReviews = new List<UserRating>();
        public double Rank = 0;
        public List<Commodity> UserItems = new List<Commodity>();
        public UserProductsViewModel(string UserId)
        {
            ThisUser = db.AspNetUsers.Where(x => x.Id == UserId).FirstOrDefault();
            UserItems = db.Commodities.Where(x=>x.SellerId == UserId).ToList();
            ThisUserRatings = ThisUser.UserRatings1.Where(r => r.Rating != null).ToList();
            ThisUserReviews = ThisUser.UserRatings1.Where(r => r.Comment != null).OrderByDescending(r => r.CreationDate).ToList();
            Rank = (double)ThisUserRatings.Sum(r => r.Rating) / (double)ThisUserRatings.Count;
        }
    }
}