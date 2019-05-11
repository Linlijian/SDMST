using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace WEBAPP.Helper
{
    public abstract class PageHelper
    {
        // protected virtual string  
        protected virtual void GetAuth()
        {

        }

        public static string RenderPartialToString(Controller controller, string partialViewName, object model, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName);

            if (result.View != null)
            {
                controller.ViewData.Model = model;
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(controller.ControllerContext, result.View, viewData, tempData, output);
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }

    }

    public class Partials : PageHelper
    {
        public const string StandardButton = "_StandardButton";
        public const string Alert = "_Alert";
        public const string Modals = "_Modals";
        public const string Breadcrumb = "_Breadcrumb";
        public const string WebGridtoolbar = "_WebGridtoolbar";


    }


    public class Breadcrumb
    {
        public const string TempDataKey = "Breadcrumb";
        public string Home { get; set; }

        public string SYS_CODE { get; set; }

        public string PRG_CODE { get; set; }
        public string ProgramName { get; set; }
        public string SystemName { get; set; }
        //public string ProgramUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }



}