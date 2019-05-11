using System.Web.Mvc;
using UtilityLib;
using WEBAPP.Helper;
using System.Linq;

namespace WEBAPP.Areas.Admin.Controllers
{
    public class DefaultController : AdminBaseController
    {
        public ActionResult Index()
        {
            var home = SessionHelper.SYS_MenuModel.Where(m => m.SYS_CODE.AsString().ToUpper() == "HOME").FirstOrDefault();
            if (home != null && AppExtensions.ExistsAction(home.PRG_ACTION, home.PRG_CONTROLLER, home.PRG_AREA))
            {
                return RedirectToAction(home.PRG_ACTION, home.PRG_CONTROLLER, new { Area = home.PRG_AREA, SYS_SYS_CODE = home.SYS_CODE, SYS_PRG_CODE = home.PRG_CODE });
            }
            return View();
        }
    }
}