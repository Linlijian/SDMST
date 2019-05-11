using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WEBAPP.Controllers;
using WEBAPP.Helper;

namespace WEBAPP.Areas.Ux.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Ux/Report
        [HttpPost]
        public JsonResult DeletePrintFile()
        {
            string path = "~/Uploads/Temp";
            string fPath = Server.MapPath(path);

            DirectoryInfo d = new DirectoryInfo(fPath);
            foreach (FileInfo file in d.GetFiles(SessionHelper.SYS_USER_ID + SessionHelper.SYS_CurrentPRG_CODE + "*.*"))
            {
                file.Delete();
            }
            var fMorethan7d = d.GetFiles().Where(m => m.CreationTime > DateTime.Now.AddDays(-7));
            if (fMorethan7d.Any())
            {
                foreach (var item in fMorethan7d)
                {
                    item.Delete();
                }
            }
            return Json(new WEBAPP.Models.AjaxResult(StandardActionName.Delete, true));
        }
    }
}