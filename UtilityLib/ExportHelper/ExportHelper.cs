using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Data;
using System.Reflection;
using System.Web;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Resources;
using System.Threading;
using System.Collections;
using System.Web.Configuration;
using OfficeOpenXml.Style;

namespace UtilityLib
{
    public static class ExportHelper
    {
        public enum XLSType
        {
            XLS, XLSX
        }

        //string FileExportName = "Export";
        //IEnumerable<T> exportDataSource = null;
        //public IEnumerable<T> ExportDataSource
        //{
        //    set { exportDataSource = value; }
        //}<TSource>(IEnumerable<TSource> data)
        public static void ExportExcel<T>(HttpResponseBase response, IEnumerable<T> objList) where T : class, new()
        {
            var obj = new T();
            var lst = obj.GetDisplayName();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Export");
                int i = 0;
                foreach (var item in lst)
                {
                    i++;
                    ws.Cells[1, i].Style.Font.Bold = true;
                    ws.Cells[1, i].Value = item;
                }
                //ws.Cells["A1"].LoadFromDataTable(dtHeader, true);
                ws.Cells["A2"].LoadFromCollection(objList, false);

                Byte[] fileBytes = pck.GetAsByteArray();
                response.Clear();
                response.Buffer = true;
                response.AddHeader("content-disposition", "attachment;filename=Export.xlsx");

                response.Charset = "";
                response.ContentType = "application/vnd.ms-excel";
                response.BinaryWrite(fileBytes);
                response.End();
            }
        }

