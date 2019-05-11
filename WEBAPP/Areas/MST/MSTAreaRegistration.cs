using System.Web.Mvc;

namespace WEBAPP.Areas.MST
{
    public class MSTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MST";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MST_default",
                "MST/{controller}/{action}/{id}",
                new { controller = "Profile", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}