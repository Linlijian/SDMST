using System.Web.Mvc;

namespace WEBAPP.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        //[Log]
        //public ActionResult Error()
        //{
        //    return RedirectToAction("Index");
        //}

        [Log]
        public ActionResult Index(string exception, string errorcode)
        {
            string filterContextController = "";
            string filterContextExceptionMessage = "";
            if (!string.IsNullOrEmpty(exception))
            {
                filterContextController = exception;
            }
            if (!string.IsNullOrEmpty(errorcode))
            {
                filterContextExceptionMessage = errorcode;
            }
            if (Session["filterContextController"] != null)
            {
                filterContextController = Session["filterContextController"].ToString();
                Session.Remove("filterContextController");
            }
            if (Session["filterContextExceptionMessage"] != null)
            {
                filterContextExceptionMessage = Session["filterContextExceptionMessage"].ToString();
                Session.Remove("filterContextExceptionMessage");
            }

            ViewBag.errorcontroller = filterContextController;
            ViewBag.errorcessage = filterContextExceptionMessage;

            return View();
        }

        [Log]
        public ActionResult AccessDenied()
        {
            return Content("AccessDenied ERORR");
        }

        [Log]
        public ActionResult NotFound()
        {
            return Content("NotFound ERORR");
        }

        [Log]
        public ActionResult BadRequest()
        {
            return Content("BadRequest ERORR");
        }

    }
}