using System;
using System.Web.Mvc;

namespace WEBAPP.Filter
{
    public class CaptchaValidator : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string response = Convert.ToString(filterContext.RequestContext.HttpContext.Request["CaptchaImg"]);
                string SYS_Captcha = Convert.ToString(filterContext.HttpContext.Session.Contents[Helper.SessionHelper.SYS_Captcha]);

                if (SYS_Captcha.Equals(string.Empty))
                {//session หมดอายุจะได้ค่าว่าง
                    filterContext.Controller.ViewData.ModelState.AddModelError("ValidCaptcha", "Please Refresh Captcha");
                }
                else
                {
                    if (!response.Equals(SYS_Captcha))
                    {
                        filterContext.Controller.ViewData.ModelState.AddModelError("ValidCaptcha", "Is Valid Captcha");
                    }
                }
            }
        }
    }

    public class CaptchaValidator2 : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if ((System.Configuration.ConfigurationManager.AppSettings["LoginType"]).Equals("External"))
            {
                if (filterContext.Controller.ViewData.ModelState.IsValid)
                {
                    string response = Convert.ToString(filterContext.RequestContext.HttpContext.Request["CaptchaImg"]);
                    string SYS_Captcha = Convert.ToString(filterContext.HttpContext.Session.Contents[Helper.SessionHelper.SYS_Captcha]);

                    if (SYS_Captcha.Equals(string.Empty))
                    {//session หมดอายุจะได้ค่าว่าง
                        filterContext.Controller.ViewData.ModelState.AddModelError("ValidCaptcha", "Please Refresh Captcha");
                    }
                    else
                    {
                        if (!response.Equals(SYS_Captcha))
                        {
                            filterContext.Controller.ViewData.ModelState.AddModelError("ValidCaptcha", "Is Valid Captcha");
                        }
                    }
                }
            }
        }
    }
}