using System.Web.Mvc;
using System.Web.Routing;

namespace WEBAPP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{Area}/{controller}/{action}",
                defaults: new { controller = "Account", action = "Index", Area = "Users" },
                namespaces: new string[] { "WEBAPP.Controllers" }
            );
        }
    }
}
