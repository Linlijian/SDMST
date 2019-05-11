using System.Web.Mvc;

namespace WEBAPP.Controllers
{
    public class PrivacyPolicyController : Controller
    {
        // GET: PrivacyPolicy
        public ActionResult Index()
        {
            ViewBag.CompanyName = "V-Samert co,LTD";
            ViewBag.Title = "Privacy Policy - นโยบายความเป็นส่วนตัว";
            ViewBag.PageName = "Privacy Policy";

            return View();
        }
    }
}