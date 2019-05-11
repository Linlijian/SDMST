using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WEBAPP.Startup))]
namespace WEBAPP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
