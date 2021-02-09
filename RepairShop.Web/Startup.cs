using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(RepairShop.Web.Startup))]

namespace RepairShop.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			ConfigureAuth(app);
        }
    }
}
