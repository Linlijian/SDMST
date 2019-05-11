using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using UtilityLib;

namespace WEBAPP.Helper
{
    public class ExcelData
    {
        private static string _json_sumModel;
        private static string _json_sumColumn;
        private static string _json_sumData;
        private static string _colover;

        public static string TBL_SELECT_NAME = "";
        public static DataSet TBL_SELECT
        {
            get
            {
                if (HttpContext.Current.Session[TBL_SELECT_NAME] == null)
                {
                    DataSet ds = new DataSet();
                    HttpContext.Current.Session[TBL_SELECT_NAME] = ds;
                }

                return (DataSet)HttpContext.Current.Session[TBL_SELECT_NAME];
            }
            set
            {
                HttpContext.Current.Session[TBL_SELECT_NAME] = value;
            }
        }

        public static List<Dictionary<object, string>> TBL_SELECT_SUM
        {
            get
            {
                if (HttpContext.Current.Session[TBL_SELECT_NAME + "SUM"] == null)
                {
                    List<Dictionary<object, string>> items = new List<Dictionary<object, string>>();
                    HttpContext.Current.Session[TBL_SELECT_NAME + "SUM"] = items;
                }

                return (List<Dictionary<object, string>>)HttpContext.Current.Session[TBL_SELECT_NAME + "SUM"];
            }
            set
            {
                HttpContext.Current.Session[TBL_SELECT_NAME + "SUM"] = value;
            }
        }

        public static string UPLOAD_FILENAME
        {
            get
            {
                return (string)HttpContext.Current.Session[TBL_SELECT_NAME + "FILENAME"];
            }
            set
            {
                HttpContext.Current.Session[TBL_SELECT_NAME + "FILENAME"] = value;
            }
        }

        public static void setTmpDataComplate(string dtName, DataSet ds)
        {
            if (HttpContext.Current.Session[dtName] != null)
            {
                HttpContext.Current.Session[dtName] = null;
            }

            HttpContext.Current.Session[dtName] = ds;
        }
        public static void setTmpDataError(string dtName, DataSet ds)
        {
            if (HttpContext.Current.Session[dtName] != null)
            {
                HttpContext.Current.Session[dtName] = null;

            }
            HttpContext.Current.Session[dtName] = ds;

        }


        public static string defaultFormatDate
        {
            get
            {
                return "dd/MM/yyyy";
            }
        }

        public static Boolean IsNumericInt(Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64)
                return true;

            try
            {
                if (Expression is string)
                {
                    //Convert.ToDouble(Expression as string);/* Komsan */
                    Expression.AsInt(); //Adjust by March
                }
                else
                {
                    //Double.Parse(Expression.ToString());
                    Expression.ToString().AsInt(); //Adjust by March
                }

                return true;
            }
            catch { return false; } // just dismiss errors but return false

            return false;
        }

        public static Boolean IsNumericDecimal(Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;



            try
            {
                if (Expression is string)
                    Convert.ToDecimal(Expression as string); /* Komsan */
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch
            {
                return false;
            } // just dismiss errors but return false

            return false;
        }

        public static void RemoveCol(ref DataTable dt)
        {
            string col = "";
            string comma = "";
            string[] markCol = new string[3];
            try
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName == "IDX")
                    {
                        markCol[0] = "IDX";

                    }
                    else if (dc.ColumnName == "ERR_COL")
                    {
                        markCol[1] = "ERR_COL";
                    }
                    else if (dc.ColumnName == "COM_COL")
                    {
                        markCol[2] = "COM_COL";
                    }
                }

