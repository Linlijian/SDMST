using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web;
using UtilityLib;

namespace WEBAPP.Helper
{
    public class PDFHelper
    {
        public static List<FileUpload> CreatePDFToFileUpload(string ReportPath, List<ReportParameter> lstRptParam, List<ReportDataSource> reportDataSource
            , string PRG_CODE, decimal? DOCUMENT_TYPE_ID, decimal? SECTION_GROUP_ID, decimal? COVER_SHEET_SEND_ID, decimal? HEADER_INPUT_ID)
        {
            LocalReport report = new LocalReport();
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            FileUpload File = new FileUpload();
            List<FileUpload> FileList = new List<FileUpload>();

            report.ReportPath = HttpContext.Current.Server.MapPath(ReportPath);
            report.Refresh();

            report.SetParameters(lstRptParam);
            report.DataSources.Clear();
            foreach (var item in reportDataSource)
            {
                report.DataSources.Add(item);
            }

            byte[] bytes = report.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            File.PRG_CODE = PRG_CODE;
            File.DOCUMENT_TYPE_ID = DOCUMENT_TYPE_ID;
            File.SECTION_GROUP_ID = SECTION_GROUP_ID;
            File.COVER_SHEET_SEND_ID = COVER_SHEET_SEND_ID;
            File.HEADER_INPUT_ID = HEADER_INPUT_ID;

            File.COM_CODE = SessionHelper.SYS_COM_CODE;
            File.FILE_NAME = DOCUMENT_TYPE_ID.AsString() + SECTION_GROUP_ID.AsString() + COVER_SHEET_SEND_ID.AsString() + HEADER_INPUT_ID.AsString() + ".pdf";
            File.FILE_SIZE = Math.Round(bytes.Length * (Math.Pow(10, -6)), 4).AsDecimal();
            File.FILE_DATE = DateTime.Now;
            File.CREATE_DT = DateTime.Now;
            File.BLOB_FILE = bytes;
            File.BLOB_FILE_HASH = GetFileHash(bytes);

            FileList.Add(File);

            return FileList;
        }


        private static string GetFileHash(byte[] bfile)
        {
            var sha = new System.Security.Cryptography.SHA1Managed();
            byte[] hashbfile = sha.ComputeHash(bfile);
            return Convert.ToBase64String(hashbfile);
        }
    }
}