using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace WEBAPP.Filter
{
    public class AuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public string Role;

        public void OnAuthentication(AuthenticationContext filterContext)
        {
           
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {

            }
        }
    }
}