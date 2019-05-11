using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WEBAPP.Controllers;


namespace WEBAPP.Areas.SEC
{
    public class SECBaseController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}