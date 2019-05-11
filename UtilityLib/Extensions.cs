using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using System.ComponentModel;
using System.Reflection;
using System.Web;

namespace UtilityLib
{
    public static class Extensions
    {
        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        public static bool IsNullOrEmpty(this object data)
        {
            return string.IsNullOrEmpty(Convert.ToString(data));
        }

        public static string AsString(this object data, string defaultValue = "", bool isDateTHFormat = false)
        {
            string newValue = string.Empty;

            if (!IsNullOrEmpty(data))
            {
                if (data.GetType() == typeof(DateTime))
                {
                    if (IsNullOrEmpty(defaultValue))
                    {
                        defaultValue = "dd/MM/yyyy";
                    }
                    if (!isDateTHFormat)
                    {
                        newValue = string.Format("{0:" + defaultValue + "}", data, defaultValue);
                    }
                    else
                    {
                        var cultureinfo = CultureInfo.CreateSpecificCulture("th-TH");
                        var culture = Thread.CurrentThread.CurrentCulture;
                        Thread.CurrentThread.CurrentCulture = cultureinfo;

                        DateTime date = Convert.ToDateTime(data);

                        newValue = string.Format("{0:" + defaultValue + "}", date, defaultValue);

                        Thread.CurrentThread.CurrentCulture = culture;
                    }
                }
                else
                {
                    newValue = Convert.ToString(data).Trim();
                }
            }
            else
            {
                newValue = IsNullOrEmpty(defaultValue) ? string.Empty : Convert.ToString(defaultValue).Trim();
            }
            return newValue;
        }

        public static string AsStringDate(this object data, string defaultValue = "", bool isDateTHFormat = false)
        {
            string newValue = string.Empty;

            if (!IsNullOrEmpty(data))
            {
                if (data.GetType() == typeof(DateTime))
                {
                    if (IsNullOrEmpty(defaultValue))
                    {
                        defaultValue = "dd/MM/yyyy HH:mm:ss";
                    }
                    if (!isDateTHFormat)
                    {
                        newValue = string.Format("{0:" + defaultValue + "}", data, defaultValue);
                    }
                    else
                    {
                        var cultureinfo = CultureInfo.CreateSpecificCulture("th-TH");
                        var culture = Thread.CurrentThread.CurrentCulture;
                        Thread.CurrentThread.CurrentCulture = cultureinfo;

                        DateTime date = Convert.ToDateTime(data);

                        newValue = string.Format("{0:" + defaultValue + "}", date, defaultValue);

                        Thread.CurrentThread.CurrentCulture = culture;
                    }
                }
                else
                {
                    newValue = Convert.ToString(data).Trim();
                }
            }
            else
            {
                newValue = IsNullOrEmpty(defaultValue) ? string.Empty : Convert.ToString(defaultValue).Trim();
            }
            return newValue;
        }

        public static string AsChar(this object data, int length)
        {
            string newValue = string.Empty;
            if (!IsNullOrEmpty(data))
            {
                newValue = Convert.ToString(data);
                if (newValue.Length < length)
                {
                    for (int i = newValue.Length; i < length; i++)
                    {
                        newValue += " ";
                    }
                }
                else if (newValue.Length > length)
                {
                    try
                    {
                        newValue += newValue.Substring(0, newValue.Length - 1);
                    }
                    catch (Exception ex)
                    {
                        newValue = Convert.ToString(data);
                    }
                }
            }
            return newValue;
        }
        public static DateTime AsDateTime(this object data, DateTime? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToDateTime(defaultValue) : DateTime.MinValue;

            var dutchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            dutchCultureInfo.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            dutchCultureInfo.DateTimeFormat.DateSeparator = "/";

            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = dutchCultureInfo;
            DateTime dtTime = Convert.ToDateTime(data);

            var date = string.Format("{0:dd/MM/yyyy}", dtTime);

            DateTime newDate = DateTime.ParseExact(date, "dd/MM/yyyy", dutchCultureInfo, DateTimeStyles.None);

            Thread.CurrentThread.CurrentCulture = culture;
            return newDate;

        }

        public static DateTime AsDateTimes(this object data, DateTime? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToDateTime(defaultValue) : DateTime.MinValue;

            var dutchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            dutchCultureInfo.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            dutchCultureInfo.DateTimeFormat.DateSeparator = "/";

            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = dutchCultureInfo;
            DateTime dtTime = Convert.ToDateTime(data);

            var date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", dtTime);

            DateTime newDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", dutchCultureInfo, DateTimeStyles.None);

            Thread.CurrentThread.CurrentCulture = culture;
            return newDate;

        }

