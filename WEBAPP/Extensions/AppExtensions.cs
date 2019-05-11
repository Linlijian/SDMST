using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using UtilityLib;

namespace WEBAPP.Helper
{
    public static class AppExtensions
    {
        public static SelectList ToSelectList<T>(this IEnumerable<T> data, string dataValueField, string dataTextField)
        {
            return data != null ? new SelectList(data, dataValueField, dataTextField) : null;
        }
        public static SelectList ToSelectList<T>(this IEnumerable<T> data, string dataValueField, string dataTextField, object selectedValue)
        {
            return data != null ? new SelectList(data, dataValueField, dataTextField, selectedValue) : null;
        }
        public static MultiSelectList ToSelectList<T>(this IEnumerable<T> data, string dataValueField, string dataTextField, IEnumerable<object> selectedValue)
        {
            return data != null ? new MultiSelectList(data, dataValueField, dataTextField, selectedValue) : null;
        }
        public static bool ExistsAction(string action, string controller, string area)
        {
            var result = false;
            try
            {
                var controllerFullName = string.Format("WEBAPP.Areas.{0}.Controllers.{1}Controller", area, controller);

                var cont = System.Reflection.Assembly.GetExecutingAssembly().GetType(controllerFullName);
                if (cont != null && cont.GetMethod(action) != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static string GetUrl(this Controller ctrl, string actionName)
        {
            return ctrl.Url.Action(actionName, new { SYS_SYS_CODE = ctrl.Request.GetRequest("SYS_SYS_CODE"), SYS_PRG_CODE = ctrl.Request.GetRequest("SYS_PRG_CODE") });
        }
        public static string GetUrl(this Controller ctrl, string actionName, string controllerName, string areaName)
        {
            return ctrl.Url.Action(actionName, controllerName, new { Area = areaName, SYS_SYS_CODE = ctrl.Request.GetRequest("SYS_SYS_CODE"), SYS_PRG_CODE = ctrl.Request.GetRequest("SYS_PRG_CODE") });
        }
        public static string GetUrl(this Controller ctrl, string actionName, string controllerName, object routeValues)
        {
            var dicRouteValues = new RouteValueDictionary(routeValues);
            dicRouteValues.Add("SYS_SYS_CODE", ctrl.Request.GetRequest("SYS_SYS_CODE"));
            dicRouteValues.Add("SYS_PRG_CODE", ctrl.Request.GetRequest("SYS_PRG_CODE"));
            return ctrl.Url.Action(actionName, controllerName, dicRouteValues);
        }
        public static string GetUrl(this WebViewPage wvp, string action)
        {
            return wvp.Url.Action(action, new { SYS_SYS_CODE = wvp.Request.GetRequest("SYS_SYS_CODE"), SYS_PRG_CODE = wvp.Request.GetRequest("SYS_PRG_CODE") });
        }
        public static string GetUrl(this WebViewPage wvp, string actionName, string controllerName, string areaName)
        {
            return wvp.Url.Action(actionName, controllerName, new { Area = areaName, SYS_SYS_CODE = wvp.Request.GetRequest("SYS_SYS_CODE"), SYS_PRG_CODE = wvp.Request.GetRequest("SYS_PRG_CODE") });
        }
        public static string GetUrl(this WebViewPage wvp, string actionName, string controllerName, object routeValues)
        {
            var dicRouteValues = new RouteValueDictionary(routeValues);
            dicRouteValues.Add("SYS_SYS_CODE", wvp.Request.GetRequest("SYS_SYS_CODE"));
            dicRouteValues.Add("SYS_PRG_CODE", wvp.Request.GetRequest("SYS_PRG_CODE"));
            return wvp.Url.Action(actionName, controllerName, dicRouteValues);
        }


        public static List<ValidationError> AsValidationError(this PKIResult data)
        {
            var errors = new List<ValidationError>();
            try
            {
                if (data.ErrorType == PKIErrorType.DataFile)
                {
                    foreach (var item in data.FileData)
                    {
                        if (item.Value.GetType() == typeof(FileUpload))
                        {
                            var file = (FileUpload)data.FileData[item.Key];
                            if (!file.Success)
                            {
                                errors.Add(new ValidationError(item.Key, file.ErrorMSG));
                            }
                        }
                        else
                        {
                            foreach (var file in (List<FileUpload>)data.FileData[item.Key])
                            {
                                if (!file.Success)
                                {
                                    errors.Add(new ValidationError(item.Key, file.ErrorMSG));
                                }
                            }
                        }
                    }
                }
                else if (data.ErrorType == PKIErrorType.Certificate)
                {
                    errors.Add(new ValidationError("ใบรับรอง", data.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
            }
            return errors;
        }
    }

    public class JubpasViewEngine : RazorViewEngine
    {

        private static readonly string[] NewPartialViewFormats = new[] {
        "~/Views/{1}/Partials/{0}.cshtml",
        "~/Views/Shared/Partials/{0}.cshtml",
        "~/Views/Partials/{0}.cshtml"
        };

        public JubpasViewEngine()
        {
            base.PartialViewLocationFormats = base.PartialViewLocationFormats.Union(NewPartialViewFormats).ToArray();
        }

    }
}