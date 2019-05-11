using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WEBAPP.Controllers;

namespace WEBAPP.Areas.Admin.Controllers
{
    public class AdminBaseController : BaseController
    {
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            //custom authentication logic
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //custom authentication challenge logic
            var user = filterContext.HttpContext.User;
        }
    }
}