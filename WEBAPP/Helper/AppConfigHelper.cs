using System.Collections.Generic;
using System.Configuration;

namespace WEBAPP.Helper
{
    public static class AppConfigHelper
    {
        public static string TemplatePath
        {
            get
            {
                return ConfigurationManager.AppSettings["templatePath"];
            }
        }
        public static string UploadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["uploadPath"];
            }
        }
        public static string TempPath
        {
            get
            {
                return ConfigurationManager.AppSettings["tempPath"];
            }
        }

        public static string ReportServerName
        {
            get
            {
                return ConfigurationManager.AppSettings["REPORT_SERVER_NAME"];
            }
        }
        public static string ReportFolderName
        {
            get
            {
                return ConfigurationManager.AppSettings["REPORT_FOLDER_NAME"];
            }
        }

        public static string IS_ENCRYPTED_CONFIG
        {
            get
            {
                return ConfigurationManager.AppSettings["IS_ENCRYPTED_CONFIG"];
            }
        }
        public static string FTP_BON_BK_PATH
        {
            get
            {
                return ConfigurationManager.AppSettings["FTP_BON_BK_PATH"];
            }
        }
        public static string FTP_BON_TEMP_PATH
        {
            get
            {
                return ConfigurationManager.AppSettings["FTP_BON_TEMP_PATH"];
            }
        }

        public static string MonthEndFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["MonthEndFilePath"];
            }
        }
    }
}
