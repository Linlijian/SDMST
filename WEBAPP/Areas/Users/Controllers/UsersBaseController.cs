using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WEBAPP.Controllers;
using WEBAPP.Helper;

namespace WEBAPP.Areas.Users.Controllers
{
    public class UsersBaseController : BaseController
    {

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            var currentArea = filterContext.RouteData.DataTokens["area"] ?? string.Empty;
            var currentController = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var currentAction = filterContext.ActionDescriptor.ActionName;

            if (Request.UrlReferrer != null &&
                currentAction != StandardActionName.Clear &&
                currentAction != StandardWizardButtonName.Reset &&
                currentAction != "LangUi" &&
                Request.Url.AbsolutePath != Request.UrlReferrer.AbsolutePath &&
                !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                TempData["UrlReferrer"] = Request.UrlReferrer;
            }
            TempData.Keep("UrlReferrer");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //custom authentication challenge logic
            var user = filterContext.HttpContext.User;

            //if (user == null || !user.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new HttpUnauthorizedResult();
            //}
            //else 
            //{
            //    filterContext.Result =
            //        new RedirectToRouteResult(
            //            new RouteValueDictionary(new {controller = "Error", action = "AccessDenied", area = ""}));
            //}
        }
    }
}