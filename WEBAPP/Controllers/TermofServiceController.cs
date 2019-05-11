using System.Web.Mvc;

namespace WEBAPP.Controllers
{
    public class TermofServiceController : Controller
    {
        // GET: TermofService
        public ActionResult Index()
        {
            ViewBag.Company = "V-Samert co,LTD";
            ViewBag.title = "Terms of Service - นโยบายความเป็นส่วนตัว";
            ViewBag.PageName = "Terms of Service";


            return View();
        }
    }
}