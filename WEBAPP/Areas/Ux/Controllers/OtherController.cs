using System;
using System.Web.Mvc;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class OtherController : UxBaseController
    {
        // GET: Ux/Other
        public ActionResult SlidingTime()
        {
            Session["SYS_DateNow"] = DateTime.Now;
            return Content("Slided Time");
        }
    }
}