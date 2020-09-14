using PrimeMarket.Models;
using PrimeMarket.Models.UIViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrimeMarket.Controllers
{
    public class ShoppingCartController : Controller
    {
        public PrimeMarketEntities db = new PrimeMarketEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var model = new CartViewModel();
            return View(model);
        }
    }
}