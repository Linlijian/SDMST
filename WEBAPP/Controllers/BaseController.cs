using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using UtilityLib;
using WEBAPP.Helper;
using WEBAPP.Models;
using System.Configuration;
using DataAccess;

namespace WEBAPP.Controllers
{
    /// <summary>
    /// Code By Jubpas
    /// </summary>
    public class BaseController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger("RootLogger");
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //TODO Logic Permission-based access      
            //filterContext.HttpContext.Response.Redirect("http://google.co.th", true);

            var currentArea = filterContext.RouteData.DataTokens["area"].AsString();
            var currentController = filterContext.RouteData.GetRequiredString("controller");
            var currentAction = filterContext.RouteData.GetRequiredString("action");
            var BlockedUrlByPermission = Convert.ToBoolean(ConfigurationManager.AppSettings["BlockedUrlByPermission"]);

            var menu = (List<DataAccess.SEC.MenuModel>)filterContext.HttpContext.Session[SessionSystemName.SYS_MENU];
            var existsMenu = 0;
            if (menu != null)
            {
                existsMenu = menu.Where(m => m.PRG_AREA.AsString().Equals(currentArea) && m.PRG_CONTROLLER.AsString().Equals(currentController)).Count();
            }
            var isAuth = (filterContext.HttpContext.User != null) && filterContext.HttpContext.User.Identity.IsAuthenticated;
            var allowUrl = true;
            if (BlockedUrlByPermission &&
                existsMenu <= 0 &&
                !currentArea.Equals("Ux") &&
                !(currentArea.Equals("Admin") && currentController.Equals("Default")) &&
                !(currentArea.Equals("SEC") && currentController.Equals("SEC_Contact")) &&
                !(currentArea.Equals("GEN") && currentController.Equals("GEN_GEND00400")) &&
                !(currentArea.Equals("SEC") && currentController.Equals("SEC_SECM00500") && (currentAction.Equals("Info") || currentAction.Equals("GetUserApp"))) &&
                !(currentArea.Equals("SEC") && currentController.Equals("SEC_SECM00501") && (currentAction.Equals("Info") || currentAction.Equals("BindDataSelectPrivEdit") || currentAction.Equals("GetFileInfo"))))
            {
                allowUrl = false;
            }
            if (isAuth && filterContext.HttpContext.Session[SessionSystemName.SYS_USER_ID] != null && allowUrl)
            {
                filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentArea] = currentArea;
                filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentController] = currentController;
                filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentAction] = currentAction;
                filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentAreaController] = currentArea + currentController;

                if (Request["SYS_SYS_CODE"] != null && Request["SYS_PRG_CODE"] != null)
                {
                    var currentSysCode = Request["SYS_SYS_CODE"] ?? string.Empty;
                    var currentPrgCode = Request["SYS_PRG_CODE"] ?? string.Empty;
                    filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentSYS_CODE] = currentSysCode;
                    filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentPRG_CODE] = currentPrgCode;
                    filterContext.HttpContext.Session[SessionSystemName.SYS_CurrentPrgConfig] = menu.Where(m => m.SYS_CODE.Equals(currentSysCode) && m.PRG_CODE.Equals(currentPrgCode)).FirstOrDefault();
                    var breadcrumb = new Breadcrumb
                    {
                        SYS_CODE = currentSysCode,
                        PRG_CODE = currentPrgCode
                    };

                    if (menu != null && menu.Count > 0)
                    {
                        var menuModel = menu.Where(x =>
                        x.PRG_CODE.AsString().Equals(currentPrgCode.Trim()) &&
                        x.SYS_CODE.AsString().Equals(currentSysCode.Trim()) &&
                        x.PRG_AREA.AsString().Equals(currentArea.Trim()) &&
                        x.PRG_CONTROLLER.AsString().Equals(currentController.Trim()))
                                        .FirstOrDefault();

                        if (menuModel != null)
                        {
                            breadcrumb.SystemName = menuModel.SYS_NAME_TH;
                            breadcrumb.ProgramName = menuModel.PRG_NAME_TH;
                            //breadcrumb.ProgramUrl = "/" + menu?[0].PRG_AREA + "/" + menu?[0].PRG_CONTROLLER;
                            breadcrumb.Area = menuModel.PRG_AREA;
                            breadcrumb.Controller = menuModel.PRG_CONTROLLER;
                            breadcrumb.Action = menuModel.PRG_ACTION;
                        }
                        TempData[Breadcrumb.TempDataKey] = breadcrumb;
                        TempData.Keep(Breadcrumb.TempDataKey);
                    }
                }
                else
                {
                    if (TempData.ContainsKey(Breadcrumb.TempDataKey))
                    {
                        TempData.Keep(Breadcrumb.TempDataKey);
                    }
                }


                if (Request.UrlReferrer != null &&
                    currentAction != StandardActionName.Clear &&
                    currentAction != StandardWizardButtonName.Reset &&
                    currentAction != "LangUi" &&
                    Request.Url.AbsolutePath != Request.UrlReferrer.AbsolutePath &&
                    !filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    TempData["UrlReferrer"] = Request.UrlReferrer;
                }
                TempData.Keep("UrlReferrer");
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    if (isAuth && filterContext.HttpContext.Session[SessionSystemName.SYS_USER_ID] != null && !allowUrl)
                    {
                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                        filterContext.Result = new JsonResult
                        {
                            Data = new Models.BaseAjaxResult { Status = false, Mode = "Access denied" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        filterContext.HttpContext.Response.StatusCode = 440;
                        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                        filterContext.Result = new JsonResult
                        {
                            Data = new Models.BaseAjaxResult { Status = false, Mode = "Time Out" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }
                else
                {
                    if (isAuth && filterContext.HttpContext.Session[SessionSystemName.SYS_USER_ID] != null && !allowUrl)
                    {
                        filterContext.Result = RedirectToAction("Error", "Account", new { Area = "Users", exception = Url.Encode("Access denied"), errorcode = 401 });
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            controller = "Account",
                            action = "SignOut",
                            area = "Users"
                        }));
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if ((Request.IsAjaxRequest() && (!Request.GetRequest("button").IsNullOrEmpty() || SessionHelper.SYS_CurrentAction.Contains("Delete"))) || !Request.IsAjaxRequest())
            {
                //var da = new DataAccess.SEC.SECBaseDA();
                //SetStandardLog(da.DTO);
                //da.Insert(da.DTO);
            }
            //TODO Logic
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var currentArea = filterContext.RouteData.DataTokens["area"].AsString();
            var currentController = filterContext.RouteData.GetRequiredString("controller");
            var currentAction = filterContext.RouteData.GetRequiredString("action");

            if (filterContext.Exception is HttpAntiForgeryException)
            {
                filterContext.HttpContext.Response.StatusCode = 440;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.Result = new JsonResult
                {
                    Data = new Models.BaseAjaxResult { Status = false, Mode = "Time Out", Url = Url.Action("SignOut", "Account", new { Area = "Users" }) },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            filterContext.HttpContext.Session[SessionSystemName.SYS_ErrorPath] = currentArea + "/" + currentController + "/" + currentAction;
            filterContext.HttpContext.Session[SessionSystemName.SYS_ErrorMessage] = filterContext.Exception.Message;
            filterContext.HttpContext.Session[SessionSystemName.SYS_ErrorCode] = 500;

            logger.Error(Environment.NewLine + "Path : " + currentArea + "/" + currentController + "/" + currentAction + Environment.NewLine + "Error Messages: " + filterContext.Exception.Message + Environment.NewLine);
            base.OnException(filterContext);
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            //custom authentication logic
        }

        protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //custom authentication challenge logic
        }

        protected override void ExecuteCore()
        {
            string UIculture = "en-US";
            if (this.Session == null || this.Session[SessionSystemName.SYS_CurrentCulture] == null)
            {

                var httpSessionStateBase = this.Session;
                if (httpSessionStateBase != null) httpSessionStateBase[SessionSystemName.SYS_CurrentCulture] = UIculture;
            }
            else
            {
                UIculture = (string)this.Session[SessionSystemName.SYS_CurrentCulture];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = UIculture;

            base.ExecuteCore();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            string UIculture = "en-US";
            if (requestContext.HttpContext.Session == null || requestContext.HttpContext.Session[SessionSystemName.SYS_CurrentCulture] == null)
            {
                var httpSessionStateBase = requestContext.HttpContext.Session;
                if (httpSessionStateBase != null) httpSessionStateBase[SessionSystemName.SYS_CurrentCulture] = UIculture;
            }
            else
            {
                UIculture = (string)requestContext.HttpContext.Session[SessionSystemName.SYS_CurrentCulture];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = UIculture;
            base.Initialize(requestContext);
        }

        protected void SetDefaulButton(StandardButtonMode mode, string formName = "form1", bool IsValidate = false, bool requiredCer = false, bool overrideSubmit = false, bool showToolTip = false)
        {
            TempData[StandardButton.TempDataKey] = new List<StandardButton>(); //Clear Before Add
            if (mode == StandardButtonMode.Index)
            {
                AddStandardButton(StandardButtonName.Search, formName, isValidate: IsValidate, overrideSubmit: overrideSubmit,showToolTip: showToolTip);
                AddStandardButton(StandardButtonName.Add, formName, showToolTip: showToolTip);
                AddStandardButton(StandardButtonName.DeleteSearch, formName, isValidate: IsValidate, overrideSubmit: overrideSubmit, showToolTip: showToolTip);
            }
            else if (mode == StandardButtonMode.Create)
            {
                AddStandardButton(StandardButtonName.SaveCreate, formName, requiredCer: requiredCer, isValidate: true, overrideSubmit: overrideSubmit, showToolTip: showToolTip);
            }
            else if (mode == StandardButtonMode.Modify)
            {
                AddStandardButton(StandardButtonName.SaveModify, formName, requiredCer: requiredCer, isValidate: true, overrideSubmit: overrideSubmit, showToolTip: showToolTip);
            }

            if (mode != StandardButtonMode.View)
            {
                AddStandardButton(StandardButtonName.Clear, formName, showToolTip: showToolTip);
            }
        }

        protected void AddStandardButton(
            string name,
            string formName = "form1",
            int index = -1,
            bool requiredCer = false,
            bool isValidate = false,
            bool overrideSubmit = false,
            string url = "",
            bool showToolTip = false)
        {
            if (url.IsNullOrEmpty())
            {
                if (name == StandardButtonName.Clear)
                {
                    var action = RouteData.GetRequiredString("action");
                    var routes = new RouteValueDictionary();
                    routes.Add("refAction", action);
                    foreach (var key in Request.QueryString.AllKeys)
                    {
                        routes.Add(key, Request.GetRequest(key));
                    }
                    url = Url.Action(name, routes);
                }
                else if (name == StandardButtonName.LoadFile)
                {
                    url = "javascript:void(0)";
                }
                else if (name != StandardButtonName.DownloadTemplate)
                {
                    url = Url.Action(name);
                }
            }
            if (name == StandardButtonName.Search)
            {
                AddButtonToolbar(StandButtonType.ButtonAjax, name, formName, null, (showToolTip ? "" : Translation.CenterLang.Center.Search), "std-btn-search", FaIcons.FaSearch, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.Add)
            {
                AddButtonToolbar(StandButtonType.Button, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.Add), "btn-add", FaIcons.FaPlusCircle, TextColor.green, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.DeleteSearch)
            {
                AddButtonToolbar(StandButtonType.ButtonComfirmAjax, name, formName, null, (showToolTip ? "" : Translation.CenterLang.Center.Delete), "std-btn-delete", FaIcons.FaTrash, TextColor.red, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.Save || name == StandardButtonName.SaveCreate || name == StandardButtonName.SaveModify)
            {
                AddButtonToolbar(StandButtonType.ButtonComfirmAjax, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.Save), "std-btn-confirm", FaIcons.FaSave, index: index, requiredCer: requiredCer, isValidate: isValidate, overrideSubmit: overrideSubmit);
            }
            if (name == StandardButtonName.Upload)
            {
                AddButtonToolbar(StandButtonType.Button, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.Upload), "btn-upload", FaIcons.FaArrowCircleUp, TextColor.green, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.Download)
            {
                AddButtonToolbar(StandButtonType.ButtonNewWindows, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.Download), "btn-download", FaIcons.FaArrowCircleDown, TextColor.light_green, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.ExportExcel)
            {
                AddButtonToolbar(StandButtonType.ButtonNewWindows, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.ExportExcel), "btn-exportexcel", FaIcons.FaFileExcelO, TextColor.green, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.LoadFile)
            {
                AddButtonToolbar(StandButtonType.Button, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.LoadFile), "btn-loadfile", FaIcons.FaCaretSquareOUp, TextColor.blue, index: index, isValidate: isValidate);
            }
            if (name == StandardButtonName.DownloadTemplate)
            {
                if (!url.IsNullOrEmpty())
                {
                    var newUrl = Url.Action(name, new { Key = url });
                    AddButtonToolbar(StandButtonType.ButtonNewWindows, name, formName, newUrl, (showToolTip ? "" : Translation.CenterLang.Center.DownloadTemplate), "btn-downloadtemplate", FaIcons.FaCaretSquareODown, TextColor.blue, index: index, isValidate: isValidate);
                }
            }
            if (name == StandardButtonName.Clear)
            {
                AddButtonToolbar(StandButtonType.ButtonComfirm, name, formName, url, (showToolTip ? "" : Translation.CenterLang.Center.Reset), "std-btn-clear", FaIcons.FaRefresh, TextColor.red2);
            }
        }

        protected void RemoveStandardButton(string buttonName, params string[] buttonNames)
        {
            var standardButton = TempData.ContainsKey(StandardButton.TempDataKey)
                ? (List<StandardButton>)TempData[StandardButton.TempDataKey]
                : new List<StandardButton>();

            if (standardButton.Count > 0)
            {
                var button = (from d in standardButton
                              where d.Name == buttonName
                              select d).FirstOrDefault();
                if (button != null)
                {
                    standardButton.Remove(button);
                }
                if (buttonNames != null)
                {
                    foreach (var item in buttonNames)
                    {
                        var btn = (from d in standardButton
                                   where d.Name == item
                                   select d).FirstOrDefault();
                        if (btn != null)
                        {
                            standardButton.Remove(btn);
                        }
                    }
                }
            }
        }

        protected void AddButton(
            StandButtonType type,
            string name,
            string buttontext = "",
            string cssClass = "",
            string iconCssClass = "",
            TextColor iconColor = TextColor.None,
            StandardIconPosition iconPosition = StandardIconPosition.BeforeText,
            string url = "",
            string formName = "form1",
            bool isUseTable = false,
            int index = -1,
            bool requiredCer = false,
            bool isValidate = false,
            bool overrideSubmit = false,
            string toolTipText = "")
        {
            AddButtonToolbar(type, name, formName, url, buttontext, cssClass, iconCssClass, iconColor, iconPosition, index, requiredCer, isValidate, overrideSubmit, toolTipText);
        }

        private void AddButtonToolbar(
            StandButtonType type,
            string name,
            string formName,
            string url,
            string buttonText = "",
            string cssClass = "",
            string iconCssClass = "",
            TextColor iconColor = TextColor.None,
            StandardIconPosition iconPosition = StandardIconPosition.BeforeText,
            int index = -1,
            bool requiredCer = false,
            bool isValidate = false,
            bool overrideSubmit = false,
            string toolTipText = "")
        {
            var standardButton = TempData.ContainsKey(StandardButton.TempDataKey)
                ? (List<StandardButton>)TempData[StandardButton.TempDataKey]
                : new List<StandardButton>();

            if (index < 0)
            {
                if (standardButton.Exists(m => m.Name == StandardButtonName.Clear))
                {
                    standardButton.Insert(standardButton.Count - 1, new StandardButton
                    {
                        Type = type,
                        Name = name,
                        FormName = formName,
                        Url = url,
                        Text = buttonText,// string.IsNullOrEmpty(buttonText) ? name : buttonText,
                        CssClass = cssClass,
                        IconCssClass = iconCssClass,
                        IconColor = iconColor != TextColor.None ? iconColor.GetDescription() : null,
                        IconPosition = iconPosition,
                        RequiredCer = requiredCer,
                        IsValidate = isValidate,
                        OverrideSubmit = overrideSubmit,
                        ToolTipText = toolTipText//string.IsNullOrEmpty(toolTipText) ? name : toolTipText
                    });
                }
                else
                {
                    standardButton.Add(new StandardButton
                    {
                        Type = type,
                        Name = name,
                        FormName = formName,
                        Url = url,
                        Text = buttonText,// string.IsNullOrEmpty(buttonText) ? name : buttonText,
                        CssClass = cssClass,
                        IconCssClass = iconCssClass,
                        IconColor = iconColor != TextColor.None ? iconColor.GetDescription() : null,
                        IconPosition = iconPosition,
                        RequiredCer = requiredCer,
                        IsValidate = isValidate,
                        OverrideSubmit = overrideSubmit,
                        ToolTipText = toolTipText//string.IsNullOrEmpty(toolTipText) ? name : toolTipText
                    });
                }
            }
            else
            {
                standardButton.Insert(index, new StandardButton
                {
                    Type = type,
                    Name = name,
                    FormName = formName,
                    Url = url,
                    Text = buttonText,// string.IsNullOrEmpty(buttonText) ? name : buttonText,
                    CssClass = cssClass,
                    IconCssClass = iconCssClass,
                    IconColor = iconColor != TextColor.None ? iconColor.GetDescription() : null,
                    RequiredCer = requiredCer,
                    IsValidate = isValidate,
                    IconPosition = iconPosition,
                    OverrideSubmit = overrideSubmit,
                    ToolTipText = toolTipText//string.IsNullOrEmpty(toolTipText) ? name : toolTipText
                });
            }

            TempData[StandardButton.TempDataKey] = standardButton;
        }

        public void AddButtonToolbarOperateData(ButtonConfig btnConfig, params ButtonConfig[] tbButtons)
        {
            var tbButton = TempData.ContainsKey("Temp")
                ? (List<ButtonConfig>)TempData[StandardTempKey.OperateData]
                : new List<ButtonConfig>();
            tbButton.Add(btnConfig);
            if (tbButtons != null)
            {
                tbButton.AddRange(tbButtons);
            }

            TempData[StandardTempKey.OperateData] = tbButton;
        }

        protected T SetStandardField<T>(T model) where T : class, new()
        {
            foreach (var prop in model.GetType().GetProperties().Where(m =>
            m.Name.Equals("COM_CODE") ||
            m.Name.Equals("CRET_BY") ||
            m.Name.Equals("CRET_DATE") ||
            m.Name.Equals("MNT_BY") ||
            m.Name.Equals("MNT_DATE")))
            {
                var propertyInfo = model.GetType().GetProperty(prop.Name);
                if (prop.Name == "COM_CODE")
                    propertyInfo.SetValue(model, SessionHelper.SYS_COM_CODE, null);

                if (prop.Name == "CRET_BY")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "CRET_DATE")
                    propertyInfo.SetValue(model, DateTime.Now, null);

                if (prop.Name == "MNT_BY")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "MNT_DATE")
                    propertyInfo.SetValue(model, DateTime.Now, null);
            }
            return model;
        }

        protected T SetStandardFieldWithoutComCode<T>(T model) where T : class, new()
        {
            foreach (var prop in model.GetType().GetProperties().Where(m =>
            m.Name.Equals("CRET_BY") ||
            m.Name.Equals("CRET_DATE") ||
            m.Name.Equals("MNT_BY") ||
            m.Name.Equals("MNT_DATE")))
            {
                var propertyInfo = model.GetType().GetProperty(prop.Name);

                if (prop.Name == "CRET_BY")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "CRET_DATE")
                    propertyInfo.SetValue(model, DateTime.Now, null);

                if (prop.Name == "MNT_BY")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "MNT_DATE")
                    propertyInfo.SetValue(model, DateTime.Now, null);
            }
            return model;
        }

        protected BaseDTO SetStandardErrorLog(BaseDTO dto)
        {
            var log = new TransactionLogModel();

            log.SYS_CODE = SessionHelper.SYS_CurrentSYS_CODE;
            log.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            log.LOG_HEADER = SessionHelper.SYS_CurrentAction;
            log.ACTIVITY_TYPE = GetActivityType();
            log.IP_ADDRESS = GetUserIP();
            log.DoInsertLog = false;

            SetStandardField(log);

            dto.TransactionLog = log;
            return dto;
        }

        protected BaseDTO SetStandardLog(BaseDTO dto)
        {
            var log = new TransactionLogModel();

            log.SYS_CODE = SessionHelper.SYS_CurrentSYS_CODE;
            log.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            log.LOG_HEADER = SessionHelper.SYS_CurrentAction;
            log.ACTIVITY_TYPE = GetActivityType();
            log.IP_ADDRESS = GetUserIP();
            log.DoInsertLog = true;

            SetStandardField(log);

            dto.TransactionLog = log;
            return dto;
        }

        protected BaseDTO SetStandardLog(BaseDTO dto, object objectValue, SaveLogConfig config, params SaveLogConfig[] configs)
        {
            var log = new TransactionLogModel();

            log.SYS_CODE = SessionHelper.SYS_CurrentSYS_CODE;
            log.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            log.LOG_HEADER = SessionHelper.SYS_CurrentAction;

            log.ACTIVITY_TYPE = GetActivityType();
            log.IP_ADDRESS = GetUserIP();
            log.DoInsertLog = true;

            log.SaveLogConfig = new List<SaveLogConfig>();
            log.SaveLogConfig.Add(config);
            log.SaveLogConfig.AddRange(configs);
            log.ObjectValue = objectValue;

            SetStandardField(log);
            log.COM_CODE = SessionHelper.SYS_APP_CODE;

            dto.TransactionLog = log;
            return dto;
        }

        private Dictionary<string, object> DicActivityType = new Dictionary<string, object> {
            {"Search",10074001 },
            {"SaveCreate",10074002 },
            {"SaveModify",10074003 },
            {"Delete",10074004 },
            {"Info",10074005 },
            {"Process",10074006 },
            {"Report",10074007 },
            {"Upload",10074008 }
        };

        private decimal? GetActivityType()
        {
            var action = SessionHelper.SYS_CurrentAction;
            if (SessionHelper.SYS_CurrentAction.Contains("Delete"))
            {
                action = SessionHelper.SYS_CurrentAction.Substring(0, 6);
            }
            decimal? data = 10074000;
            if (DicActivityType.ContainsKey(action))
            {
                data = DicActivityType[action].AsDecimalNull();
            }
            return data;
        }
        protected SaveLogConfig GetSaveLogConfig(string schema, string tableName, string pkColumnName, params string[] pkColumns)
        {
            var col = new LogColumnConfig
            {
                PKColumnName = pkColumnName
            };
            List<LogColumnConfig> cols = null;
            if (pkColumns != null)
            {
                cols = new List<LogColumnConfig>();
                foreach (var pkColumn in pkColumns)
                {
                    cols.Add(new LogColumnConfig
                    {
                        PKColumnName = pkColumn
                    });
                }
            }
            return GetSaveLogConfig(schema, tableName, col, cols.ToArray());
        }
        protected SaveLogConfig GetSaveLogConfig(string schema, string tableName, LogColumnConfig columnConfig, params string[] pkColumns)
        {
            List<LogColumnConfig> cols = null;
            if (pkColumns != null)
            {
                cols = new List<LogColumnConfig>();
                foreach (var pkColumn in pkColumns)
                {
                    cols.Add(new LogColumnConfig
                    {
                        PKColumnName = pkColumn
                    });
                }
            }
            return GetSaveLogConfig(schema, tableName, columnConfig, cols.ToArray());
        }
        protected SaveLogConfig GetSaveLogConfig(string schema, string tableName, LogColumnConfig columnConfig, params LogColumnConfig[] columnConfigs)
        {
            var log = new SaveLogConfig();
            log.Schema = schema;
            log.TableName = tableName;
            log.Columns = new List<LogColumnConfig>();
            log.Columns.Add(columnConfig);
            if (columnConfigs != null)
            {
                log.Columns.AddRange(columnConfigs);
            }
            return log;
        }
        protected LogColumnConfig GetColumnChar(string columName, int charLength = -1)
        {
            return new LogColumnConfig(columName, true, charLength);
        }
        protected IEnumerable<ValidationError> FindErrors(ModelStateDictionary modelState)
        {
            var result = new List<ValidationError>();
            var erroneousFields = modelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
            foreach (var erroneousField in erroneousFields)
            {
                var fieldKey = erroneousField.Key;
                var fieldErrors = erroneousField.Errors.Select(error => new ValidationError(fieldKey, error.ErrorMessage));
                result.AddRange(fieldErrors);
            }
            return result;
        }
        protected IEnumerable<ValidationError> FindErrors(IList<FluentValidation.Results.ValidationFailure> errors)
        {
            return errors.Select(m => new ValidationError { Key = m.PropertyName, Message = m.ErrorMessage });
        }
        protected RouteValueDictionary GetRoute(bool isAddQueryStringbtnClear = true)
        {
            var action = RouteData.GetRequiredString("action");
            var routeValue = new RouteValueDictionary();
            if (Request.QueryString.Count > 0)
            {
                foreach (var item in Request.QueryString.AllKeys.Where(m => m != "page"))
                {
                    if (action == StandardActionName.Clear || action == StandardWizardButtonName.Reset)
                    {
                        if (item != "refAction" && isAddQueryStringbtnClear)
                        {
                            routeValue.Add(item, Request.QueryString[item]);
                        }
                    }
                    else
                    {
                        routeValue.Add(item, Request.QueryString[item]);
                    }
                }
            }
            return routeValue;
        }
        protected List<DDLCenterModel> GetDDLCenter(string KEY_ID, params VSMParameter[] param)
        {
            return GetDDLCenter(KEY_ID, "", "", param);
        }
        protected List<DDLCenterModel> GetDDLCenter(string KEY_ID, string whereClause = "", string orderBy = "", params VSMParameter[] param)
        {
            var ddl = new DDLCenterDA();
            ddl.DTO.Parameter.KEY_ID = KEY_ID;
            ddl.DTO.Parameter.WhereClause = whereClause;
            ddl.DTO.Parameter.OrderBy = orderBy;
            if (param != null)
            {
                ddl.DTO.Parameter.ParameterValues = string.Join(",", param.Select(m => m.Value));
            }
            ddl.SelectNoEF(ddl.DTO);
            return ddl.DTO.DDLCenters;
        }
        protected JsonResult Success(DTOResult result, string mode, bool isConfirm, params string[] redirectToUrls)
        {
            return Success(result, new ResultOptions
            {
                Mode = mode,
                ExistsChioce = isConfirm
            }, redirectToUrls);
        }
        protected JsonResult Success(DTOResult result, string mode, params string[] redirectToUrls)
        {
            return Success(result, new ResultOptions
            {
                Mode = mode
            }, redirectToUrls);
        }
        protected JsonResult Success(DTOResult result, ResultOptions option, params string[] redirectToUrls)
        {
            var ajRes = new AjaxResult();
            ajRes.Mode = option.Mode;
            ajRes.Status = result.IsResult;
            ajRes.button = Request.GetRequest("button");
            if (result.IsResult)
            {
                ajRes.Style = AlertStyles.Success;
                if (option.Mode == StandardActionName.Delete)
                {
                    /*
                     * ชื่อปุ่มต้องมี Length > 6 และตามด้วย ชื่อGrid เสมอเพื่อนำกลับไป Refresh grid
                     * เช่น buttonGridName จะตัดคำว่า button ทิ้ง
                     * */
                    ajRes.Message = Translation.CenterLang.Center.DeleteCompleted;
                    if (!option.GridName.IsNullOrEmpty())
                    {
                        ajRes.button = option.GridName;
                    }
                    else if (ajRes.button.Length > 6)
                    {
                        ajRes.button = ajRes.button.Substring(6, ajRes.button.Length - 6);
                    }
                }
                else
                {
                    ajRes.Message = Translation.CenterLang.Center.SaveCompleted;
                }
                if (!option.SuccessMessage.IsNullOrEmpty())
                {
                    ajRes.Message = option.SuccessMessage;
                }

                if (redirectToUrls != null && redirectToUrls.Count() > 0)
                {
                    ajRes.RedirectToUrl = new List<string>();
                    ajRes.RedirectToUrl.AddRange(redirectToUrls);
                }
                ajRes.ExistsChioce = option.ExistsChioce;
            }
            else
            {
                ajRes.Style = AlertStyles.Error;
                if (option.Mode == StandardActionName.Delete)
                {
                    ajRes.Message = Translation.CenterLang.Center.DeleteNotComplete;
                }
                else
                {
                    ajRes.Message = Translation.CenterLang.Center.SaveNotComplete;
                }
                if (!option.ErrorMessage.IsNullOrEmpty())
                {
                    ajRes.Message = option.ErrorMessage;
                }
                ajRes.Errors = new List<ValidationError> {
                    new ValidationError(result.ResultCode,result.ResultMsg)
                };
            }

            return Json(ajRes);
        }
        protected JsonResult ValidateError(ModelStateDictionary modelState, string mode)
        {
            var ajRes = new AjaxResult();
            ajRes.Mode = mode;
            ajRes.Status = false;
            ajRes.Style = AlertStyles.Error;
            ajRes.Message = Translation.CenterLang.Center.ValidateError;
            ajRes.Errors = FindErrors(modelState);
            return Json(ajRes);
        }
        protected JsonResult ValidateError(string mode, params ValidationError[] validMessage)
        {
            var ajRes = new AjaxResult();
            ajRes.Mode = mode;
            ajRes.Status = false;
            ajRes.Style = AlertStyles.Error;
            ajRes.Message = Translation.CenterLang.Center.ValidateError;
            if (validMessage != null)
            {
                var err = new List<ValidationError>();
                err.AddRange(validMessage);
                ajRes.Errors = err;
            }
            return Json(ajRes);
        }
        protected JsonResult JsonAllowGet<T>(List<T> data, DTOResult result, int recordsTotal, int draw) where T : class, new()
        {
            var ajgRes = new AjaxGridResult();
            ajgRes.Status = true;
            if (data == null || !result.IsResult)
            {
                data = new List<T>();
                ajgRes.Status = false;
                ajgRes.Style = AlertStyles.Error;
                ajgRes.Message = Translation.CenterLang.Center.DataNotFound;
                if (!result.IsResult)
                {
                    ajgRes.error = result.ResultMsg;
                }
            }
            ajgRes.data = data;
            ajgRes.draw = draw;
            ajgRes.recordsFiltered = recordsTotal;
            ajgRes.recordsTotal = recordsTotal;
            var json = Json(ajgRes, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        protected JsonResult JsonAllowGet<T>(List<T> data, DTOResult result) where T : class, new()
        {
            var ajgRes = new AjaxGridResult();
            ajgRes.Status = true;
            if (data == null || !result.IsResult)
            {
                data = new List<T>();
                ajgRes.Status = false;
                ajgRes.Style = AlertStyles.Error;
                ajgRes.Message = Translation.CenterLang.Center.DataNotFound;
                if (!result.IsResult)
                {
                    ajgRes.error = result.ResultMsg;
                }
            }
            ajgRes.data = data;
            var json = Json(ajgRes, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        protected JsonResult JsonAllowGet<T>(T data) where T : class, new()
        {
            var ajgRes = new AjaxGridResult();
            ajgRes.Status = true;
            if (data == null)
            {
                data = new T();
                ajgRes.Status = false;
                ajgRes.Style = AlertStyles.Error;
                ajgRes.Message = Translation.CenterLang.Center.DataNotFound;
            }
            ajgRes.data = data;
            var json = Json(ajgRes, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        protected string GetUserIP()
        {
            string VisitorsIPAddr = string.Empty;
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }
        protected string GetRootPathUrl(bool isSubLastString = true)
        {
            string returnStr = string.Format("{0}://{1}{2}{3}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port == 80 ? "" : ":" + System.Web.HttpContext.Current.Request.Url.Port, string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ApplicationPath) ? "/" : System.Web.HttpContext.Current.Request.ApplicationPath + "/");

            if (isSubLastString)
            {
                returnStr = returnStr.Substring(0, returnStr.Length - 1);
            }

            return returnStr;
        }
        public virtual ActionResult Clear(string refAction)
        {
            if (refAction == StandardActionName.Index)
            {
                TempData[StandardActionName.Search + SessionHelper.SYS_CurrentAreaController] = null;
            }

            return RedirectToAction(refAction, GetRoute());
        }
        public virtual ActionResult DownloadTemplate(string Key)
        {
            var pathConfig = AppConfigHelper.TemplatePath;
            var json = "~/App_Data/FileConfig.json".GetAllText().JsonToObject<AppDataConfig>();
            var value = json.Where(m => m.Key == Key).Select(m => m.Value).FirstOrDefault();
            if (!value.IsNullOrEmpty())
            {
                ExportHelper.ExportFile(Response, Server.MapPath(pathConfig + value));
            }

            return Content(Translation.CenterLang.Center.DownloadCompleted);
        }
        
        //Wizard
        protected void SetHeaderWizard(WizardHelper.WizardHeaderConfig wizardHeaderConfig)
        {
            TempData[WizardHelper.WizardHeaderConfig.TempDataKey] = wizardHeaderConfig;
        }

        protected void SetClientSideRuleSet(string ruleSet, params string[] ruleSets)
        {
            var lsRuleSet = new List<string>();
            lsRuleSet.Add(ruleSet);
            if (ruleSets != null)
            {
                lsRuleSet.AddRange(ruleSets);
            }
            ControllerContext.HttpContext.Items["_FV_ClientSideRuleSet"] = lsRuleSet.ToArray();
        }
    }
}