        public static DateTime AsDatePicker(this object data, DateTime? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToDateTime(defaultValue) : DateTime.MinValue;

            var dutchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            dutchCultureInfo.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            dutchCultureInfo.DateTimeFormat.DateSeparator = "/";

            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = dutchCultureInfo;
            DateTime dtTime = Convert.ToDateTime(data);

            var date = string.Format("{0:dd/MM/yyyy}", dtTime);

            DateTime newDate = DateTime.ParseExact(date, "MM/dd/yyyy", dutchCultureInfo, DateTimeStyles.None);

            Thread.CurrentThread.CurrentCulture = culture;
            return newDate;

        }

        public static DateTime? AsDateTimeNull(this object data)
        {
            //return IsNullOrEmpty(data) ? (DateTime?)null : Convert.ToDateTime(data);

            if (IsNullOrEmpty(data))
                return null;

            var dutchCultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            dutchCultureInfo.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            dutchCultureInfo.DateTimeFormat.DateSeparator = "/";

            var culture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = dutchCultureInfo;
            DateTime dtTime = Convert.ToDateTime(data);

            var date = string.Format("{0:dd/MM/yyyy}", dtTime);

            DateTime newDate = DateTime.ParseExact(date, "dd/MM/yyyy", dutchCultureInfo, DateTimeStyles.None);

            Thread.CurrentThread.CurrentCulture = culture;
            return newDate;
        }

        public static decimal AsDecimal(this object data, decimal? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToDecimal(defaultValue) : 0;

            return Convert.ToDecimal(data);
        }

        public static decimal? AsDecimalNull(this object data)
        {
            return IsNullOrEmpty(data) ? (decimal?)null : Convert.ToDecimal(data);
        }

        public static int AsInt(this object data, int? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToInt32(defaultValue) : 0;

            return Convert.ToInt32(data);
        }

        public static int? AsIntNull(this object data)
        {
            return IsNullOrEmpty(data) ? (int?)null : Convert.ToInt32(data);
        }

        public static long AsLong(this object data, long? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return defaultValue != null ? Convert.ToInt64(defaultValue) : 0;

            return Convert.ToInt64(data);
        }

        public static long? AsLongNull(this object data)
        {
            return IsNullOrEmpty(data) ? (long?)null : Convert.ToInt64(data);
        }

        public static short AsShort(this object data, short? defaultValue = null)
        {
            if (IsNullOrEmpty(data))
                return (short)(defaultValue != null ? Convert.ToInt16(defaultValue) : 0);

            return Convert.ToInt16(data);
        }

        public static short? AsShortNull(this object data)
        {
            return IsNullOrEmpty(data) ? (short?)null : Convert.ToInt16(data);
        }
        public static string GetRequest(this System.Web.HttpRequestBase data, string key)
        {
            return data[key] != null ? Convert.ToString(data[key]) : string.Empty;
        }
        public static string GetRequest(this System.Web.HttpRequest data, string key)
        {
            return data[key] != null ? Convert.ToString(data[key]) : string.Empty;
        }
        public static bool IsPropertyExist(dynamic dynamicObj, string property)
        {
            try
            {
                var value = dynamicObj[property].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }

        }
        public static string GetDescription(this Enum @enum)
        {
            FieldInfo fieldInfo = @enum.GetType().GetField(@enum.ToString());
            DescriptionAttribute description = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).OfType<DescriptionAttribute>().FirstOrDefault();
            return description == null ? @enum.ToString() : description.Description;
        }

        public static string GetFileName(this HttpPostedFileBase file)
        {
            string data = null;
            if (file != null)
            {
                data = file.FileName;
            }
            return data;
        }
        public static decimal GetFileSize(this HttpPostedFileBase file)
        {
            decimal data = 0;
            if (file != null && file.ContentLength > 0)
            {
                data = file.ContentLength.AsDecimal() / (1048576M);
            }
            return data;
        }
        public static decimal GetFileSize(this byte[] file)
        {
            decimal data = 0;
            if (file != null && file.Length > 0)
            {
                data = file.Length.AsDecimal() / (1048576M);
            }
            return data;
        }

        public static string GetAllText(this string path)
        {
            return System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
        }

        public static string GetSingleValue(this string valueTH, string valueEN)
        {
            string newValue = valueTH;
            var culture = Thread.CurrentThread.CurrentUICulture;
            if ((culture.Name == "en-US" && !string.IsNullOrEmpty(valueEN)) ||
                (culture.Name != "en-US" && !string.IsNullOrEmpty(valueEN) && string.IsNullOrEmpty(valueTH))
                )
            {
                newValue = valueEN;
            }
            return newValue;
        }
    }
}
