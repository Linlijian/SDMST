using DataAccess;
using Microsoft.Reporting.WebForms;
using System;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using UtilityLib;
using WEBAPP.Helper;

namespace WEBAPP
{
    public class BaseReport : System.Web.UI.Page
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HtmlIframe _frmPrint;
        private HtmlIframe frmPrint
        {
            get { return _frmPrint ?? (_frmPrint = (HtmlIframe)Master.FindControl("frmPrint")); }
        }

        protected virtual void CreatePDF(LocalReport report)
        {
            //LocalReport report = new LocalReport();
            //report.ReportPath = Server.MapPath(_ReportPath);
            //report.Refresh();

            //report.SetParameters(lstRptParam);
            //report.DataSources.Clear();
            //report.DataSources.Add(new ReportDataSource("DemoReport", BindData()));

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = report.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);
            var tempPath = WebConfigurationManager.AppSettings["tempPath"];
            string pathOutput = tempPath + SessionHelper.SYS_USER_ID + SessionHelper.SYS_CurrentPRG_CODE + "_Print.pdf";
            string fPathOutput = Server.MapPath(pathOutput);
            FileStream fs = new FileStream(fPathOutput, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            frmPrint.Attributes["src"] = ResolveUrl(pathOutput);
        }
        
        public virtual void Page_Error(object sender, EventArgs e)
        {
            string messageerror = Server.GetLastError().Message.ToString();
            string errorpath = Request.Url.AbsolutePath;

            Server.ClearError();

            logger.Error(Environment.NewLine + "Path : " + errorpath + Environment.NewLine + "Error Messages : " + messageerror + Environment.NewLine);

            Session[SessionSystemName.SYS_ErrorPath] = errorpath;
            Session[SessionSystemName.SYS_ErrorMessage] = messageerror;
            Session[SessionSystemName.SYS_ErrorCode] = 500;

            Response.Redirect("~/Users/Account/Error?exception=&errorcode=500");
        }
        protected BaseDTO SetStandardLog(BaseDTO dto)
        {
            var log = new TransactionLogModel();
            
            log.SYS_CODE = SessionHelper.SYS_CurrentSYS_CODE;
            log.PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
            log.LOG_HEADER = SessionHelper.SYS_CurrentAction;
            log.ACTIVITY_TYPE = 10074007;
            log.IP_ADDRESS = GetUserIP();
            log.DoInsertLog = true;

            SetStandardField(log);

            dto.TransactionLog = log;
            return dto;
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

        protected T SetStandardField<T>(T model) where T : class, new()
        {
            foreach (var prop in model.GetType().GetProperties().Where(m =>
            m.Name.Equals("CREATE_USER") ||
            m.Name.Equals("CREATE_DT") ||
            m.Name.Equals("CREATED_USER") ||
            m.Name.Equals("CREATED_DT") ||
            m.Name.Equals("UPDATE_USER") ||
            m.Name.Equals("UPDATE_DT")))
            {
                var propertyInfo = model.GetType().GetProperty(prop.Name);
                //if (prop.Name == "COM_CODE")
                //    propertyInfo.SetValue(model, SessionHelper.SYS_COM_CODE, null);

                if (prop.Name == "CREATE_USER")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "CREATE_DT")
                    propertyInfo.SetValue(model, DateTime.Now, null);

                if (prop.Name == "CREATED_USER")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "CREATED_DT")
                    propertyInfo.SetValue(model, DateTime.Now, null);

                if (prop.Name == "UPDATE_USER")
                    propertyInfo.SetValue(model, SessionHelper.SYS_USER_ID, null);

                if (prop.Name == "UPDATE_DT")
                    propertyInfo.SetValue(model, DateTime.Now, null);
            }
            return model;
        }
    }
}