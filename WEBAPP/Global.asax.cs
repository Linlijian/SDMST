using FluentValidation.Mvc;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WEBAPP.Helper;

namespace WEBAPP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Add(new JubpasViewEngine()); //Custom ViewEngine

            FluentValidationModelValidatorProvider.Configure();
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {

            //ref1 => http://www.chambaud.com/2013/02/27/localization-in-asp-net-mvc-4/
            //ref2 => http://www.codeproject.com/Articles/778040/Beginners-Tutorial-on-Globalization-and-Localizati
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Fix By Jubpas Cast en-US
            var cInfo = new CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    ShortDatePattern = "dd/MM/yyyy",
                    DateSeparator = "/"
                }
            };
            Thread.CurrentThread.CurrentCulture = cInfo;
            //Thread.CurrentThread.CurrentUICulture = cInfo;
        }

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    HttpException httpException = (HttpException)exception;
        //    int httpCode = httpException.GetHttpCode();

        //    if (httpCode == 404)
        //    {
        //        Server.ClearError();

        //        RouteData routeData = new RouteData();
        //        routeData.Values.Add("controller", "Error");
        //        routeData.Values.Add("action", "Index");
        //        routeData.Values.Add("exception", exception);
        //        routeData.Values.Add("errorcode", httpCode.ToString());

        //        if (exception.GetType() == typeof(HttpException))
        //        {
        //            routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
        //        }
        //        else
        //        {
        //            routeData.Values.Add("statusCode", 500);
        //        }

        //        IController controller = new WEBAPP.Controllers.ErrorController();
        //        controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //        Response.End();
        //    }
        //}
    }
}
