using System.Web.Mvc;

namespace WEBAPP.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}",
                new { controller = "Dashboard", action = "Index" }
            );
        }
    }
}