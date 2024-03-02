using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantChain.Startup))]
namespace RestaurantChain
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
