using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace UtilityLib
{
    public static class ConvertHelper
    {
        public static DataTable ToDataTable<TSource>(IEnumerable<TSource> data)
        {
            var dataTable = new DataTable(typeof(TSource).Name);
            var props = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (var item in data)
            {
                var values = new object[props.Length];

                for (var i = 0; i < props.Length; i++)
                    values[i] = props[i].GetValue(item, null);

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var list = new List<T>();
                foreach (var row in table.AsEnumerable())
                {
                    var obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);

                            if ((table.Columns.Contains(prop.Name + "_TH") || table.Columns.Contains(prop.Name + "_EN")) && !table.Columns.Contains(prop.Name))
                            {
                                var tblCol = "_TH";
                                var culture = Thread.CurrentThread.CurrentUICulture;
                                if (culture.Name == "en-US" && !(row[prop.Name + tblCol] == DBNull.Value || Extensions.IsNullOrEmpty(row[prop.Name + tblCol])))
                                {
                                    tblCol = "_EN";
                                }
                                if (table.Columns.Contains(prop.Name + tblCol))
                                    propertyInfo.SetValue(obj, row[prop.Name + tblCol] == DBNull.Value || Extensions.IsNullOrEmpty(row[prop.Name + tblCol]) ? null : Convert.ChangeType(row[prop.Name + tblCol], Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                            }
                            else if (table.Columns.Contains(prop.Name))
                            {
                                propertyInfo.SetValue(obj, row[prop.Name] == DBNull.Value || Extensions.IsNullOrEmpty(row[prop.Name]) ? null : Convert.ChangeType(row[prop.Name], Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                            }

                        }
                        catch (Exception ex)
                        {
                            //ex.Log();
                            //throw;
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception ex)
            {
                //ex.Log();
                return null;
            }
        }

        public static List<Dictionary<string, object>> ToListDictionary(this DataTable table, int pageIndex = -1, int pageSize = -1)
        {
            try
            {
                var list = new List<Dictionary<string, object>>();
                if (pageIndex == -1 && pageSize == -1)
                {
                    foreach (var row in table.AsEnumerable())
                    {
                        var dic = new Dictionary<string, object>();
                        foreach (DataColumn col in table.Columns)
                        {
                            try
                            {
                                dic.Add(col.ColumnName, (row[col.ColumnName] != DBNull.Value && !Extensions.IsNullOrEmpty(row[col.ColumnName])) ? Extensions.AsString(row[col.ColumnName]) : string.Empty);
                            }
                            catch (Exception ex)
                            {
                                //ex.Log();
                                //throw;
                            }
                        }
                        list.Add(dic);
                    }
                }
                else
                {
                    var startIndex = pageIndex * pageSize;
                    var endIndex = (pageIndex + 1) * pageSize;
                    if (endIndex > table.Rows.Count)
                    {
                        endIndex = table.Rows.Count;
                    }
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        var row = table.Rows[i];
                        var dic = new Dictionary<string, object>();
                        foreach (DataColumn col in table.Columns)
                        {
                            try
                            {
                                dic.Add(col.ColumnName, (row[col.ColumnName] != DBNull.Value && !Extensions.IsNullOrEmpty(row[col.ColumnName])) ? Extensions.AsString(row[col.ColumnName]) : string.Empty);
                            }
                            catch (Exception ex)
                            {
                                //ex.Log();
                                //throw;
                            }
                        }
                        list.Add(dic);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                //ex.Log();
                return null;
            }
        }

        public static T ToObject<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var obj = new T();
                foreach (var row in table.AsEnumerable())
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);
                            if (table.Columns.Contains(prop.Name))
                                propertyInfo.SetValue(obj, row[prop.Name] == DBNull.Value || Extensions.IsNullOrEmpty(row[prop.Name]) ? null : Convert.ChangeType(row[prop.Name], Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                        }
                        catch (Exception ex)
                        {
                            //ex.Log();
                            //throw;
                        }
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                //ex.Log();
                return null;
            }
        }

        public static List<T> ConvertDataTableToObject<T>(DataTable dt, List<T> objList)
        {
            if (objList == null)
            {
                objList = new List<T>();
            }

            objList.Clear();
            Type type = typeof(T);
            T obj;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                obj = (T)Activator.CreateInstance(type);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string ColumnName = dt.Columns[j].ColumnName;
                    PropertyInfo info = type.GetProperty(ColumnName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                    if (info != null)
                    {
                        object val = dt.Rows[i][j];
                        if (val != DBNull.Value)
                        {
                            info.SetValue(obj, val, null);
                        }
                    }
                }
                objList.Add(obj);
            }
            return objList;
        }

        public static List<TTarget> ToNewListObject<TSource, TTarget>(this List<TSource> objList)
            where TSource : class
            where TTarget : class, new()
        {
            var newObjList = new List<TTarget>();
            foreach (var item in objList)
            {
                var newObj = new TTarget();
                foreach (var prop in item.GetType().GetProperties())
                {
                    try
                    {
                        var propertyInfo = newObj.GetType().GetProperty(prop.Name);
                        if (propertyInfo != null)
                        {
                            var value = prop.GetValue(item, null);
                            propertyInfo.SetValue(newObj, Extensions.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        //ex.Log();
                        //throw;
                    }
                }
                newObjList.Add(newObj);
            }

            return newObjList;
        }

        public static TTarget ToNewObject<TSource, TTarget>(this TSource obj)
            where TSource : class
            where TTarget : class, new()
        {
            var newObj = new TTarget();
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var propertyInfo = newObj.GetType().GetProperty(prop.Name);
                    if (propertyInfo != null)
                    {
                        var value = prop.GetValue(obj, null);
                        propertyInfo.SetValue(newObj, Extensions.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                    }
                }
                catch (Exception ex)
                {
                    //ex.Log();
                    //throw;
                }
            }
            return newObj;
        }

        public static TTarget ToNewObject<TSource, TTarget>(this TSource obj, TTarget newObj)
            where TSource : class
            where TTarget : class, new()
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var propertyInfo = newObj.GetType().GetProperty(prop.Name);
                    if (propertyInfo != null)
                    {
                        var value = prop.GetValue(obj, null);
                        propertyInfo.SetValue(newObj, Extensions.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                    }
                }
                catch (Exception ex)
                {
                    //ex.Log();
                    //throw;
                }
            }
            return newObj;
        }

        public static TSource MergeObject<TSource, TTarget>(this TSource obj, TTarget obj2)
            where TSource : class
            where TTarget : class
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var propertyInfo = obj2.GetType().GetProperty(prop.Name);
                    if (propertyInfo != null)
                    {
                        var newValue = propertyInfo.GetValue(obj2, null);
                        prop.SetValue(obj, Extensions.IsNullOrEmpty(newValue) ? null : Convert.ChangeType(newValue, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                    }
                }
                catch (Exception ex)
                {
                    //ex.Log();
                    //throw;
                }
            }
            return obj;
        }

        public static T CloneObject<T>(this T obj)
            where T : class, new()
        {
            var newObj = new T();
            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    var propertyInfo = newObj.GetType().GetProperty(prop.Name);
                    if (propertyInfo != null)
                    {
                        var value = prop.GetValue(obj, null);
                        propertyInfo.SetValue(newObj, Extensions.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                    }
                }
                catch (Exception ex)
                {
                    //ex.Log();
                    //throw;
                }
            }
            return newObj;
        }

        public static string ConvertObjectToJson(object obj, string[] optionIgn = null)
        {
            var strJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            if (optionIgn == null)
            {
                optionIgn = new[] { "[", "]", "\"" };
            }
            foreach (var str in optionIgn)
            {
                strJson = strJson.Replace(str, "");
            }
            return strJson;
        }

        public static string ConvertObjectToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static byte[] ToArrayByte(this HttpPostedFileBase file)
        {
            byte[] data = null;
            if (file != null && file.ContentLength > 0)
            {
                MemoryStream memoryStream = new MemoryStream();
                file.InputStream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }
            return data;
        }

        public static DataTable ToExcelDataTable(this byte[] File, string SheetName)
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
        public static List<T> ToExcelList<T>(this byte[] File, string SheetName) where T : class, new()
        {
            var list = new List<T>();
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

                        //string sColName = "";

                        //// create all the needed DataColumn
                        //for (int col = startCell.Column; col <= endCell.Column; col++)
                        //{

                        //    sColName = worksheet.Cells[1, col].Address.ToString();

                        //    dt.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                        //}
                        // place all the data into DataTable

                        for (int row = startCell.Row + 1; row <= endCell.Row; row++)
                        {
                            try
                            {
                                var obj = new T();
                                for (int col = startCell.Column; col <= endCell.Column; col++)
                                {
                                    var colName = worksheet.Cells[1, col].Value.ToString();
                                    var propertyInfo = obj.GetType().GetProperty(colName);
                                    if (propertyInfo != null)
                                    {
                                        propertyInfo.SetValue(obj, Extensions.IsNullOrEmpty(worksheet.Cells[row, col].Value) ? null : Convert.ChangeType(worksheet.Cells[row, col].Value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                                    }
                                }
                                list.Add(obj);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }
                        }
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
            return list;
        }
        public static List<T> ToExcelList<T>(this byte[] File, string SheetName, int startRowIndex) where T : class, new()
        {
            var list = new List<T>();
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

                        //string sColName = "";

                        //// create all the needed DataColumn
                        //for (int col = startCell.Column; col <= endCell.Column; col++)
                        //{

                        //    sColName = worksheet.Cells[1, col].Address.ToString();

                        //    dt.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                        //}
                        // place all the data into DataTable

                        for (int row = startRowIndex; row <= endCell.Row; row++)
                        {
                            try
                            {
                                var obj = new T();
                                for (int col = startCell.Column; col <= endCell.Column; col++)
                                {
                                    var colName = worksheet.Cells[1, col].Value.ToString();
                                    var propertyInfo = obj.GetType().GetProperty(colName);
                                    if (propertyInfo != null)
                                    {
                                        propertyInfo.SetValue(obj, Extensions.IsNullOrEmpty(worksheet.Cells[row, col].Value) ? null : Convert.ChangeType(worksheet.Cells[row, col].Value, Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);
                                    }
                                }
                                list.Add(obj);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                            }
                        }
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
            return list;
        }
        public static DataTable ToExcelDataTable(this byte[] File, string SheetName, int START_EXCEL_ROW, bool IsCheckFirstRow = true)
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
        public static DataSet ToExcelDataSet(this byte[] File, Dictionary<string, int> dicSheet, bool IsCheckFirstRow = true)
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
        public static string ToFileHash(this byte[] bfile)
        {
            var sha = new System.Security.Cryptography.SHA1Managed();
            byte[] hashbfile = sha.ComputeHash(bfile);
            return Convert.ToBase64String(hashbfile);
        }
        public static List<T> JsonToObject<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
        }
    }
}
