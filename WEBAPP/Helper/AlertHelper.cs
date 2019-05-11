using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WEBAPP.Helper
{
    /// <summary>
    /// Code By Jubpas
    /// Date 2016
    /// </summary>
    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";

        public string AlertStyle { get; set; }
        public string Message { get; set; }
        public bool Dismissable { get; set; }
        public string Id  { get; set; }

        public List<string> Type { get; set; }

   
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Error = "danger";
     
    } 

    public class Modal
    {
        public const string TempDataKey = "TempDataModals";
        public const string Id = "TheModal";
        

        public string ModalStyle { get; set; }
        public string MessageBody { get; set; }
        public string MessageHeader { get; set; }
        public bool Dismissable { get; set; }

        public string ActionOk { get; set; }
        public string ActionCancel { get; set; }

        private string CallbackUrl { get; set; }
        public bool Callback { get; set; }

        public string ActionAffterAlert(string actionName = "Index", string controllerName = "" , object routeValue = null)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var urlAction = "";
            var currentController = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];

            if (controllerName == "")
            {
                if (currentController != null) controllerName = currentController.ToString();
            }

            if (routeValue != null)
            {
                urlAction = urlHelper.Action(actionName, controllerName, routeValue);
            }
            else
            {
                urlAction = urlHelper.Action(actionName, controllerName);
            }
            CallbackUrl = urlAction;
            return urlAction;
        }

        public MvcHtmlString RegisterModalDialog()
        {
            StringBuilder  strHtml = new StringBuilder();
            strHtml.AppendLine(" $('#"+ Id + "').modal({");
            strHtml.AppendLine(" backdrop: 'static',");
            strHtml.AppendLine(" keyboard: false");
            strHtml.AppendLine(" });");

            if (Callback)
            {
                strHtml.AppendLine("$('#" + Id + "').on('hidden.bs.modal', function (e) {");
                strHtml.AppendLine(" window.location = \"" + this.CallbackUrl + " \";");
                strHtml.AppendLine("});");
            }


            return MvcHtmlString.Create(strHtml.ToString());
        }

    }
}