        public static void ExportExcel<T>(HttpResponseBase response, IEnumerable<T> objList, List<string> propertyNames) where T : class, new()
        {
            var obj = new T();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Export");
                var i = 1;
                foreach (var propName in propertyNames)
                {
                    ws.Cells[1, i].Value = obj.GetDisplayName(propName);
                    ws.Cells[1, i].Style.Font.Bold = true;
                    ws.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    i++;
                }
                //ws.Cells["A1"].LoadFromDataTable(dtHeader, true);
                ws.Cells["A2"].LoadFromCollection(objList, false);

                Byte[] fileBytes = pck.GetAsByteArray();
                response.Clear();
                response.Buffer = true;
                response.AddHeader("content-disposition", "attachment;filename=Export.xlsx");

                response.Charset = "";
                response.ContentType = "application/vnd.ms-excel";
                response.BinaryWrite(fileBytes);
                response.End();
            }
        }

        public static void ExportExcel<T>(HttpResponseBase response, IEnumerable<T> objList, string FileName) where T : class, new()
        {
            var obj = new T();
            var lst = obj.GetDisplayName();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(FileName);
                int i = 0;
                foreach (var item in lst)
                {
                    i++;
                    ws.Cells[1, i].Style.Font.Bold = true;
                    ws.Cells[1, i].Value = item;
                }
                //ws.Cells["A1"].LoadFromDataTable(dtHeader, true);
                ws.Cells["A2"].LoadFromCollection(objList, false);

                Byte[] fileBytes = pck.GetAsByteArray();
                response.Clear();
                response.Buffer = true;
                response.AddHeader("content-disposition", "attachment;filename=Export.xlsx");

                response.Charset = "";
                response.ContentType = "application/vnd.ms-excel";
                response.BinaryWrite(fileBytes);
                response.End();
            }
        }
        public static List<string> GetDisplayName<TModel>(this TModel obj) where TModel : class, new()
        {
            //var obj = new TModel();
            string DisplayName = "";

            var lst = new List<string>();
            var props = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var display = prop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                if (display != null)
                {
                    var resourceManager = new ResourceManager(display.ResourceType.FullName, display.ResourceType.Assembly);
                    var entry =
                        resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
                          .OfType<DictionaryEntry>()
                          .FirstOrDefault(p => p.Key.ToString() == display.Name);
                    DisplayName = entry.Value.ToString();
                }
                else
                {
                    DisplayName = prop.Name;
                }
                lst.Add(DisplayName);
            }
            return lst;
        }
        public static string GetDisplayName<TModel>(this TModel model, string propertyName)
        {
            Type type = typeof(TModel);
            string displayValue = string.Empty;
            var display = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            if (display != null)
            {
                var resourceManager = new ResourceManager(display.ResourceType.FullName, display.ResourceType.Assembly);
                var entry =
                    resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
                      .OfType<DictionaryEntry>()
                      .FirstOrDefault(p => p.Key.ToString() == display.Name);
                displayValue = entry.Value.ToString();
            }
            else
            {
                displayValue = propertyName;
            }
            return displayValue;
        }
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {

            Type type = typeof(TModel);

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);

            // First look into attributes on a type and it's parents
            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : String.Empty;


        }
        public static string GetDisplayFormat<TModel>(this TModel model, string propertyName)
        {
            Type type = typeof(TModel);
            string formatValue = string.Empty;
            var displayFormat = (DisplayFormatAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayFormatAttribute), true).SingleOrDefault();

            if (displayFormat != null)
            {
                formatValue = displayFormat.DataFormatString;
            }
            return formatValue;
        }

        //Add by Phoochana 2012
        public static void ExportFile(HttpResponseBase Response, string filepath)
        {
            try
            {
                string filename = Path.GetFileName(filepath);
                System.IO.FileInfo file = new FileInfo(filepath);
                //file.Extension
                if (file.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", "attachment; filename=" + filename.Replace(" ", "_"));
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.AddHeader("Content-Type", ContentType(file.Extension));
                    //Response.ContentType = "application/vnd.xls";
                    //FileType.txt
                    //Response.ContentType = "application/octet-stream";

                    //Response.ContentType = ContentType(file.Extension);
                    Response.WriteFile(file.FullName);
                    // Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    //Ext.Net.X.Msg.Alert("Massage", "This file does not exist.").Show();
                    Response.Write("This file does not exist.");
                }
            }
            catch (Exception ex)
            {

                //Ext.Net.X.Msg.Alert("Massage", ex.Message).Show();
                Response.Write(ex.Message);
                // An error occurred.. 
            }
        }

        public static void ExportFile(HttpResponseBase response, string filepath, byte[] Byte)
        {
            string fileName = HttpUtility.UrlEncode(Path.GetFileName(filepath), Encoding.UTF8);
            string ext = Path.GetExtension(filepath);
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            response.AddHeader("Content-Type", ContentType(ext));
            response.ContentEncoding = Encoding.UTF8;
            //response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Transfer-Encoding", "binary");
            response.BinaryWrite(Byte);
            response.End();
        }

        public static void ExportMultiSheet(HttpResponseBase response, List<ExportDataModel> data)
        {
            try
            {
                var fileName = data.Select(m => m.FILE_NAME).Distinct().FirstOrDefault();
                if (!fileName.IsNullOrEmpty())
                {
                    ExportMultiSheet(response, data, fileName);
                }
                else
                {
                    response.Write("Can not FileName");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void ExportMultiSheet(HttpResponseBase response, List<ExportDataModel> data, string templateFileName)
        {
            try
            {
                var pathTempate = WebConfigurationManager.AppSettings["templatePath"];
                var fullPathTempate = HttpContext.Current.Server.MapPath(pathTempate + templateFileName);
                byte[] fbyte = File.ReadAllBytes(fullPathTempate);
                MemoryStream msExcel = new MemoryStream(fbyte);
                fbyte = null;
                using (var xlPackage = new ExcelPackage(msExcel))
                {
                    // xlPackage = new ExcelPackage(ExcelFile);
                    ExcelRange excelRange;
                    ExcelWorksheet xWorksheet;

                    var sheets = data.Select(m => m.REPORT_CODE).Distinct();
                    foreach (string sheetName in sheets)
                    {
                        if (!sheetName.IsNullOrEmpty())
                        {
                            xWorksheet = xlPackage.Workbook.Worksheets[sheetName.AsString()];
                            if (xWorksheet != null)
                            {
                                var dataSheet = data.Where(m => m.REPORT_CODE == sheetName);
                                foreach (var item in dataSheet)
                                {
                                    excelRange = xWorksheet.Cells[item.EXCEL_XY.AsString()];
                                    if (!item.INPUT_VALUE.IsNullOrEmpty())
                                    {
                                        switch (item.DATA_TYPE.AsString().ToLower())
                                        {
                                            case "number":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "#,##0.00_);[Red](#,##0.00)"; //"#,##0.00";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            case "integer":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "#,##0_);[Red](#,##0)";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            case "percentage":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "0.00%";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            //case "date":
                                            //    try
                                            //    {
                                            //        excelRange.Value = Extension.AsDateTimeNull(dataExport.INPUT_VALUE);
                                            //        excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                            //        excelRange.Style.Numberformat.Format =
                                            //            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                            //    }
                                            //    catch (Exception)
                                            //    {
                                            //        excelRange.Value = dataExport.INPUT_VALUE;
                                            //        //result = false;
                                            //    }
                                            //    break;
                                            default:
                                                if (item.INPUT_VALUE.Contains('\t'))
                                                {
                                                    var format = new OfficeOpenXml.ExcelTextFormat();
                                                    format.Delimiter = '\t';
                                                    format.TextQualifier = '"';
                                                    format.DataTypes = new[] { eDataTypes.String };
                                                    excelRange.LoadFromText(item.INPUT_VALUE, format);
                                                }
                                                else
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ExportFile(response, templateFileName, xlPackage.GetAsByteArray());
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static string ContentType(string fileextension)
        {
            IDictionary<string, string> _mappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            #region Big freaking list of mime types
            // combination of values from Windows 7 Registry and 
            // from C:\Windows\System32\inetsrv\config\applicationHost.config
            // some added, including .7z and .dat
            {".323", "text/h323"},
            {".3g2", "video/3gpp2"},
            {".3gp", "video/3gpp"},
            {".3gp2", "video/3gpp2"},
            {".3gpp", "video/3gpp"},
            {".7z", "application/x-7z-compressed"},
            {".aa", "audio/audible"},
            {".AAC", "audio/aac"},
            {".aaf", "application/octet-stream"},
            {".aax", "audio/vnd.audible.aax"},
            {".ac3", "audio/ac3"},
            {".aca", "application/octet-stream"},
            {".accda", "application/msaccess.addin"},
            {".accdb", "application/msaccess"},
            {".accdc", "application/msaccess.cab"},
            {".accde", "application/msaccess"},
            {".accdr", "application/msaccess.runtime"},
            {".accdt", "application/msaccess"},
            {".accdw", "application/msaccess.webapplication"},
            {".accft", "application/msaccess.ftemplate"},
            {".acx", "application/internet-property-stream"},
            {".AddIn", "text/xml"},
            {".ade", "application/msaccess"},
            {".adobebridge", "application/x-bridge-url"},
            {".adp", "application/msaccess"},
            {".ADT", "audio/vnd.dlna.adts"},
            {".ADTS", "audio/aac"},
            {".afm", "application/octet-stream"},
            {".ai", "application/postscript"},
            {".aif", "audio/x-aiff"},
            {".aifc", "audio/aiff"},
            {".aiff", "audio/aiff"},
            {".air", "application/vnd.adobe.air-application-installer-package+zip"},
            {".amc", "application/x-mpeg"},
            {".application", "application/x-ms-application"},
            {".art", "image/x-jg"},
            {".asa", "application/xml"},
            {".asax", "application/xml"},
            {".ascx", "application/xml"},
            {".asd", "application/octet-stream"},
            {".asf", "video/x-ms-asf"},
            {".ashx", "application/xml"},
            {".asi", "application/octet-stream"},
            {".asm", "text/plain"},
            {".asmx", "application/xml"},
            {".aspx", "application/xml"},
            {".asr", "video/x-ms-asf"},
            {".asx", "video/x-ms-asf"},
            {".atom", "application/atom+xml"},
            {".au", "audio/basic"},
            {".avi", "video/x-msvideo"},
            {".axs", "application/olescript"},
            {".bas", "text/plain"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".c", "text/plain"},
            {".cab", "application/octet-stream"},
            {".caf", "audio/x-caf"},
            {".calx", "application/vnd.ms-office.calx"},
            {".cat", "application/vnd.ms-pki.seccat"},
            {".cc", "text/plain"},
            {".cd", "text/plain"},
            {".cdda", "audio/aiff"},
            {".cdf", "application/x-cdf"},
            {".cer", "application/x-x509-ca-cert"},
            {".chm", "application/octet-stream"},
            {".class", "application/x-java-applet"},
            {".clp", "application/x-msclip"},
            {".cmx", "image/x-cmx"},
            {".cnf", "text/plain"},
            {".cod", "image/cis-cod"},
            {".config", "application/xml"},
            {".contact", "text/x-ms-contact"},
            {".coverage", "application/xml"},
            {".cpio", "application/x-cpio"},
            {".cpp", "text/plain"},
            {".crd", "application/x-mscardfile"},
            {".crl", "application/pkix-crl"},
            {".crt", "application/x-x509-ca-cert"},
            {".cs", "text/plain"},
            {".csdproj", "text/plain"},
            {".csh", "application/x-csh"},
            {".csproj", "text/plain"},
            {".css", "text/css"},
            {".csv", "text/csv"},
            {".cur", "application/octet-stream"},
            {".cxx", "text/plain"},
            {".dat", "application/octet-stream"},
            {".datasource", "application/xml"},
            {".dbproj", "text/plain"},
            {".dcr", "application/x-director"},
            {".def", "text/plain"},
            {".deploy", "application/octet-stream"},
            {".der", "application/x-x509-ca-cert"},
            {".dgml", "application/xml"},
            {".dib", "image/bmp"},
            {".dif", "video/x-dv"},
            {".dir", "application/x-director"},
            {".disco", "text/xml"},
            {".dll", "application/x-msdownload"},
            {".dll.config", "text/xml"},
            {".dlm", "text/dlm"},
            {".doc", "application/msword"},
            {".docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".dot", "application/msword"},
            {".dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {".dsp", "application/octet-stream"},
            {".dsw", "text/plain"},
            {".dtd", "text/xml"},
            {".dtsConfig", "text/xml"},
            {".dv", "video/x-dv"},
            {".dvi", "application/x-dvi"},
            {".dwf", "drawing/x-dwf"},
            {".dwp", "application/octet-stream"},
            {".dxr", "application/x-director"},
            {".eml", "message/rfc822"},
            {".emz", "application/octet-stream"},
            {".eot", "application/octet-stream"},
            {".eps", "application/postscript"},
            {".etl", "application/etl"},
            {".etx", "text/x-setext"},
            {".evy", "application/envoy"},
            {".exe", "application/octet-stream"},
            {".exe.config", "text/xml"},
            {".fdf", "application/vnd.fdf"},
            {".fif", "application/fractals"},
            {".filters", "Application/xml"},
            {".fla", "application/octet-stream"},
            {".flr", "x-world/x-vrml"},
            {".flv", "video/x-flv"},
            {".fsscript", "application/fsharp-script"},
            {".fsx", "application/fsharp-script"},
            {".generictest", "application/xml"},
            {".gif", "image/gif"},
            {".group", "text/x-ms-group"},
            {".gsm", "audio/x-gsm"},
            {".gtar", "application/x-gtar"},
            {".gz", "application/x-gzip"},
            {".h", "text/plain"},
            {".hdf", "application/x-hdf"},
            {".hdml", "text/x-hdml"},
            {".hhc", "application/x-oleobject"},
            {".hhk", "application/octet-stream"},
            {".hhp", "application/octet-stream"},
            {".hlp", "application/winhlp"},
            {".hpp", "text/plain"},
            {".hqx", "application/mac-binhex40"},
            {".hta", "application/hta"},
            {".htc", "text/x-component"},
            {".htm", "text/html"},
            {".html", "text/html"},
            {".htt", "text/webviewhtml"},
            {".hxa", "application/xml"},
            {".hxc", "application/xml"},
            {".hxd", "application/octet-stream"},
            {".hxe", "application/xml"},
            {".hxf", "application/xml"},
            {".hxh", "application/octet-stream"},
            {".hxi", "application/octet-stream"},
            {".hxk", "application/xml"},
            {".hxq", "application/octet-stream"},
            {".hxr", "application/octet-stream"},
            {".hxs", "application/octet-stream"},
            {".hxt", "text/html"},
            {".hxv", "application/xml"},
            {".hxw", "application/octet-stream"},
            {".hxx", "text/plain"},
            {".i", "text/plain"},
            {".ico", "image/x-icon"},
            {".ics", "application/octet-stream"},
            {".idl", "text/plain"},
            {".ief", "image/ief"},
            {".iii", "application/x-iphone"},
            {".inc", "text/plain"},
            {".inf", "application/octet-stream"},
            {".inl", "text/plain"},
            {".ins", "application/x-internet-signup"},
            {".ipa", "application/x-itunes-ipa"},
            {".ipg", "application/x-itunes-ipg"},
            {".ipproj", "text/plain"},
            {".ipsw", "application/x-itunes-ipsw"},
            {".iqy", "text/x-ms-iqy"},
            {".isp", "application/x-internet-signup"},
            {".ite", "application/x-itunes-ite"},
            {".itlp", "application/x-itunes-itlp"},
            {".itms", "application/x-itunes-itms"},
            {".itpc", "application/x-itunes-itpc"},
            {".IVF", "video/x-ivf"},
            {".jar", "application/java-archive"},
            {".java", "application/octet-stream"},
            {".jck", "application/liquidmotion"},
            {".jcz", "application/liquidmotion"},
            {".jfif", "image/pjpeg"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".jpb", "application/octet-stream"},
            {".jpe", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".js", "application/x-javascript"},
            {".jsx", "text/jscript"},
            {".jsxbin", "text/plain"},
            {".latex", "application/x-latex"},
            {".library-ms", "application/windows-library+xml"},
            {".lit", "application/x-ms-reader"},
            {".loadtest", "application/xml"},
            {".lpk", "application/octet-stream"},
            {".lsf", "video/x-la-asf"},
            {".lst", "text/plain"},
            {".lsx", "video/x-la-asf"},
            {".lzh", "application/octet-stream"},
            {".m13", "application/x-msmediaview"},
            {".m14", "application/x-msmediaview"},
            {".m1v", "video/mpeg"},
            {".m2t", "video/vnd.dlna.mpeg-tts"},
            {".m2ts", "video/vnd.dlna.mpeg-tts"},
            {".m2v", "video/mpeg"},
            {".m3u", "audio/x-mpegurl"},
            {".m3u8", "audio/x-mpegurl"},
            {".m4a", "audio/m4a"},
            {".m4b", "audio/m4b"},
            {".m4p", "audio/m4p"},
            {".m4r", "audio/x-m4r"},
            {".m4v", "video/x-m4v"},
            {".mac", "image/x-macpaint"},
            {".mak", "text/plain"},
            {".man", "application/x-troff-man"},
            {".manifest", "application/x-ms-manifest"},
            {".map", "text/plain"},
            {".master", "application/xml"},
            {".mda", "application/msaccess"},
            {".mdb", "application/x-msaccess"},
            {".mde", "application/msaccess"},
            {".mdp", "application/octet-stream"},
            {".me", "application/x-troff-me"},
            {".mfp", "application/x-shockwave-flash"},
            {".mht", "message/rfc822"},
            {".mhtml", "message/rfc822"},
            {".mid", "audio/mid"},
            {".midi", "audio/mid"},
            {".mix", "application/octet-stream"},
            {".mk", "text/plain"},
            {".mmf", "application/x-smaf"},
            {".mno", "text/xml"},
            {".mny", "application/x-msmoney"},
            {".mod", "video/mpeg"},
            {".mov", "video/quicktime"},
            {".movie", "video/x-sgi-movie"},
            {".mp2", "video/mpeg"},
            {".mp2v", "video/mpeg"},
            {".mp3", "audio/mpeg"},
            {".mp4", "video/mp4"},
            {".mp4v", "video/mp4"},
            {".mpa", "video/mpeg"},
            {".mpe", "video/mpeg"},
            {".mpeg", "video/mpeg"},
            {".mpf", "application/vnd.ms-mediapackage"},
            {".mpg", "video/mpeg"},
            {".mpp", "application/vnd.ms-project"},
            {".mpv2", "video/mpeg"},
            {".mqv", "video/quicktime"},
            {".ms", "application/x-troff-ms"},
            {".msi", "application/octet-stream"},
            {".mso", "application/octet-stream"},
            {".mts", "video/vnd.dlna.mpeg-tts"},
            {".mtx", "application/xml"},
            {".mvb", "application/x-msmediaview"},
            {".mvc", "application/x-miva-compiled"},
            {".mxp", "application/x-mmxp"},
            {".nc", "application/x-netcdf"},
            {".nsc", "video/x-ms-asf"},
            {".nws", "message/rfc822"},
            {".ocx", "application/octet-stream"},
            {".oda", "application/oda"},
            {".odc", "text/x-ms-odc"},
            {".odh", "text/plain"},
            {".odl", "text/plain"},
            {".odp", "application/vnd.oasis.opendocument.presentation"},
            {".ods", "application/oleobject"},
            {".odt", "application/vnd.oasis.opendocument.text"},
            {".one", "application/onenote"},
            {".onea", "application/onenote"},
            {".onepkg", "application/onenote"},
            {".onetmp", "application/onenote"},
            {".onetoc", "application/onenote"},
            {".onetoc2", "application/onenote"},
            {".orderedtest", "application/xml"},
            {".osdx", "application/opensearchdescription+xml"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p7c", "application/pkcs7-mime"},
            {".p7m", "application/pkcs7-mime"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7s", "application/pkcs7-signature"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcast", "application/x-podcast"},
            {".pct", "image/pict"},
            {".pcx", "application/octet-stream"},
            {".pcz", "application/octet-stream"},
            {".pdf", "application/pdf"},
            {".pfb", "application/octet-stream"},
            {".pfm", "application/octet-stream"},
            {".pfx", "application/x-pkcs12"},
            {".pgm", "image/x-portable-graymap"},
            {".pic", "image/pict"},
            {".pict", "image/pict"},
            {".pkgdef", "text/plain"},
            {".pkgundef", "text/plain"},
            {".pko", "application/vnd.ms-pki.pko"},
            {".pls", "audio/scpls"},
            {".pma", "application/x-perfmon"},
            {".pmc", "application/x-perfmon"},
            {".pml", "application/x-perfmon"},
            {".pmr", "application/x-perfmon"},
            {".pmw", "application/x-perfmon"},
            {".png", "image/png"},
            {".pnm", "image/x-portable-anymap"},
            {".pnt", "image/x-macpaint"},
            {".pntg", "image/x-macpaint"},
            {".pnz", "image/png"},
            {".pot", "application/vnd.ms-powerpoint"},
            {".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {".potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {".ppa", "application/vnd.ms-powerpoint"},
            {".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {".ppm", "image/x-portable-pixmap"},
            {".pps", "application/vnd.ms-powerpoint"},
            {".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {".prf", "application/pics-rules"},
            {".prm", "application/octet-stream"},
            {".prx", "application/octet-stream"},
            {".ps", "application/postscript"},
            {".psc1", "application/PowerShell"},
            {".psd", "application/octet-stream"},
            {".psess", "application/xml"},
            {".psm", "application/octet-stream"},
            {".psp", "application/octet-stream"},
            {".pub", "application/x-mspublisher"},
            {".pwz", "application/vnd.ms-powerpoint"},
            {".qht", "text/x-html-insertion"},
            {".qhtm", "text/x-html-insertion"},
            {".qt", "video/quicktime"},
            {".qti", "image/x-quicktime"},
            {".qtif", "image/x-quicktime"},
            {".qtl", "application/x-quicktimeplayer"},
            {".qxd", "application/octet-stream"},
            {".ra", "audio/x-pn-realaudio"},
            {".ram", "audio/x-pn-realaudio"},
            {".rar", "application/octet-stream"},
            {".ras", "image/x-cmu-raster"},
            {".rat", "application/rat-file"},
            {".rc", "text/plain"},
            {".rc2", "text/plain"},
            {".rct", "text/plain"},
            {".rdlc", "application/xml"},
            {".resx", "application/xml"},
            {".rf", "image/vnd.rn-realflash"},
            {".rgb", "image/x-rgb"},
            {".rgs", "text/plain"},
            {".rm", "application/vnd.rn-realmedia"},
            {".rmi", "audio/mid"},
            {".rmp", "application/vnd.rn-rn_music_package"},
            {".roff", "application/x-troff"},
            {".rpm", "audio/x-pn-realaudio-plugin"},
            {".rqy", "text/x-ms-rqy"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".ruleset", "application/xml"},
            {".s", "text/plain"},
            {".safariextz", "application/x-safari-safariextz"},
            {".scd", "application/x-msschedule"},
            {".sct", "text/scriptlet"},
            {".sd2", "audio/x-sd2"},
            {".sdp", "application/sdp"},
            {".sea", "application/octet-stream"},
            {".searchConnector-ms", "application/windows-search-connector+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".settings", "application/xml"},
            {".sgimb", "application/x-sgimb"},
            {".sgml", "text/sgml"},
            {".sh", "application/x-sh"},
            {".shar", "application/x-shar"},
            {".shtml", "text/html"},
            {".sit", "application/x-stuffit"},
            {".sitemap", "application/xml"},
            {".skin", "application/xml"},
            {".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {".slk", "application/vnd.ms-excel"},
            {".sln", "text/plain"},
            {".slupkg-ms", "application/x-ms-license"},
            {".smd", "audio/x-smd"},
            {".smi", "application/octet-stream"},
            {".smx", "audio/x-smd"},
            {".smz", "audio/x-smd"},
            {".snd", "audio/basic"},
            {".snippet", "application/xml"},
            {".snp", "application/octet-stream"},
            {".sol", "text/plain"},
            {".sor", "text/plain"},
            {".spc", "application/x-pkcs7-certificates"},
            {".spl", "application/futuresplash"},
            {".src", "application/x-wais-source"},
            {".srf", "text/plain"},
            {".SSISDeploymentManifest", "text/xml"},
            {".ssm", "application/streamingmedia"},
            {".sst", "application/vnd.ms-pki.certstore"},
            {".stl", "application/vnd.ms-pki.stl"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".svc", "application/xml"},
            {".swf", "application/x-shockwave-flash"},
            {".t", "application/x-troff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".testrunconfig", "application/xml"},
            {".testsettings", "application/xml"},
            {".tex", "application/x-tex"},
            {".texi", "application/x-texinfo"},
            {".texinfo", "application/x-texinfo"},
            {".tgz", "application/x-compressed"},
            {".thmx", "application/vnd.ms-officetheme"},
            {".thn", "application/octet-stream"},
            {".tif", "image/tiff"},
            {".tiff", "image/tiff"},
            {".tlh", "text/plain"},
            {".tli", "text/plain"},
            {".toc", "application/octet-stream"},
            {".tr", "application/x-troff"},
            {".trm", "application/x-msterminal"},
            {".trx", "application/xml"},
            {".ts", "video/vnd.dlna.mpeg-tts"},
            {".tsv", "text/tab-separated-values"},
            {".ttf", "application/octet-stream"},
            {".tts", "video/vnd.dlna.mpeg-tts"},
            {".txt", "text/plain"},
            {".u32", "application/octet-stream"},
            {".uls", "text/iuls"},
            {".user", "text/plain"},
            {".ustar", "application/x-ustar"},
            {".vb", "text/plain"},
            {".vbdproj", "text/plain"},
            {".vbk", "video/mpeg"},
            {".vbproj", "text/plain"},
            {".vbs", "text/vbscript"},
            {".vcf", "text/x-vcard"},
            {".vcproj", "Application/xml"},
            {".vcs", "text/plain"},
            {".vcxproj", "Application/xml"},
            {".vddproj", "text/plain"},
            {".vdp", "text/plain"},
            {".vdproj", "text/plain"},
            {".vdx", "application/vnd.ms-visio.viewer"},
            {".vml", "text/xml"},
            {".vscontent", "application/xml"},
            {".vsct", "text/xml"},
            {".vsd", "application/vnd.visio"},
            {".vsi", "application/ms-vsi"},
            {".vsix", "application/vsix"},
            {".vsixlangpack", "text/xml"},
            {".vsixmanifest", "text/xml"},
            {".vsmdi", "application/xml"},
            {".vspscc", "text/plain"},
            {".vss", "application/vnd.visio"},
            {".vsscc", "text/plain"},
            {".vssettings", "text/xml"},
            {".vssscc", "text/plain"},
            {".vst", "application/vnd.visio"},
            {".vstemplate", "text/xml"},
            {".vsto", "application/x-ms-vsto"},
            {".vsw", "application/vnd.visio"},
            {".vsx", "application/vnd.visio"},
            {".vtx", "application/vnd.visio"},
            {".wav", "audio/wav"},
            {".wave", "audio/wav"},
            {".wax", "audio/x-ms-wax"},
            {".wbk", "application/msword"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wcm", "application/vnd.ms-works"},
            {".wdb", "application/vnd.ms-works"},
            {".wdp", "image/vnd.ms-photo"},
            {".webarchive", "application/x-safari-webarchive"},
            {".webtest", "application/xml"},
            {".wiq", "application/xml"},
            {".wiz", "application/msword"},
            {".wks", "application/vnd.ms-works"},
            {".WLMP", "application/wlmoviemaker"},
            {".wlpginstall", "application/x-wlpg-detect"},
            {".wlpginstall3", "application/x-wlpg3-detect"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wmd", "application/x-ms-wmd"},
            {".wmf", "application/x-msmetafile"},
            {".wml", "text/vnd.wap.wml"},
            {".wmlc", "application/vnd.wap.wmlc"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wmlsc", "application/vnd.wap.wmlscriptc"},
            {".wmp", "video/x-ms-wmp"},
            {".wmv", "video/x-ms-wmv"},
            {".wmx", "video/x-ms-wmx"},
            {".wmz", "application/x-ms-wmz"},
            {".wpl", "application/vnd.ms-wpl"},
            {".wps", "application/vnd.ms-works"},
            {".wri", "application/x-mswrite"},
            {".wrl", "x-world/x-vrml"},
            {".wrz", "x-world/x-vrml"},
            {".wsc", "text/scriptlet"},
            {".wsdl", "text/xml"},
            {".wvx", "video/x-ms-wvx"},
            {".x", "application/directx"},
            {".xaf", "x-world/x-vrml"},
            {".xaml", "application/xaml+xml"},
            {".xap", "application/x-silverlight-app"},
            {".xbap", "application/x-ms-xbap"},
            {".xbm", "image/x-xbitmap"},
            {".xdr", "text/plain"},
            {".xht", "application/xhtml+xml"},
            {".xhtml", "application/xhtml+xml"},
            {".xla", "application/vnd.ms-excel"},
            {".xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {".xlc", "application/vnd.ms-excel"},
            {".xld", "application/vnd.ms-excel"},
            {".xlk", "application/vnd.ms-excel"},
            {".xll", "application/vnd.ms-excel"},
            {".xlm", "application/vnd.ms-excel"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".xlt", "application/vnd.ms-excel"},
            {".xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {".xlw", "application/vnd.ms-excel"},
            {".xml", "text/xml"},
            {".xmta", "application/xml"},
            {".xof", "x-world/x-vrml"},
            {".XOML", "text/plain"},
            {".xpm", "image/x-xpixmap"},
            {".xps", "application/vnd.ms-xpsdocument"},
            {".xrm-ms", "text/xml"},
            {".xsc", "application/xml"},
            {".xsd", "text/xml"},
            {".xsf", "text/xml"},
            {".xsl", "text/xml"},
            {".xslt", "text/xml"},
            {".xsn", "application/octet-stream"},
            {".xss", "application/xml"},
            {".xtp", "application/octet-stream"},
            {".xwd", "image/x-xwindowdump"},
            {".z", "application/x-compress"},
            {".zip", "application/x-zip-compressed"},
            #endregion
            };

            string contentType = "application/octet-stream";
            try
            {
                contentType = _mappings[fileextension];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return contentType;
        }


        public static DataTable ExcelImportToDataTable(byte[] File, string SheetName)
        {
            DataTable dt = new DataTable();
            try
            {
                MemoryStream ms = new MemoryStream(File);
                using (ExcelPackage xlPackage = new ExcelPackage(ms))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[SheetName];

                    // Fetch the WorkSheet size

                    if (worksheet != null)
                    {
                        ExcelCellAddress startCell = worksheet.Dimension.Start;
                        ExcelCellAddress endCell = worksheet.Dimension.End;

                        string sColName = "";

                        // create all the needed DataColumn
                        for (int col = startCell.Column; col <= endCell.Column; col++)
                        {

                            sColName = worksheet.Cells[1, col].Address.ToString();

                            dt.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                        }
                        // place all the data into DataTable
                        for (int row = startCell.Row; row <= endCell.Row; row++)
                        {
                            DataRow dr = dt.NewRow();
                            int x = 0;
                            for (int col = startCell.Column; col <= endCell.Column; col++)
                            {

                                dr[x++] = worksheet.Cells[row, col].Value;
                            }
                            dt.Rows.Add(dr);
                        }
                        dt.TableName = SheetName;
                    }
                    else
                    {
                        throw new Exception("ไม่พบ Sheet : " + SheetName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public static DataTable ExcelImportToDataTable(byte[] File, string SheetName, int START_EXCEL_ROW, bool IsCheckFirstRow = true)
        {
            DataTable dt = new DataTable();
            try
            {
                MemoryStream ms = new MemoryStream(File);
                using (ExcelPackage xlPackage = new ExcelPackage(ms))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[SheetName];

                    // Fetch the WorkSheet size

                    if (worksheet != null)
                    {
                        ExcelCellAddress startCell = worksheet.Dimension.Start;
                        ExcelCellAddress endCell = worksheet.Dimension.End;

                        string sColName = "";

                        // create all the needed DataColumn
                        for (int col = startCell.Column; col <= endCell.Column; col++)
                        {

                            sColName = worksheet.Cells[1, col].Address.ToString();

                            dt.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                        }
                        // place all the data into DataTable
                        for (int row = startCell.Row; row <= endCell.Row; row++)
                        {
                            if (IsCheckFirstRow)
                            {
                                if (row >= START_EXCEL_ROW)
                                {
                                    if (string.IsNullOrEmpty((worksheet.Cells[row, 1].Value ?? string.Empty).ToString()))
                                    {
                                        continue;
                                    }
                                }
                            }
                            DataRow dr = dt.NewRow();
                            int x = 0;
                            for (int col = startCell.Column; col <= endCell.Column; col++)
                            {

                                dr[x++] = worksheet.Cells[row, col].Value;
                            }
                            dt.Rows.Add(dr);
                        }
                        //xlPackage.Dispose();
                        //xlPackage = null;

                        dt.TableName = SheetName;
                        //Thread.CurrentThread.CurrentCulture = Current;
                    }
                    else
                    {
                        throw new Exception("ไม่พบ Sheet : " + SheetName);
                    }
                }
            }
            catch (Exception ex)
            {
                //Thread.CurrentThread.CurrentCulture = Current;
                throw ex;
            }
            return dt;
        }

        public static DataSet ExcelImportToDataSet(byte[] File, Dictionary<string, int> dicSheet, bool IsCheckFirstRow = true)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                MemoryStream ms = new MemoryStream(File);
                using (ExcelPackage xlPackage = new ExcelPackage(ms))
                {
                    foreach (var sheet in dicSheet)
                    {
                        dt = new DataTable();
                        // get the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[sheet.Key];

                        // Fetch the WorkSheet size

                        if (worksheet != null)
                        {
                            ExcelCellAddress startCell = worksheet.Dimension.Start;
                            ExcelCellAddress endCell = worksheet.Dimension.End;

                            string sColName = "";

                            // create all the needed DataColumn
                            for (int col = startCell.Column; col <= endCell.Column; col++)
                            {

                                sColName = worksheet.Cells[1, col].Address.ToString();

                                dt.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                            }
                            // place all the data into DataTable
                            for (int row = startCell.Row; row <= endCell.Row; row++)
                            {
                                if (IsCheckFirstRow)
                                {
                                    if (row >= sheet.Value) // row start
                                    {
                                        if (string.IsNullOrEmpty((worksheet.Cells[row, 1].Value ?? string.Empty).ToString()))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                DataRow dr = dt.NewRow();
                                int x = 0;
                                for (int col = startCell.Column; col <= endCell.Column; col++)
                                {

                                    dr[x++] = worksheet.Cells[row, col].Value;
                                }
                                dt.Rows.Add(dr);
                            }
                            //xlPackage.Dispose();
                            //xlPackage = null;

                            dt.TableName = sheet.Key;
                            //Thread.CurrentThread.CurrentCulture = Current;
                            ds.Tables.Add(dt);
                        }
                        else
                        {
                            throw new Exception("ไม่พบ Sheet : " + sheet.Key);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                //Thread.CurrentThread.CurrentCulture = Current;
                throw ex;
            }
            return ds;
        }

        public static byte[] ExportMultiSheetOutput(byte[] TemplateOutput, List<ExportDataModel> data, string sFileName)
        {
            byte[] excelByte = null;
            try
            {

                //var pathTempate = WebConfigurationManager.AppSettings["templatePath"];
                //var fullPathTempate = HttpContext.Current.Server.MapPath(pathTempate + templateFileName);
                byte[] fbyte = TemplateOutput;
                MemoryStream msExcel = new MemoryStream(fbyte);
                //  fbyte = null;
                using (var xlPackage = new ExcelPackage(msExcel))
                {
                    // xlPackage = new ExcelPackage(ExcelFile);
                    ExcelRange excelRange;
                    ExcelWorksheet xWorksheet;

                    var sheets = data.Select(m => m.REPORT_CODE).Distinct();
                    foreach (string sheetName in sheets)
                    {
                        if (!sheetName.IsNullOrEmpty())
                        {
                            xWorksheet = xlPackage.Workbook.Worksheets[sheetName.AsString()];
                            if (xWorksheet != null)
                            {
                                var dataSheet = data.Where(m => m.REPORT_CODE == sheetName);
                                foreach (var item in dataSheet)
                                {
                                    excelRange = xWorksheet.Cells[item.EXCEL_XY.AsString()];
                                    if (!item.INPUT_VALUE.IsNullOrEmpty())
                                    {
                                        switch (item.DATA_TYPE.AsString().ToLower())
                                        {
                                            case "number":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "#,##0.00_);[Red](#,##0.00)"; //"#,##0.00";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            case "integer":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "#,##0_);[Red](#,##0)";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            case "percentage":
                                                try
                                                {
                                                    excelRange.Value = item.INPUT_VALUE.AsDecimal();
                                                    excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                                    excelRange.Style.Numberformat.Format = "0.00%";
                                                }
                                                catch (Exception)
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                    //result = false;
                                                }
                                                break;
                                            //case "date":
                                            //    try
                                            //    {
                                            //        excelRange.Value = Extension.AsDateTimeNull(dataExport.INPUT_VALUE);
                                            //        excelRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                            //        excelRange.Style.Numberformat.Format =
                                            //            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                            //    }
                                            //    catch (Exception)
                                            //    {
                                            //        excelRange.Value = dataExport.INPUT_VALUE;
                                            //        //result = false;
                                            //    }
                                            //    break;
                                            default:
                                                if (item.INPUT_VALUE.Contains('\t'))
                                                {
                                                    var format = new OfficeOpenXml.ExcelTextFormat();
                                                    format.Delimiter = '\t';
                                                    format.TextQualifier = '"';
                                                    format.DataTypes = new[] { eDataTypes.String };
                                                    excelRange.LoadFromText(item.INPUT_VALUE, format);
                                                }
                                                else
                                                {
                                                    excelRange.Value = item.INPUT_VALUE;
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    excelByte = xlPackage.GetAsByteArray();

                }



            }
            catch (Exception ex)
            {

            }
            return excelByte;
        }

        public static void ExportCSV<T>(HttpResponseBase response, IEnumerable<T> objList, List<string> propertyNames) where T : class, new()
        {
            var obj = new T();
            //var lst = obj.GetDisplayName();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Export");
                var i = 1;
                foreach (var propName in propertyNames)
                {
                    ws.Cells[1, i].Value = obj.GetDisplayName(propName);
                    ws.Cells[1, i].Style.Font.Bold = true;
                    ws.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    i++;
                }
                var r = 2;
                foreach (var item in objList)
                {
                    var c = 1;
                    foreach (var propName in propertyNames)
                    {
                        var propInfo = obj.GetType().GetProperty(propName);
                        var value = propInfo.GetValue(item, null);
                        var format = string.Empty;
                        if (propInfo.PropertyType == typeof(DateTime))
                        {
                            format = item.GetDisplayFormat(propName);
                        }
                        ws.Cells[r, c].Value = value.AsString(format);
                        c++;
                    }
                    r++;
                }

                Byte[] fileBytes = pck.GetAsByteArray();
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.AddHeader("content-disposition", "attachment; filename=Export.csv");
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "text/csv";
                response.AddHeader("Content-Transfer-Encoding", "binary");
                response.BinaryWrite(fileBytes);
                response.Flush();
                response.End();
            }
        }

        public static void ExportZipBlob(HttpResponseBase response, List<ZipDataModel> BLOBLIST, string FileName = "Download")
        {// Add ICSharpCode.SharpZipLib NuGet
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ".zip");
            response.ContentType = "application/zip";

            using (var zipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(response.OutputStream))
            {
                foreach (var BLOB in BLOBLIST)
                {
                    var fileEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(BLOB.FILE_NAME);
                    fileEntry.IsUnicodeText = true;
                    fileEntry.Size = BLOB.BLOB_FILE.Length;

                    zipStream.PutNextEntry(fileEntry);
                    zipStream.Write(BLOB.BLOB_FILE, 0, BLOB.BLOB_FILE.Length);
                }

                zipStream.Flush();
                zipStream.Close();
            }
        }

        public static void ExportTextToFile(HttpResponseBase response, string TextData, string FileName = "Download", string Extension = "xml")
        {
            response.Clear();

            response.AppendHeader(
                "Content-Disposition",
                "attachment ; filename =" + FileName + "." + Extension
            );

            response.ContentType = "application/x-download";
            response.ContentEncoding = Encoding.UTF8;

            response.Write(TextData);

            response.End();
        }

        
    }


}


