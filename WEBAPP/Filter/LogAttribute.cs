using System.Web.Mvc;

namespace WEBAPP
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // ... log stuff before execution            
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // ... log stuff after execution
        }
    }
}