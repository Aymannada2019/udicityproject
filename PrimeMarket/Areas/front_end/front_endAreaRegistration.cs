using System.Web.Mvc;

namespace PrimeMarket.Areas.front_end
{
    public class front_endAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "front_end";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "front_end_default",
                "front_end/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}