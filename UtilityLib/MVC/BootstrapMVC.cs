using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace UtilityLib.MVC
{
    public static class BootstrapMVC
    {
        public static MvcHtmlString GetWidgetHeader(this HtmlHelper html, string title = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("<div class=\"widget-header\">");
            sb.AppendLine("<h4 class=\"widget-title\">" + title + "</h4>");
            sb.AppendLine("<div class=\"widget-toolbar\">");
            sb.AppendLine(" <a href=\"#\" data-action=\"collapse\">");
            sb.AppendLine("<i class=\"ace-icon fa fa-chevron-up\"></i>");
            sb.AppendLine("</a></div ></div>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString GetWidgetHeaderFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {


            return MvcHtmlString.Create("");
        }

    }
}
