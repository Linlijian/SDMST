using System.Web.Mvc;

namespace WEBAPP.Areas.SEC
{
    public class SECAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SEC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SEC_default",
                "SEC/{controller}/{action}/{id}",
                new { controller = "Profile", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}