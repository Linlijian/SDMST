using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WEBAPP.Controllers;


namespace WEBAPP.Areas.MIS
{
    public class MISBaseController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}