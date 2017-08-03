using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarRepairWeb.Startup))]
namespace CarRepairWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
