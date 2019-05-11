using System.Web.Mvc;
using WEBAPP.Controllers;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class UxBaseController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        
    }
}