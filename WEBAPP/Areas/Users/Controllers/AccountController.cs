using DataAccess.SEC;
using DataAccess.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using UtilityLib;
using WEBAPP.Filter;
using WEBAPP.Helper;
using FluentValidation.Mvc;

using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using DataAccess;

namespace WEBAPP.Areas.Users.Controllers
{
    public class AccountController : UsersBaseController
    {
        #region Property
        private UserModel localModel = new UserModel();
        private UserModel TempModel
        {
            get
            {
                if (TempData["Model" + SessionHelper.SYS_CurrentAreaController] == null)
                {
                    TempData["Model" + SessionHelper.SYS_CurrentAreaController] = new UserModel();
                }
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
                return TempData["Model" + SessionHelper.SYS_CurrentAreaController] as UserModel;
            }
            set
            {
                TempData["Model" + SessionHelper.SYS_CurrentAreaController] = value;
                TempData.Keep("Model" + SessionHelper.SYS_CurrentAreaController);
            }
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("RootLogger");

        #endregion

        #region View
        public ActionResult Index()
        {
            ViewBag.UrlToClosePage = "";

            var user = HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
            }

            TempModel = new UserModel();

            return RedirectToAction("SignIn");
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SignIn(string returnUrl, string Username)
        {
            ViewBag.message = string.Empty;
            ViewBag.messageHeader = string.Empty;
            string cdsid = Username;

            if (!string.IsNullOrEmpty(Request.ServerVariables["AUTH_USER"].ToString().Trim()))
            {
                cdsid = Request.ServerVariables["AUTH_USER"].ToString().Split('\\')[0];
            }

            SECS02P002Model dsSys = GET_VSMS_USER(cdsid);

            if (!dsSys.USER_ID.IsNullOrEmpty())
            {
                if (dsSys.IS_DISABLED == "Y")
                {
                    ViewBag.message = string.Concat("-", "Your account is disabled,please contact your system administrator", "<br/>");
                }
                else
                {
                    if (!dsSys.COM_CODE.IsNullOrEmpty() && !dsSys.USG_ID.IsNullOrEmpty())
                    {
                        SECS02P001Model dsUSerGroup = GET_VSMS_USRGROUP(dsSys.COM_CODE, dsSys.USG_ID);
                        if (!dsUSerGroup.COM_CODE.IsNullOrEmpty() &&
                            dsUSerGroup.USG_ID != null &&
                            !dsUSerGroup.USG_CODE.IsNullOrEmpty())
                        {
                            if (!dsUSerGroup.USG_STATUS.IsNullOrEmpty() &&
                                dsUSerGroup.USG_STATUS == "D")
                            {
                                ViewBag.message = string.Concat("-", "Your user group is disable,plase contarct your system administrator", "<br/>");
                            }
                            else
                            {
                                FormsAuthentication.SetAuthCookie(cdsid.Trim(), false);

                                LogInResult enmLogInResult = CheckUserLogInForWindowAuthen(cdsid.Trim());
                                if (enmLogInResult == LogInResult.WrongUserNameOrPassword)
                                {
                                    ViewBag.message = string.Concat("-", "Wrong username or password", "<br/>");
                                }
                                else if (enmLogInResult == LogInResult.Success)
                                 {
                                    return RedirectToAction("SelectModule");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ViewBag.message = string.Concat("User ", cdsid, ": Wrong user name or password");
            }

            return View("SignIn");
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SignIn2(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (Session[SessionSystemName.SYS_USER_ID] != null && !Convert.ToString(Session[SessionSystemName.SYS_USER_ID]).IsNullOrEmpty())
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
            }

            var da = new UserDA();
            da.DTO.Execute.ExecuteType = UserExecuteType.GetUser;
            da.DTO.Model.COM_CODE = "AAT";
            da.DTO.Model.USER_ID = "vsmadmin";
            da.Select(da.DTO);
            if (!da.DTO.Result.IsResult)
            {
                log.Error(da.DTO.Result.ResultMsg);
                return RedirectToAction("SignOut");
            }
            Session[SessionSystemName.SYS_COM_CODE] = da.DTO.Model.COM_CODE;
            Session[SessionSystemName.SYS_USER_ID] = da.DTO.Model.USER_ID;
            Session[SessionSystemName.SYS_USER_FNAME_TH] = da.DTO.Model.USER_FNAME_TH;
            Session[SessionSystemName.SYS_USER_FNAME_EN] = da.DTO.Model.USER_FNAME_EN;
            Session[SessionSystemName.SYS_USER_LNAME_TH] = da.DTO.Model.USER_LNAME_TH;
            Session[SessionSystemName.SYS_USER_LNAME_EN] = da.DTO.Model.USER_LNAME_EN;
            Session[SessionSystemName.SYS_USG_ID] = da.DTO.Model.USG_ID;
            Session[SessionSystemName.SYS_COM_NAME_TH] = da.DTO.Model.COM_NAME_T;
            Session[SessionSystemName.SYS_COM_NAME_EN] = da.DTO.Model.COM_NAME_E;
            Session[SessionSystemName.SYS_USG_LEVEL] = da.DTO.Model.USG_LEVEL;
            FormsAuthentication.SetAuthCookie(da.DTO.Model.USER_ID, false);

            #region Check_DB_SERVER_NAME
            var daVSMS_PROGRAM = new SECS01P003DA();
            daVSMS_PROGRAM.DTO.Execute.ExecuteType = SECS01P003ExecuteType.CHECK_DB_SERVER_NAME;
            daVSMS_PROGRAM.SelectNoEF(daVSMS_PROGRAM.DTO);

            if (!daVSMS_PROGRAM.DTO.Model.SERVER_NAME.IsNullOrEmpty())
            {
                Session[SessionSystemName.SYS_ServerDBName] = GetConfigFile(daVSMS_PROGRAM.DTO.Model.SERVER_NAME);
            }
            else
            {
                Session[SessionSystemName.SYS_ServerDBName] = "Hostname is not set.";
            }
            #endregion

            return RedirectToAction("SelectModule");
        }
        
        [HttpGet]
        [AuthAttribute]
        public ActionResult SelectModule()
        {
            if (SessionHelper.SYS_USER_ID.IsNullOrEmpty())
            {
                return RedirectToAction("SignIn");
            }

            if (SessionHelper.SYS_USG_LEVEL != "A" && SessionHelper.SYS_USG_LEVEL != "S")
            {
                string name = "1 Manage Issue";
                return RedirectToAction("SelectedModule" , new { NAME = name });
            }

            var da = new UserDA();
            da.DTO.Execute.ExecuteType = UserExecuteType.GetConfigGeraral;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.USG_LEVEL = SessionHelper.SYS_USG_LEVEL;

            //da.DTO.Model.SYS_GROUP_NAME = SessionHelper.SYS_SYS_GROUP_NAME;
            //da.DTO.Model.SYS_GROUP_NAME = "SEC01";

            da.Select(da.DTO);
            SessionHelper.SYS_IsMultipleGroup = (da.DTO.ConfigGerarals.Count > 1);
            return View(da.DTO.ConfigGerarals);
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SelectedModule(string NAME)
        {
            Session[SessionSystemName.SYS_SYS_GROUP_NAME] = NAME;
            //Session[SessionSystemName.SYS_MODULE] = NAME; //test

            var da = new SECBaseDA();
            da.DTO.Execute.ExecuteType = SECBaseExecuteType.GetMenu;
            da.DTO.Menu.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Menu.USG_LEVEL = SessionHelper.SYS_USG_LEVEL;
            da.DTO.Menu.SYS_GROUP_NAME = NAME;
            da.Select(da.DTO);
            Session[SessionSystemName.SYS_MENU] = da.DTO.Menus;
            return Redirect(FormsAuthentication.DefaultUrl);

            //return Redirect(Url.Action("Index", "Default", new { area = "Admin" }));
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SelectSystem()
        {
            if (SessionHelper.SYS_USER_ID.IsNullOrEmpty())
            {
                return RedirectToAction("SignIn");
            }

            var da = new UserDA();
            da.DTO.Execute.ExecuteType = UserExecuteType.GetConfigSys;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.USG_ID = SessionHelper.SYS_USG_ID;
            da.Select(da.DTO);
            SessionHelper.SYS_IsMultipleGroup = (da.DTO.ConfigGerarals.Count > 1);
            return View(da.DTO.ConfigGerarals);
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SelectedSystem(string NAME)
        {
            Session[SessionSystemName.SYS_SYS_GROUP_NAME] = NAME;
            Session[SessionSystemName.SYS_SYSTEM] = NAME; //test
            return RedirectToAction("SelectModule");
        }

        [HttpGet]
        [AuthAttribute]
        public ActionResult SelectedApp(string COM_CODE)
        {
            if (SessionHelper.SYS_AppModel != null && SessionHelper.SYS_AppModel.Count > 0)
            {
                Session[SessionSystemName.SYS_MENU] = null;
                var data = SessionHelper.SYS_AppModel.Where(m => m.COM_CODE == COM_CODE).FirstOrDefault();

                Session[SessionSystemName.SYS_COM_CODE] = data.COM_CODE;
                Session[SessionSystemName.SYS_USG_ID] = data.USG_ID;
                Session[SessionSystemName.SYS_COM_NAME_TH] = data.COM_NAME_T;
                Session[SessionSystemName.SYS_COM_NAME_EN] = data.COM_NAME_E;
            }

            return Redirect(Url.Action("SelectModule"));
        }

        public ActionResult Error(string exception, string errorcode)
        {
            string ErrorPath = "";
            string ErrorCode = "";
            string ErrorMessage = "";

            if (!string.IsNullOrEmpty(errorcode))
            {
                ErrorCode = errorcode;
            }
            if (!string.IsNullOrEmpty(exception))
            {
                ErrorMessage = exception;
            }
            if (Session[SessionSystemName.SYS_ErrorPath] != null)
            {
                ErrorPath = Session[SessionSystemName.SYS_ErrorPath].AsString();
                Session.Remove(SessionSystemName.SYS_ErrorPath);
            }
            if (Session[SessionSystemName.SYS_ErrorMessage] != null)
            {
                ErrorMessage = Session[SessionSystemName.SYS_ErrorMessage].AsString();
                Session.Remove(SessionSystemName.SYS_ErrorMessage);
            }
            if (Session[SessionSystemName.SYS_ErrorCode] != null)
            {
                ErrorCode = Session[SessionSystemName.SYS_ErrorCode].AsString();
                Session.Remove(SessionSystemName.SYS_ErrorCode);
            }

            ViewBag.ErrorPath = ErrorPath;
            ViewBag.ErrorCode = ErrorCode;
            ViewBag.ErrorMessage = ErrorMessage;

            return View();
        }

        public ActionResult SignOut()
        {
            //if (SessionHelper.SYS_USER_ID != null && SessionHelper.SYS_USER_ID != "")
            //{
            //    InsertAuthenlogs(SessionHelper.SYS_USER_ID, "2", true, Translation.SEC.SEC_SECM00500.SIGNOUT, Request.Url.AbsoluteUri, true, "", "");
            //}

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account", null);
        }
        #endregion

        #region Action

        public ActionResult LangUi(string lang)
        {
            Session[SessionSystemName.SYS_CurrentCulture] = lang;
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult RemoveCountNTF()
        {
            var da = new UserDA();
            da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.UpdateFlag;
            //da.DTO.Notification.NTF_KEY = NTF_KEY;
            da.UpdateNoEF(da.DTO);

            return JsonAllowGet(da);
        }
        public ActionResult FatchNotification()
        {
            var da = new UserDA();
            da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.FatchNotification;
            da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
            da.SelectNoEF(da.DTO);
            SessionHelper.Notification = da.DTO.Notifications;

            da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.GetNotificationCount;
            da.SelectNoEF(da.DTO);



            return JsonAllowGet(da.DTO.Notification);
            //return PartialView("_LayoutNotification", JsonAllowGet(da.DTO.Notification));
        }
        public ActionResult ReloadNotification()
        {
            // return PartialView("_LayoutNotification");
            return PartialView("_LayoutNotification");
        }
        [HttpPost]
        public JsonResult SignOutOnClose()
        {
            //InsertAuthenlogs(SessionHelper.SYS_USER_ID, "2", true, Translation.SEC.SEC_SECM00500.SIGNOUT, Request.Url.AbsoluteUri, true, "", "");

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return Json(new WEBAPP.Models.AjaxResult(StandardActionName.Clear, true));
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Method
        private bool AuthenticateUser(UserModel user)
        {
            var status = false;
            try
            {
                //if (ValidateUser(user))
                //{
                //    Session[SessionSystemName.SYS_COM_CODE] = user.COM_CODE;
                //    Session[SessionSystemName.SYS_USER_ID] = user.USER_ID;
                //    Session[SessionSystemName.SYS_USER_FNAME_TH] = user.USER_FNAME_TH;
                //    Session[SessionSystemName.SYS_USER_FNAME_EN] = user.USER_FNAME_EN;
                //    Session[SessionSystemName.SYS_USER_LNAME_TH] = user.USER_LNAME_TH;
                //    Session[SessionSystemName.SYS_USER_LNAME_EN] = user.USER_LNAME_EN;
                //    Session[SessionSystemName.SYS_SID] = user.SID;
                //    Session[SessionSystemName.SYS_USG_LEVEL] = user.USG_LEVEL;

                //    Session[SessionSystemName.SYS_MS_CORP_ID] = user.MS_CORP_ID;
                //    Session[SessionSystemName.SYS_MS_CORP_CODE] = user.MS_CORP_CODE;
                //    Session[SessionSystemName.SYS_MS_CORP_INITIALS] = user.MS_CORP_INITIALS;
                //    Session[SessionSystemName.SYS_MS_CORP_NAME_TH] = user.MS_CORP_NAME_TH;
                //    Session[SessionSystemName.SYS_MS_CORP_NAME_EN] = user.MS_CORP_NAME_EN;

                //    Session[SessionSystemName.SYS_MS_CORP_GROUP] = user.MS_CORP_GROUP;

                //    //Response.Redirect(ResolveUrl("~/Home/Default.aspx"));
                //    status = true;

                //}
            }
            catch (Exception ex) // เก็บ log user ที่ login
            {
                status = false;
                //log.Error("LoginINS UserLogin function:", ex);

            }
            return status;
        }
        private bool ValidateUser(UserModel user)
        {
            DateTime dtNow = DateTime.Now;
            bool valid = true;
            try
            {
                ////ยังไม่ได้รับการพิจารณาอนุมัติใช้งาน
                //if (user.APPROVE_STATUS != null)
                //{
                //    if (user.APPROVE_STATUS != "10040003")
                //    {
                //        valid = false;
                //        InsertAuthenlogs(user.USER_ID, "1", true, "Login ด้วย User ที่ยังไม่ได้รับการพิจารณาอนุมัติใช้งาน", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "user01");
                //        ModelState.AddModelError("user01", Translation.SEC.SEC_SECM00500.USER_NOT_ACTIVE);
                //    }
                //}

                ////ตรวจสอบล็อกพาสเวิด
                //if (user.IS_DISABLED.AsString() == "Y")
                //{
                //    valid = false;
                //    InsertAuthenlogs(user.USER_ID, "1", true, "Login ด้วย User หรือ Password ไม่ถูกต้อง เกินจำนวนครั้งที่ระบบกำหนด รหัสผู้ใช้งานจะถูกล๊อค", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "user02");
                //    ModelState.AddModelError("user02", Translation.SEC.SEC_SECM00500.USER_LOCK);
                //}

                ////วันที่เริ่มใช้งาน
                //if (user.USER_EFF_DATE != null)
                //{
                //    if (user.USER_EFF_DATE > dtNow)
                //    {
                //        valid = false;
                //        InsertAuthenlogs(user.USER_ID, "1", true, "Login ด้วย User ที่ยังได้รับการพิจารณาอนุมัติใช้งานแล้ว แต่ทำการ Loginก่อนวันที่เริ่มใช้งาน", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "WRN025");
                //        ModelState.AddModelError("user03", Translation.SEC.SEC_SECM00500.USER_NOT_ALLOWED_CURRENTLY);
                //    }
                //}
                ////วันหมดอายุ
                //if (user.USER_EXP_DATE != null)
                //{
                //    if (user.USER_EXP_DATE <= dtNow)
                //    {
                //        valid = false;
                //        InsertAuthenlogs(user.USER_ID, "1", true, "Login ด้วย User ที่บัญชีผู้ใช้หมดอายุ", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "WRN026");
                //        ModelState.AddModelError("user04", Translation.SEC.SEC_SECM00500.USER_IS_EXPIRED);
                //    }
                //}
                ////วันที่พาสเวิคหมดอายุ
                //if (user.PWD_EXP_DATE != null)
                //{
                //    if (user.PWD_EXP_DATE <= dtNow)
                //    {
                //        valid = false;
                //        InsertAuthenlogs(user.USER_ID, "1", true, "Login ด้วย User ที่รหัสผ่านของบัญชีผู้ใช้หมดอายุ", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "WRN028");
                //        ModelState.AddModelError("user05", Translation.SEC.SEC_SECM00500.USER_PWD_IS_EXPIRED);
                //    }
                //}
                ////สถานะการใช้งาน Y:ใช้งาน   N:ไม่ใช้งาน
                //if (user.USER_STATUS.AsString() == "N")
                //{
                //    valid = false;
                //    InsertAuthenlogs(user.USER_ID, "1", false, "บัญชีผู้ใช้ถูกยกเลิกการใช้งาน กรุณาติดต่อผู้ดูแลระบบ คปภ.", Request.Url.AbsoluteUri, true, user.LOGIN_ID, "WRN023");
                //    ModelState.AddModelError("user06", Translation.SEC.SEC_SECM00500.USER_STATUS_NO_USE);

                //}

                ////บริษัทยังไม่ได้เปิดใช้งาน
                //if (user.CORP_ACTIVE.AsString() != "Y")
                //{
                //    valid = false;
                //    InsertAuthenlogs(user.USER_ID, "1", false, Translation.SEC.SEC_SECM00500.CORP_NOT_ACTIVE, Request.Url.AbsoluteUri, true, user.LOGIN_ID, "WRN024");
                //    ModelState.AddModelError("user07", Translation.SEC.SEC_SECM00500.CORP_NOT_ACTIVE);
                //}
            }
            catch (Exception ex)
            {
                valid = false;
            }
            return valid;
        }

        private string GetConfigFile(string ServerDBName)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DBConnnectionConfig>>(System.IO.File.ReadAllText(Server.MapPath("~/App_Data/DBConnection.json")));
            return json.Where(m => m.Name == ServerDBName).Select(m => m.ConnectionString).FirstOrDefault();
        }

        private bool IsIpBlocked(string ip)
        {
            //IPAddress ipAddress;

            //if (IPAddress.TryParse(ip, out ipAddress))
            //{
            //    var ddlCT = new DataAccess.Users.BlockedIp.SEC_BlockedIpDA();
            //    ddlCT.DTO.Execute.ExecuteType = DataAccess.Users.BlockedIp.BlockedIpExecuteType.GetCheckBlockedIP;
            //    ddlCT.DTO.ParameterAdd("pIP_ADDRESS", ip);
            //    ddlCT.DTO.ParameterAdd("pBanedLoginIPCount", Convert.ToInt32(ConfigurationManager.AppSettings["BanedLoginIPCount"]));
            //    ddlCT.DTO.ParameterAdd("pBanedLoginIPTime", Convert.ToInt32(ConfigurationManager.AppSettings["BanedLoginIPTime"]));
            //    ddlCT.DTO.ParameterAdd("pBanedIPDuration", Convert.ToInt32(ConfigurationManager.AppSettings["BanedIPDuration"]));
            //    ddlCT.DTO.ParameterAdd("pSYS_COM_CODE", Convert.ToString(ConfigurationManager.AppSettings["SysApplication"]));
            //    ddlCT.Select(ddlCT.DTO);

            //    if (ddlCT.DTO.Result.ActionResult != 0)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
        private void InsertAuthenlogs(string userName, string actionLog, bool IsSuccess, string description, string URL_ADDR, bool isUser, string LOGIN_ID, string ERROR_CODE)
        {
            //DataAccess.SEC.SEC_AuthenLog.SEC_AuthenLogDA AuthenLogDA = new DataAccess.SEC.SEC_AuthenLog.SEC_AuthenLogDA();
            //DataAccess.SEC.SEC_AuthenLog.SEC_AuthenLogModel AuthenLogModel = new DataAccess.SEC.SEC_AuthenLog.SEC_AuthenLogModel();

            //AuthenLogModel.USER_ID = userName;
            //AuthenLogModel.OPERATION = Convert.ToDecimal(actionLog);
            //AuthenLogModel.IP_ADDRESS = GetUserIP();
            //AuthenLogModel.COM_CODE = Convert.ToString(ConfigurationManager.AppSettings["SysApplication"]);
            //AuthenLogModel.LOG_DT = DateTime.Now;
            //AuthenLogModel.LOGIN_SUCCESS = IsSuccess ? 1 : 0;
            //AuthenLogModel.DESCRIPTION = description;
            //AuthenLogModel.URL_ADDR = URL_ADDR;
            //AuthenLogModel.CREATE_DT = DateTime.Now;
            //AuthenLogModel.CREATE_USER = userName;
            //AuthenLogModel.ACTIVE = "Y";
            //AuthenLogModel.STATUS = "Y";
            //AuthenLogModel.LOGIN_ID = LOGIN_ID;
            //AuthenLogModel.ERROR_CODE = ERROR_CODE;

            //AuthenLogModel.IS_COUNT = isCount ? "Y" : "N";
            //AuthenLogModel.ERROR_CODE = ERROR_CODE;


            //AuthenLogDA.DTO.AuthenLog = AuthenLogModel;

            //AuthenLogDA.Insert(AuthenLogDA.DTO);
        }


        private SECS02P002Model GET_VSMS_USER(string USER_ID)
        {
            var da = new SECS02P002DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P002ExecuteType.GetByID;
            da.DTO.Model.USER_ID = USER_ID;

            da.Select(da.DTO);

            return da.DTO.Model;
        }
        private SECS02P001Model GET_VSMS_USRGROUP(string COM_CODE, decimal? USG_ID)
        {
            var da = new SECS02P001DA();
            SetStandardErrorLog(da.DTO);
            da.DTO.Execute.ExecuteType = SECS02P001ExecuteType.GetByID;
            da.DTO.Model.COM_CODE = COM_CODE;
            da.DTO.Model.USG_ID = USG_ID;

            da.Select(da.DTO);

            return da.DTO.Model;
        }

        private LogInResult CheckUserLogInForWindowAuthen(string USER_ID)
        {
            LogInResult enmLogInResult = LogInResult.Success;

            var da = new UserDA();
            da.DTO.Execute.ExecuteType = UserExecuteType.GetUser;
            da.DTO.Model.COM_CODE = "AAT"; // ไม่ได้ใช้
            da.DTO.Model.USER_ID = USER_ID; //ส่งไปใช้แค่ตัวนี้อย่างเดียว
            da.Select(da.DTO);

            if (da.DTO.Result.IsResult)
            {
                Session[SessionSystemName.SYS_COM_CODE] = da.DTO.Model.COM_CODE;
                Session[SessionSystemName.SYS_USER_ID] = da.DTO.Model.USER_ID;
                Session[SessionSystemName.SYS_USER_FNAME_TH] = da.DTO.Model.USER_FNAME_TH;
                Session[SessionSystemName.SYS_USER_FNAME_EN] = da.DTO.Model.USER_FNAME_EN;
                Session[SessionSystemName.SYS_USER_LNAME_TH] = da.DTO.Model.USER_LNAME_TH;
                Session[SessionSystemName.SYS_USER_LNAME_EN] = da.DTO.Model.USER_LNAME_EN;
                Session[SessionSystemName.SYS_USG_ID] = da.DTO.Model.USG_ID;
                Session[SessionSystemName.SYS_COM_NAME_TH] = da.DTO.Model.COM_NAME_T;
                Session[SessionSystemName.SYS_COM_NAME_EN] = da.DTO.Model.COM_NAME_E;
                Session[SessionSystemName.SYS_USG_LEVEL] = da.DTO.Model.USG_LEVEL;
            }
            else
            {
                enmLogInResult = LogInResult.UserGroupNotAuthorized;
            }

            return enmLogInResult;
        }
        #endregion
    }
}