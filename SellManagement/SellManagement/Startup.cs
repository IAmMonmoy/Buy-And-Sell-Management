using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SellManagement.Startup))]
namespace SellManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
