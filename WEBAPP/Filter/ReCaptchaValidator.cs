using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;

namespace WEBAPP.Filter
{
    public class ReCaptchaValidator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var response = filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"];
                string secretKey = WebConfigurationManager.AppSettings["reCaptchaSecretKey"];
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                if (!Convert.ToBoolean(data["success"]))
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError("ValidCaptcha", "Is Valid Captcha"); 
                }
            }
        }
    }
}