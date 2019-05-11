using System;
using System.Collections.Generic;
using System.Web;
using UtilityLib;

namespace WEBAPP.Helper
{
    public class SessionHelper
    {
        public static string SYS_COM_CODE
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_COM_CODE]);
            }
        }
        public static string SYS_APP_CODE
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_APPS]);
            }
        }
        public static string SYS_USER_ID
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_ID]);
            }
        }
        public static int SYS_USG_ID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session[SessionSystemName.SYS_USG_ID]);
            }
        }
        public static string SYS_USER_FNAME_TH
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_FNAME_TH]);
            }
        }
        public static string SYS_USER_FNAME_EN
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_FNAME_EN]);
            }
        }
        public static string SYS_USER_LNAME_TH
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_LNAME_TH]);
            }
        }
        public static string SYS_USER_LNAME_EN
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_LNAME_EN]);
            }
        }
        public static string SYS_USER_NAME_TH
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_FNAME_TH]) + " " + Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_LNAME_TH]);
            }
        }
        public static string SYS_USER_NAME_EN
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_FNAME_EN]) + " " + Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USER_LNAME_EN]);
            }
        }
        public static string SYS_USG_LEVEL
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_USG_LEVEL]);
            }
        }

        public static string SYS_SID
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_SID]);
            }
        }
        public static string SYS_SYS_GROUP_NAME
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_SYS_GROUP_NAME]);
            }
        }
        public static List<DataAccess.SEC.MenuModel> SYS_MenuModel
        {
            get
            {
                if ((HttpContext.Current.Session[SessionSystemName.SYS_MENU] == null || ((List<DataAccess.SEC.MenuModel>)HttpContext.Current.Session[SessionSystemName.SYS_MENU]).Count == 0) &&
                    !SYS_USG_ID.IsNullOrEmpty() &&
                    !SYS_SYS_GROUP_NAME.IsNullOrEmpty())
                {
                    var da = new DataAccess.SEC.SECBaseDA();
                    da.DTO.Execute.ExecuteType = DataAccess.SEC.SECBaseExecuteType.GetMenu;
                    da.DTO.Menu.COM_CODE = SYS_COM_CODE;
                    da.DTO.Menu.USG_ID = SYS_USG_ID;
                    da.DTO.Menu.SYS_GROUP_NAME = SYS_SYS_GROUP_NAME;
                    da.Select(da.DTO);
                    HttpContext.Current.Session[SessionSystemName.SYS_MENU] = da.DTO.Menus;
                }
                return HttpContext.Current.Session[SessionSystemName.SYS_MENU] as List<DataAccess.SEC.MenuModel>;
            }
        }

        public static string SYS_COM_NAME
        {
            get
            {
                var appName = Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_COM_NAME_TH]);
                var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                if (culture.Name == "en-US" && !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_COM_NAME_EN])))
                {
                    appName = Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_COM_NAME_EN]);
                }
                return appName;
            }
        }

        public static DataAccess.SEC.MenuModel SYS_CurrentPrgConfig
        {
            get
            {
                return (DataAccess.SEC.MenuModel)HttpContext.Current.Session[SessionSystemName.SYS_CurrentPrgConfig];
            }
        }
        public static string SYS_CurrentPRG_CODE
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentPRG_CODE]);
            }
        }
        public static string SYS_CurrentSYS_CODE
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentSYS_CODE]);
            }
        }
        public static string SYS_CurrentArea
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentArea]);
            }
        }
        public static string SYS_CurrentController
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentController]);
            }
        }
        public static string SYS_CurrentAction
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentAction]);
            }
        }
        public static string SYS_CurrentAreaController
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_CurrentAreaController]);
            }
        }
        public static string SYS_CurrentCulture
        {
            get
            {
                return HttpContext.Current.Session[SessionSystemName.SYS_CurrentCulture].AsString();
            }
        }

        public static bool SYS_IsMultipleGroup
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Current.Session[SessionSystemName.SYS_IsMultipleGroup]);
            }
            set
            {
                HttpContext.Current.Session[SessionSystemName.SYS_IsMultipleGroup] = value;
            }
        }

        public static string SYS_ServerDBName
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.SYS_ServerDBName]);
            }
        }

        public static List<DataAccess.Users.AppModel> SYS_AppModel
        {
            get
            {
                if (HttpContext.Current.Session[SessionSystemName.SYS_APPS] == null || ((List<DataAccess.Users.AppModel>)HttpContext.Current.Session[SessionSystemName.SYS_APPS]).Count == 0)
                {
                    var da = new DataAccess.Users.UserDA();
                    da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.GetApp;
                    da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
                    da.SelectNoEF(da.DTO);
                    HttpContext.Current.Session[SessionSystemName.SYS_APPS] = da.DTO.Apps;
                }
                return HttpContext.Current.Session[SessionSystemName.SYS_APPS] as List<DataAccess.Users.AppModel>;
            }
        }

        public static List<DataAccess.Users.NotificationModel> Notification
        {
            get
            {
                //if (HttpContext.Current.Session[SessionSystemName.Notification] == null || ((List<DataAccess.Users.NotificationModel>)HttpContext.Current.Session[SessionSystemName.Notification]).Count == 0)
                //{
                //    var da = new DataAccess.Users.UserDA();
                //    da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.GetNotification;
                //    da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
                //    da.SelectNoEF(da.DTO);
                //    HttpContext.Current.Session[SessionSystemName.Notification] = da.DTO.Notification;
                //}
                var da = new DataAccess.Users.UserDA();
                da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.GetNotification;
                da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
                da.SelectNoEF(da.DTO);
                HttpContext.Current.Session[SessionSystemName.Notification] = da.DTO.Notifications;
                return HttpContext.Current.Session[SessionSystemName.Notification] as List<DataAccess.Users.NotificationModel>;
            }
            set { }
        }

        public static string CountNoti
        {
            get
            {
                var da = new DataAccess.Users.UserDA();
                da.DTO.Execute.ExecuteType = DataAccess.Users.UserExecuteType.GetNotificationCount;
                da.DTO.Model.USER_ID = SessionHelper.SYS_USER_ID;
                da.SelectNoEF(da.DTO);
                HttpContext.Current.Session[SessionSystemName.CountNoti] = da.DTO.Notification.NO;
                return Convert.ToString(HttpContext.Current.Session[SessionSystemName.CountNoti]);
            }
        }
    }

    public class SessionSystemName
    {
        public const string Notification = "Notification";
        public const string CountNoti = "CountNoti";

        public const string SYS_APPS = "SYS_APPS";
        public const string SYS_COM_CODE = "SYS_COM_CODE";
        public const string SYS_USER_ID = "SYS_USER_ID";
        public const string SYS_USG_ID = "SYS_USG_ID";
        public const string SYS_USER_FNAME_TH = "SYS_USER_FNAME_TH";
        public const string SYS_USER_FNAME_EN = "SYS_USER_FNAME_EN";
        public const string SYS_USER_LNAME_TH = "SYS_USER_LNAME_TH";
        public const string SYS_USER_LNAME_EN = "SYS_USER_LNAME_EN";
        public const string SYS_MENU = "SYS_MENU";
        public const string SYS_COM_NAME_TH = "SYS_COM_NAME_TH";
        public const string SYS_COM_NAME_EN = "SYS_COM_NAME_EN";
        public const string SYS_Company = "SYS_Company";
        public const string SYS_CurrentPrgConfig = "SYS_CurrentPrgConfig";
        public const string SYS_CurrentPRG_CODE = "SYS_CurrentPRG_CODE";
        public const string SYS_CurrentSYS_CODE = "SYS_CurrentSYS_CODE";
        public const string SYS_CurrentArea = "SYS_CurrentArea";
        public const string SYS_CurrentController = "SYS_CurrentController";
        public const string SYS_CurrentAction = "SYS_CurrentAction";
        public const string SYS_SID = "SYS_SID";
        public const string SYS_CurrentAreaController = "SYS_CurrentAreaController";
        public const string SYS_USG_LEVEL = "SYS_USG_LEVEL";
        public const string SYS_SYS_GROUP_NAME = "SYS_SYS_GROUP_NAME";
        public const string SYS_IsMultipleGroup = "SYS_IsMultipleGroup";
        public const string SYS_ServerDBName = "SYS_ServerDBName";
        public const string SYS_SYSTEM = "SYS_SYSTEM";
        public const string SYS_MODULE = "SYS_MODULE";

        public const string SYS_CurrentCulture = "SYS_CurrentCulture";

        public const string SYS_ErrorMessage = "SYS_ErrorMessage";
        public const string SYS_ErrorPath = "SYS_ErrorPath";
        public const string SYS_ErrorCode = "SYS_ErrorCode";

        public const string FileModel = "FileModel";
    }
}