                dt.Columns.Remove(markCol[0]);
                dt.Columns.Remove(markCol[1]);
                dt.Columns.Remove(markCol[2]);

            }
            catch (Exception ex)
            {


            }
        }

        private static int SetResult(string PRG_CODE, DataSet ds)
        {
            DataTable dtTempE = ds.Tables[0].Clone();
            DataTable dtTempC = ds.Tables[0].Clone();

            string strExpErr = "COM_COL = 'E' ";
            string strExpCom = "COM_COL = 'C' ";

            DataRow[] dRowE = ds.Tables[0].Select(strExpErr);
            DataRow[] dRowC = ds.Tables[0].Select(strExpCom);


            for (int i = 0; i < dRowE.Length; i++)
            {

                dtTempE.ImportRow(dRowE[i]);

            }
            for (int i = 0; i < dRowC.Length; i++)
            {

                dtTempC.ImportRow(dRowC[i]);

            }

            RemoveCol(ref dtTempE);
            RemoveCol(ref dtTempC);

            dtTempE.TableName = "E";
            dtTempC.TableName = "C";

            //DataSet dsVal = EXC001.QueryAllVal(SessionHelper.SYS_COM_CODE, PRG_CODE, "");
            DataAccess.EXC001.EXC001DA da = new DataAccess.EXC001.EXC001DA();
            da.DTO.Execute.ExecuteType = DataAccess.EXC001.EXC001ExecuteType.GetQueryAllVal;
            da.DTO.Model.COM_CODE = SessionHelper.SYS_COM_CODE;
            da.DTO.Model.PRG_CODE = PRG_CODE;
            da.Select(da.DTO);

            DataTable dtC = ConvertTypeDate(dtTempC, da.DTO.Models);

            DataSet dsC = new DataSet();
            DataSet dsE = new DataSet();
            //dsC.Tables.Add(dtTempC);
            //
            dtTempC.Dispose();

            dsC.Tables.Add(dtC);
            dsE.Tables.Add(dtTempE);


            //---Test DaTa 15/11/2011----
            setTmpDataComplate("C_" + PRG_CODE, dsC);
            setTmpDataError("E_" + PRG_CODE, dsE);

            return 0;
        }

        public static DataSet GetResultExcelByRank(string PRG_CODE, int idxFrom, int idxTo)
        {
            DataSet dsResult = new DataSet();
            DataSet ds = (DataSet)HttpContext.Current.Session[PRG_CODE.Trim()];
            DataTable dtTemp = ds.Tables[0].Clone();
            string strExp = string.Format("IDX >= {0} AND IDX <= {1}", idxFrom, idxTo);
            DataRow[] dRow = ds.Tables[0].Select(strExp);

            for (int i = 0; i < dRow.Length; i++)
            {
                dtTemp.ImportRow(dRow[i]);
            }

            DataTable dtTempSum = ds.Tables[1].Copy();
            dsResult.Tables.Add(dtTemp);
            dsResult.Tables.Add(dtTempSum);

            //setTmp
            SetResult(PRG_CODE, ds);

            return dsResult;
        }

        public static DataSet GetResultExcelByRank(string PRG_CODE, int idxTable, int idxFrom, int idxTo)
        {
            DataSet dsResult = new DataSet();
            DataSet ds = (DataSet)HttpContext.Current.Session[PRG_CODE.Trim()];
            DataTable dtTemp = ds.Tables[idxTable].Clone();
            string strExp = string.Format("IDX >= {0} AND IDX <= {1}", idxFrom, idxTo);
            DataRow[] dRow = ds.Tables[idxTable].Select(strExp);

            for (int i = 0; i < dRow.Length; i++)
            {
                dtTemp.ImportRow(dRow[i]);
            }

            DataTable dtTempSum = ds.Tables[1].Copy();
            dsResult.Tables.Add(dtTemp);
            dsResult.Tables.Add(dtTempSum);

            //setTmp
            SetResult(PRG_CODE, ds);

            return dsResult;
        }


        private static Decimal ToFormatDecimal(string strVal)
        {
            Decimal dec;
            Decimal.TryParse(strVal, out dec);
            return dec;
        }
        private static Int32 ToFormatINT(string strVal)
        {

            Int32 integer;
            Int32.TryParse(strVal, out integer);

            return integer;
        }
        private static DateTime ToFormatDateTime(string strVal, string FORMAT_STYLE, string CULTULE_STYLE)
        {
            DateTime strToDate = new DateTime();
            string pattern = defaultFormatDate;
            bool valid = false;
            try
            {

                System.IFormatProvider format;

                if (CULTULE_STYLE != "")
                {
                    format = new System.Globalization.CultureInfo(CULTULE_STYLE, true);

                }
                else
                {

                    format = new System.Globalization.CultureInfo("en-US", true);

                }

                if (FORMAT_STYLE != "")
                {

                    pattern = FORMAT_STYLE;
                }

                valid = DateTime.TryParseExact(strVal, pattern, format, System.Globalization.DateTimeStyles.None, out strToDate);
            }
            catch (Exception ex)
            {

            }

            return strToDate;


        }

        private static DataTable ConvertTypeDate(DataTable dtData, List<EXC001Model> dtVal)
        {

            List<string> lsColDate = new List<string>();
            List<string> lsInt = new List<string>();
            List<string> lsDecimal = new List<string>();

            Dictionary<string, string> dicFormat = new Dictionary<string, string>();
            Dictionary<string, string> dicCULTULE = new Dictionary<string, string>();


            DataTable dtCloned = dtData.Clone();

            try
            {
                foreach (EXC001Model dr in dtVal)
                {
                    if (dr.COL_TYPE.ToString() == "4")
                    {

                        string colName = dr.COL_NAME.ToString().Trim();
                        string FORMAT_STYLE = "";
                        string CULTULE_STYLE = "";
                        lsColDate.Add(colName);

                        if (dr.FORMAT_STYLE != null)
                        {
                            FORMAT_STYLE = dr.FORMAT_STYLE.ToString().Trim();
                            dicFormat.Add(colName, CULTULE_STYLE);
                        }
                        if (dr.CULTULE_STYLE != null)
                        {
                            CULTULE_STYLE = dr.CULTULE_STYLE.ToString().Trim();
                            dicCULTULE.Add(colName, CULTULE_STYLE);

                        }


                    }
                    //INT
                    if (dr.COL_TYPE.ToString() == "2")
                    {
                        string colName = dr.COL_NAME.ToString().Trim();
                        lsInt.Add(colName);
                    }
                    //Decimal
                    if (dr.COL_TYPE.ToString() == "3")
                    {
                        string colName = dr.COL_NAME.ToString().Trim();
                        lsDecimal.Add(colName);
                    }



                }

                #region Convet Column to DateTime int , decimal

                foreach (DataColumn dc in dtData.Columns)
                {

                    foreach (string colDate in lsColDate)
                    {
                        if (dc.ColumnName == colDate)
                        {
                            dtCloned.Columns[dc.ColumnName].DataType = Type.GetType("System.DateTime");
                        }
                    }
                    foreach (string colInt in lsInt)
                    {
                        if (dc.ColumnName == colInt)
                        {
                            dtCloned.Columns[dc.ColumnName].DataType = Type.GetType("System.Int32");
                        }
                    }
                    foreach (string colDecimal in lsDecimal)
                    {
                        if (dc.ColumnName == colDecimal)
                        {
                            dtCloned.Columns[dc.ColumnName].DataType = Type.GetType("System.Decimal");
                        }
                    }
                }

                #endregion



                foreach (DataRow drData in dtData.Rows)
                {
                    //DataRow newRow = dtCloned.NewRow();

                    foreach (var colName in lsColDate)
                    {

                        var date = drData[colName];
                        if (date == System.DBNull.Value)
                        {
                            DateTime? newdate = null;
                            drData[colName] = newdate;
                        }
                        else
                        {
                            drData[colName] = ToFormatDateTime(date.ToString(), dicFormat[colName], dicCULTULE[colName]);

                        }
                        //drData[colName] = this.ToFormatDateTime( drData[colName].ToString(), dicFormat[colName], dicCULTULE[colName] );
                    }
                    foreach (var colName in lsInt)
                    {
                        drData[colName] = ToFormatINT(drData[colName].ToString());
                    }
                    foreach (var colName in lsDecimal)
                    {

                        drData[colName] = ToFormatDecimal(drData[colName].ToString());
                    }

                    dtCloned.ImportRow(drData);

                }

            }

            catch (Exception ex)
            {
                throw ex;

            }

            return dtCloned;

        }


        public static bool ValidateShipDate(string shipStatus)
        {
            DateTime startDate;
            bool valid = false;
            try
            {
                System.IFormatProvider format = new System.Globalization.CultureInfo("th-TH");

                valid = DateTime.TryParse(shipStatus, format, System.Globalization.DateTimeStyles.AssumeLocal, out startDate);
            }
            catch
            {
                return valid;
            }

            return valid;

        }
        public static bool ValidateShipDate(string shipStatus, string FORMAT_STYLE, string CULTULE_STYLE)
        {
            DateTime parsedDate;
            string pattern = defaultFormatDate;

            bool valid = false;
            try
            {

                System.IFormatProvider format;
                //System.IFormatProvider format = new System.Globalization.CultureInfo("en-US");

                if (CULTULE_STYLE != "")
                {
                    format = new System.Globalization.CultureInfo(CULTULE_STYLE, true);

                    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                }
                else
                {
                    format = new System.Globalization.CultureInfo("en-US", true);
                }

                if (FORMAT_STYLE != "")
                {

                    pattern = FORMAT_STYLE;
                }

                valid = DateTime.TryParseExact(shipStatus, pattern, format, System.Globalization.DateTimeStyles.AssumeLocal, out parsedDate);
            }
            catch (Exception ex)
            {
                valid = false;
            }

            return valid;

        }

        public static int ValidateExcelDataType(string PRG_CODE, DataTable dsData, List<EXC001Model> dsVal)
        {
            int result = 0;

            try
            {
                try
                {
                    TBL_SELECT_NAME = PRG_CODE.Trim();

                    List<EXC001Model> drs = dsVal.Where(x => x.PRG_CODE.Trim() == PRG_CODE.Trim()).ToList();

                    if (drs.Count < dsData.Columns.Count)
                    {
                        //-Data overflow or Validate config Setup below
                        _colover = "MD";
                    }
                    else if (drs.Count > dsData.Columns.Count)
                    {
                        //-Data below or Validate config Setup overflow
                        _colover = "DM";
                    }
                    else
                    {
                        _colover = "BB";
                    }

                    #region add DB Column
                    foreach (var dbcol in dsVal)
                    {
                        DataColumnCollection columns = dsData.Columns;
                        if (!dbcol.DB_COLUMN.IsNullOrEmpty())
                        {
                            if (!columns.Contains(dbcol.DB_COLUMN))
                            {//ชื่อไม่ตรง DB ให้เพิ่ม
                                dsData.Columns.Add(dbcol.DB_COLUMN);

                                foreach (DataRow dr in dsData.Rows)
                                {
                                    dr[dbcol.DB_COLUMN] = dr[dbcol.COL_NAME];
                                }
                            }
                        }
                    }
                    #endregion

                    DataSet dsResult = ValidateExcel(PRG_CODE, dsData, dsVal);

                    if (dsResult != null && dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (HttpContext.Current.Session[PRG_CODE] != null)
                        {
                            HttpContext.Current.Session[PRG_CODE] = null;
                        }

                        if (Convert.ToDecimal(dsResult.Tables[1].Rows[0]["ERROR_ROWS"]) > 0)
                        {
                            result = 2;
                        }

                        //#region add DB Column
                        //foreach (var dbcol in dsVal)
                        //{
                        //    DataColumnCollection columns = dsResult.Tables[0].Columns;
                        //    if (!dbcol.DB_COLUMN.IsNullOrEmpty())
                        //    {
                        //        if (!columns.Contains(dbcol.DB_COLUMN))
                        //        {//ชื่อไม่ตรง DB ให้เพิ่ม
                        //            dsResult.Tables[0].Columns.Add(dbcol.DB_COLUMN);

                        //            foreach (DataRow dr in dsResult.Tables[0].Rows)
                        //            {
                        //                dr[dbcol.DB_COLUMN] = dr[dbcol.COL_NAME];
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion

                        TBL_SELECT = dsResult.Copy();
                    }
                    else
                    {
                        result = 1;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                result = 99;
                throw ex;
            }

            return result;
        }
        public static DataSet ValidateExcel(string PRG_CODE, DataTable dtData, List<EXC001Model> dtVal)
        {

            DataSet dsResult = new DataSet();
            dtData.Columns.Add("IDX", typeof(int));
            dtData.Columns.Add("ERR_COL", typeof(string));
            dtData.Columns.Add("COM_COL", typeof(string));

            int allRows = dtData.Rows.Count;
            int completeRows = 0;
            int errorRows = 0;
            string errCol = "";


            int dataType = 0;
            int colVal = 0;
            string data = "";

            List<int> lsDc = new List<int>();
            List<decimal> lsDec = new List<decimal>();

            try
            {
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    dtData.Rows[i]["IDX"] = i + 1;
                    errCol = "";
                    for (int j = 0; j < dtVal.Count; j++)
                    {
                        if (dtVal[j].FLG_USE.ToString() == "Y")
                        {//เช็คว่าใช้ไหม
                            if (dtVal[j].IS_VALIDATE.ToString() == "Y")
                            {//เช็คว่าจะ VALIDATE ไหม
                                dataType = Convert.ToInt32(dtVal[j].COL_TYPE.ToString());
                                colVal = Convert.ToInt32(dtVal[j].LIST_NO.ToString());
                                data = dtData.Rows[i][colVal - 1].ToString();

                                if (string.IsNullOrEmpty(data))
                                {
                                    if (dtVal[j].ISNULL.ToString() == "Y")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        errCol += colVal.ToString() + ",";

                                        continue;
                                    }
                                }

                                bool isValidateMaxLength = true;

                                switch (dataType)
                                {
                                    case 1:
                                        #region string type
                                        if (!dtVal[j].MAX_LENGTH.IsNullOrEmpty())
                                        {
                                            if (data.Length > dtVal[j].MAX_LENGTH.AsInt())
                                            {
                                                errCol += colVal.ToString() + ",";
                                            }
                                        }
                                        #endregion
                                        break;
                                    case 2:
                                        #region int type
                                        if (!dtVal[j].MAX_LENGTH.IsNullOrEmpty())
                                        {
                                            try
                                            {
                                                if (data.Length > dtVal[j].MAX_LENGTH.AsInt())
                                                {
                                                    isValidateMaxLength = false;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }

                                        if (!IsNumericInt(data) || !isValidateMaxLength)
                                        {
                                            errCol += colVal.ToString() + ",";
                                        }
                                        else
                                        {
                                            DataColumn dc = dtData.Columns[colVal - 1];

                                            if (lsDc.IndexOf(dc.Ordinal) == -1)
                                            {
                                                lsDc.Add(dc.Ordinal);
                                            }
                                        }
                                        #endregion
                                        break;
                                    case 3:
                                        #region decimal type
                                        if (!dtVal[j].MAX_LENGTH.IsNullOrEmpty())
                                        {
                                            if (dtVal[j].MAX_LENGTH.IndexOf(',') > -1)
                                            {
                                                var maxLengths = dtVal[j].MAX_LENGTH.Split(',');
                                                int digitInteger = maxLengths[0].AsInt();
                                                int digitDecimal = maxLengths[1].AsInt();
                                                digitInteger = digitInteger - digitDecimal;

                                                if (data.IndexOf('.') > -1)
                                                {//มีทศนิยม
                                                    var datas = data.Split('.');

                                                    if (datas[0].Length > digitInteger || datas[1].Length > digitDecimal)
                                                    {
                                                        isValidateMaxLength = false;
                                                    }
                                                }
                                                else
                                                {
                                                    if (data.Length > digitInteger)
                                                    {
                                                        isValidateMaxLength = false;
                                                    }
                                                }
                                            }
                                        }

                                        if (!IsNumericDecimal(data) || !isValidateMaxLength)
                                        {
                                            errCol += colVal.ToString() + ",";
                                        }
                                        else
                                        {
                                            DataColumn dc = dtData.Columns[colVal - 1];

                                            if (lsDec.IndexOf(dc.Ordinal) == -1)
                                            {
                                                lsDec.Add(dc.Ordinal);
                                            }
                                        }
                                        #endregion
                                        break;
                                    case 4:
                                        #region datetime type
                                        string FORMAT_STYLE = "";
                                        string CULTULE_STYLE = "";

                                        var drFORMAT_STYLE = dtVal[j].FORMAT_STYLE;
                                        var drCULTULE_STYLE = dtVal[j].CULTULE_STYLE;

                                        if (drFORMAT_STYLE != null && !string.IsNullOrEmpty(drFORMAT_STYLE))
                                        {
                                            FORMAT_STYLE = drFORMAT_STYLE.ToString().Trim();

                                        }
                                        if (drCULTULE_STYLE != null && !string.IsNullOrEmpty(drCULTULE_STYLE))
                                        {
                                            CULTULE_STYLE = drCULTULE_STYLE.ToString().Trim();
                                        }

                                        if (!string.IsNullOrEmpty(data))
                                        {
                                            if (!ValidateShipDate(data, FORMAT_STYLE, CULTULE_STYLE))
                                            {
                                                errCol += colVal.ToString() + ",";
                                            }
                                        }
                                        else
                                        {
                                            errCol += colVal.ToString() + ",";
                                        }
                                        #endregion
                                        break;
                                }
                            }
                        }
                    }

                    if (errCol.Length > 0)
                    {//Fix by Bird
                        errCol = errCol.Substring(0, errCol.Length - 1);

                        dtData.Rows[i]["ERR_COL"] = errCol;
                        dtData.Rows[i]["COM_COL"] = "E";
                        errorRows++;
                    }
                    else
                    {
                        dtData.Rows[i]["COM_COL"] = "C";
                        completeRows++;
                    }
                }
            }
            catch (Exception ex)
            {


            }

            if (dtData.Rows.Count > 0)
            {
                DataTable dtSummary = new DataTable();
                dtSummary.TableName = "DTSummary";
                dtSummary.Columns.Add("PRG_CODE", typeof(string));
                dtSummary.Columns.Add("ALL_ROWS", typeof(int));
                dtSummary.Columns.Add("COMPLETE_ROWS", typeof(int));
                dtSummary.Columns.Add("ERROR_ROWS", typeof(int));

                DataRow dRow = dtSummary.NewRow();
                dRow["PRG_CODE"] = PRG_CODE;
                dRow["ALL_ROWS"] = allRows;
                dRow["COMPLETE_ROWS"] = completeRows;
                dRow["ERROR_ROWS"] = errorRows;
                dtSummary.Rows.Add(dRow);

                DataTable dtResult = dtData.Copy();
                dtResult.TableName = "DTResult";

                dsResult.Tables.Add(dtResult);
                dsResult.Tables.Add(dtSummary);
            }

            if (lsDc.Count > 0 || lsDec.Count > 0)
            {

                DataTable dtSummary = dtData.Clone();
                DataRow[] drs = dtData.Copy().Select("COM_COL <> 'E'");
                if (drs.Length > 0)
                {
                    foreach (var dr in drs)
                    {
                        dtSummary.ImportRow(dr);
                    }

                    Dictionary<object, string> dic_colSum = new Dictionary<object, string>();

                    if (lsDc.Count > 0 && dtSummary.Rows.Count > 0)
                    {
                        foreach (int idx in lsDc)
                        {
                            string colName = dtData.Columns[idx].ColumnName;
                            //string strSum = "Sum("+colName+")";
                            dtSummary.Columns.Add("Temp", typeof(int), "Convert([" + colName + "], 'System.Int32')");

                            Int32 sumTmp = Convert.ToInt32(dtSummary.Compute("Sum(Temp)", ""));
                            dtSummary.Columns.Remove("Temp");
                            dic_colSum.Add(colName, sumTmp.ToString());
                        }
                    }


                    if (lsDec.Count > 0 && dtSummary.Rows.Count > 0)
                    {
                        foreach (int idx in lsDec)
                        {
                            string colName = dtData.Columns[idx].ColumnName;
                            //string strSum = "Sum("+colName+")";
                            dtSummary.Columns.Add("Temp", typeof(decimal), "Convert([" + colName + "], 'System.Decimal')");

                            decimal sumTmp = Convert.ToDecimal(dtSummary.Compute("Sum(Temp)", ""));
                            dtSummary.Columns.Remove("Temp");
                            dic_colSum.Add(colName, sumTmp.ToString());
                        }

                    }

                    if (dic_colSum.Count > 0)
                    {
                        //DataTable dtJson = new DataTable();
                        //List<string> lsModel = new List<string>();
                        Dictionary<object, string> dic_colSumDB = new Dictionary<object, string>();

                        foreach (var item in dic_colSum)
                        {
                            var tmpDB_COL = dtVal.Where(x => x.COL_NAME.ToUpper() == item.Key.ToString().ToUpper()).ToList().FirstOrDefault();

                            if (tmpDB_COL != null)
                            {
                                if (tmpDB_COL.DB_COLUMN != null)
                                {
                                    dic_colSumDB.Add(tmpDB_COL.DB_COLUMN, item.Value);
                                }
                                else
                                {
                                    dic_colSumDB.Add(item.Key, item.Value);
                                }
                            }
                            else
                            {
                                dic_colSumDB.Add(item.Key, item.Value);
                            }
                        }

                        //foreach (string s in dic_colSum.Keys)
                        //{
                        //    var tmpDB_COL = dtVal.Where(x => x.COL_NAME == s).ToList().FirstOrDefault();

                        //    if (tmpDB_COL != null)
                        //    {
                        //        //dtJson.Columns.Add(tmpDB_COL.DB_COLUMN, typeof(string));
                        //        //lsModel.Add(tmpDB_COL.DB_COLUMN);
                        //    }
                        //    else
                        //    {
                        //        //dtJson.Columns.Add(s, typeof(string));
                        //        //lsModel.Add(s);
                        //    }
                        //}
                        //_json_sumModel = Newtonsoft.Json.JsonConvert.SerializeObject(lsModel);
                        //_json_sumColumn = GenerateColumn110px(dtJson);

                        List<Dictionary<object, string>> items = new List<Dictionary<object, string>>();
                        items.Add(dic_colSumDB);
                        //Dictionary<object, object> dicJson = new Dictionary<object, object>();
                        //dicJson.Add("items", items);
                        //_json_sumData = Newtonsoft.Json.JsonConvert.SerializeObject(dicJson);

                        TBL_SELECT_SUM = items;
                    }
                }
            }


            return dsResult;
        }

        public static EXC001Model ValidateExcel(byte[] File, string FileName, string COM_CODE, string PRG_CODE, List<EXC001Model> dsVal, int SheetIndex = 1)
        {
            DataTable dsData = new DataTable();
            EXC001Model result = new EXC001Model();

            try
            {
                MemoryStream ms = new MemoryStream(File);
                using (ExcelPackage xlPackage = new ExcelPackage(ms))
                {
                    dsData = new DataTable();

                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[SheetIndex];

                    if (worksheet != null)
                    {
                        ExcelCellAddress startCell = worksheet.Dimension.Start;
                        ExcelCellAddress endCell = worksheet.Dimension.End;

                        string sColName = "";

                        for (int col = startCell.Column; col <= endCell.Column; col++)
                        {
                            if (worksheet.Cells[1, col].Value != null)
                            {
                                sColName = worksheet.Cells[1, col].Value.ToString();
                                //dsData.Columns.Add(sColName.Substring(0, sColName.ToString().Length - 1));
                                dsData.Columns.Add(sColName);
                            }
                        }

                        for (int row = (startCell.Row + 1); row <= endCell.Row; row++)
                        {

                            DataRow dr = dsData.NewRow();
                            for (int col = startCell.Column; col <= endCell.Column; col++)
                            {
                                if (worksheet.Cells[row, col].Value != null)
                                {
                                    dr[col - 1] = worksheet.Cells[row, col].Value;
                                }
                            }
                            dsData.Rows.Add(dr);
                        }

                        dsData.TableName = worksheet.Name;
                    }
                }

                for (int i = 0; i < dsData.Columns.Count; i++)
                {
                    for (int ii = 0; ii < dsVal.Count; ii++)
                    {
                        if ((i + 1).ToString() == dsVal[ii].LIST_NO.ToString())
                        {
                            try
                            {
                                dsData.Columns[i].ColumnName = dsVal[ii].COL_NAME.ToString().ToUpper();
                            }
                            catch (Exception)
                            {
                            }
                            continue;
                        }
                    }
                }

                int ress = ValidateExcelDataType(PRG_CODE, dsData, dsVal);

                if (ress != 99)
                {
                    // OpenPopup(page, PrgCode, fi.Name, url);
                    result = Open_ExcelPop(PRG_CODE, FileName);

                }


                UPLOAD_FILENAME = FileName;
                HttpContext.Current.Session["json_param"] = null;
                HttpContext.Current.Session.Add("json_param", result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static EXC001Model Open_ExcelPop(string procode, string filename)
        {
            //!!--ไม่จำเป็นต้องแก้ มาจาก Themeplate
            string total = "";
            string complete = "";
            string error = "";

            DataSet ds = new DataSet("open_popup");

            ds = GetResultExcelByRank(procode, 0, 1);

            string strCol = GenerateColumn110px(ds.Tables[0]);
            string strModel = GenerateModel(ds.Tables[0]);

            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                //filename = fileName; //field filename
                total = ds.Tables[1].Rows[0][1].ToString();
                complete = ds.Tables[1].Rows[0][2].ToString();
                error = ds.Tables[1].Rows[0][3].ToString();
            }

            //List<Dictionary<object, string>> lisdic = new List<Dictionary<object, string>>();
            //Dictionary<object, string> dic = new Dictionary<object, string>();

            EXC001Model lisdic = new EXC001Model();
            lisdic.filename = filename;
            lisdic.total = total;
            lisdic.error = error;
            lisdic.complete = complete;
            lisdic.callback = "";
            lisdic.exportparam = "excel";
            lisdic.columns = strCol;
            lisdic.model = strModel;
            lisdic.continueUrl = "";
            lisdic.procode = procode.Trim();
            lisdic.control_id = "";
            //Tab Summary
            lisdic.data_sum = "";
            lisdic.model_sum = "";
            lisdic.columns_sum = "";
            lisdic.colover = _colover;

            return lisdic;
        }

        public static string GenerateColumn110px(DataTable dt)
        {
            string col = "";
            string comma = "";
            string[] markCol = new string[3];
            try
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName == "IDX")
                    {
                        markCol[0] = "IDX";

                    }
                    else if (dc.ColumnName == "ERR_COL")
                    {
                        markCol[1] = "ERR_COL";
                    }
                    else if (dc.ColumnName == "COM_COL")
                    {
                        markCol[2] = "COM_COL";
                    }
                }
                if (markCol.Length > 0)
                {
                    for (int i = 0; i < markCol.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(markCol[i]))
                        {
                            dt.Columns.Remove(markCol[i]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

                if (dt != null)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {

                        col += comma + "{ width : 110  , header : '" + dc.ColumnName.ToUpper() + "', dataIndex : '" + dc.ColumnName + "'}";
                        comma = ",";


                    }
                }
            }

            return "[" + col + "]";
        }
        public static string GenerateModel(DataTable dt)
        {

            string col = "";
            string comma = "";

            foreach (DataColumn dc in dt.Columns)
            {

                col += comma + "{ name : '" + dc.ColumnName + "', type: 'string' }";
                comma = ",";


            }
            //  { name: 'id', type: 'string' },


            return "[" + col + "]";
        }
    }
}