using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PrimeMarket.Startup))]
namespace PrimeMarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
