using System;
using System.Reflection;
using System.Web.Mvc;

namespace WEBAPP.Filter
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ActionButtonAttribute : ActionNameSelectorAttribute
    {
        public string FormKey { get; set; } = "button";
        public string ButtonValue { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[FormKey] != null &&
                controllerContext.HttpContext.Request[FormKey] == ButtonValue;
        }

    }

    public class ClearAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // ... log stuff before execution  
            base.OnActionExecuting(filterContext);          
        }

    }
}