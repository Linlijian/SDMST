using System.Collections.Generic;
using System.Web.Mvc;
using WEBAPP.Helper;
using WEBAPP.Filter;
using DataAccess.SEC;
using FluentValidation.Mvc;

namespace WEBAPP.Areas.Users.Controllers
{
    public class ProfileController : UsersBaseController
    {
        /////////////////////////////////////////////////////////////////////////////////////// Action
        // GET: Users/Profile
        [RuleSetForClientSideMessages("NOT")]
        public ActionResult Index()
        {
            //if (SessionHelper.SYS_MS_CORP_GROUP == "10018001")
            //{
            //    return RedirectToAction("Info", "SEC_SECM00500", new { area = "SEC", USER_ID = SessionHelper.SYS_USER_ID });
            //}

            return RedirectToAction("Info", "SEC_SECM00501", new { area = "SEC", USER_ID = SessionHelper.SYS_USER_ID });

            //SetDefaulButton(StandardButtonMode.Modify);

            //var user = new UserDA();
            //user.DTO.Execute.ExecuteType = UserExecuteType.GetByID;
            //string user_id = SessionHelper.SYS_USER_ID;
            //string app_code = SessionHelper.SYS_COM_CODE;
            //user.DTO.User.USER_ID = user_id;
            ////user.DTO.User.COM_CODE = app_code;
            //user.Select(user.DTO);
            //if (user.DTO.User.USER_TYPE == 1)
            //{
            //    user.DTO.User.USER_TYPE_NAME = "คปภ.";
            //}
            //else
            //{
            //    user.DTO.User.USER_TYPE_NAME = "บริษัทประกันภัย.";
            //}
            //return View(StandardActionName.Index, user.DTO.User);
        }

        // GET: Users/Profile/ResetPW
        public ActionResult ResetPW()
        {
            return View();
        }

        [Clear]
        public ActionResult Clear(string refAction)
        {
            return RedirectToAction(refAction, GetRoute());
        }
        
    }
}