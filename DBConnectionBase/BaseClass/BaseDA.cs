using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UtilityLib;
using System.Reflection;
using System.Collections;
using System.Data.Entity;

namespace DataAccess
{
    public abstract class BaseDA
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger("RootLogger");

        protected AATEntities _DBManger;
        protected SqlDBManger _DBMangerNoEF;
        private string connectionStringEF { get; set; }
        #region Select
        protected virtual BaseDTO DoSelect(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        public BaseDTO Select(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBManger = new AATEntities(connectionStringEF);
            try
            {
                baseDTO = DoSelect(baseDTO);
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ResultMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    baseDTO.Result.ResultMsg = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        baseDTO.Result.ResultMsg = ex.InnerException.InnerException.Message;
                    }
                }
            }
            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);

            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }
        public BaseDTO SelectNoEF(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            try
            {
                baseDTO = DoSelect(baseDTO);
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }

            return baseDTO;
        }
        #endregion Select

        #region Insert
        protected virtual BaseDTO DoInsert(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        protected virtual BaseDTO DoAfterInsert(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        public BaseDTO Insert(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBManger = new AATEntities(connectionStringEF);
            DbContextTransaction _Transaction = null;
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _Transaction = _DBManger.Database.BeginTransaction();
                }
                baseDTO = DoInsert(baseDTO);
                _DBManger.SaveChanges();
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    baseDTO.Result.ResultMsg = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        baseDTO.Result.ResultMsg = ex.InnerException.InnerException.Message;
                    }
                }
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _Transaction.Commit();
                    }
                    else
                    {
                        _Transaction.Rollback();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterInsert(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            baseDTO = SetStandardLog(baseDTO, "After");
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }

        public BaseDTO InsertNoEF(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _DBMangerNoEF.BeginTransaction();
                }
                baseDTO = DoInsert(baseDTO);
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _DBMangerNoEF.CommitTransaction();
                    }
                    else
                    {
                        _DBMangerNoEF.RollbackTransaction();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterInsert(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            baseDTO = SetStandardLog(baseDTO, "After");
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }
        #endregion Insert

        #region Update
        protected virtual BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        protected virtual BaseDTO DoAfterUpdate(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        public BaseDTO Update(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            baseDTO = SetStandardLog(baseDTO, "Before");

            _DBManger = new AATEntities(connectionStringEF);
            DbContextTransaction _Transaction = null;
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _Transaction = _DBManger.Database.BeginTransaction();
                }
                baseDTO = DoUpdate(baseDTO);
                _DBManger.SaveChanges();
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    baseDTO.Result.ResultMsg = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        baseDTO.Result.ResultMsg = ex.InnerException.InnerException.Message;
                    }
                }
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _Transaction.Commit();
                    }
                    else
                    {
                        baseDTO.Result.IsResult = false;
                        _Transaction.Rollback();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterUpdate(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            baseDTO = SetStandardLog(baseDTO, "After");
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }

        public BaseDTO UpdateNoEF(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            baseDTO = SetStandardLog(baseDTO, "Before");
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _DBMangerNoEF.BeginTransaction();
                }
                baseDTO = DoUpdate(baseDTO);
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _DBMangerNoEF.CommitTransaction();
                    }
                    else
                    {
                        _DBMangerNoEF.RollbackTransaction();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterUpdate(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            baseDTO = SetStandardLog(baseDTO, "After");
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }
        #endregion Update

        #region Delete
        protected virtual BaseDTO DoDelete(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        protected virtual BaseDTO DoAfterDelete(BaseDTO baseDTO)
        {
            return baseDTO;
        }
        public BaseDTO Delete(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            baseDTO = SetStandardLog(baseDTO, "Before");

            _DBManger = new AATEntities(connectionStringEF);
            DbContextTransaction _Transaction = null;
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _Transaction = _DBManger.Database.BeginTransaction();
                }
                baseDTO = DoDelete(baseDTO);
                _DBManger.SaveChanges();
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    baseDTO.Result.ResultMsg = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        baseDTO.Result.ResultMsg = ex.InnerException.InnerException.Message;
                    }
                }
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _Transaction.Commit();
                    }
                    else
                    {
                        baseDTO.Result.IsResult = false;
                        _Transaction.Rollback();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterDelete(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }

        public BaseDTO DeleteNoEF(BaseDTO baseDTO)
        {
            connectionStringEF = baseDTO.ConnectTo.GetDescription().GetConnectionStringEF("AATEntities");
            _DBMangerNoEF = new SqlDBManger();
            _DBMangerNoEF.ConnectionName = baseDTO.ConnectTo.GetDescription();
            baseDTO = SetStandardLog(baseDTO, "Before");
            try
            {
                if (baseDTO.IsTransaction)
                {
                    _DBMangerNoEF.BeginTransaction();
                }
                baseDTO = DoDelete(baseDTO);
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            finally
            {
                if (baseDTO.IsTransaction)
                {
                    if (baseDTO.Result.ActionResult >= 0)
                    {
                        _DBMangerNoEF.CommitTransaction();
                    }
                    else
                    {
                        _DBMangerNoEF.RollbackTransaction();
                    }
                }
            }
            try
            {
                if (baseDTO.Result.IsResult)
                {
                    baseDTO = DoAfterDelete(baseDTO);
                }
            }
            catch (Exception ex)
            {
                baseDTO.Result.IsResult = false;
                baseDTO.Result.ActionResult = -1;
                baseDTO.Result.ResultMsg = ex.Message;
            }
            baseDTO = SaveStandardLog(baseDTO);

            _DBManger = new AATEntities(connectionStringEF);
            baseDTO = SaveMessageError(baseDTO);
            if (_DBManger != null)
            {
                _DBManger.Dispose();
                _DBManger = null;
            }
            return baseDTO;
        }

        #endregion Delete

        #region StandardLog
        private BaseDTO SetStandardLog(BaseDTO baseDTO, string mode)
        {
            try
            {
                if ((baseDTO.TransactionLog != null && baseDTO.TransactionLog.SaveLogConfig != null) && ((mode == "After" && baseDTO.Result.IsResult) || mode == "Before"))
                {
                    if (baseDTO.TransactionLog.ACTIVITY_TYPE == 10074002 || baseDTO.TransactionLog.ACTIVITY_TYPE == 10074003 || baseDTO.TransactionLog.ACTIVITY_TYPE == 10074004)
                    {
                        foreach (var conf in baseDTO.TransactionLog.SaveLogConfig)
                        {
                            using (var db = new AATEntities(connectionStringEF))
                            {
                                var ctrlLog = db.VSMS_CTRLLOG.Where(m => m.TBL_NAME == conf.TableName).Select(m => m.LOG_STATUS).FirstOrDefault();
                                if (!ctrlLog.IsNullOrEmpty())
                                {
                                    conf.DoInsertLog = ctrlLog == "Y";
                                }
                            }
                            if (conf.DoInsertLog)
                            {
                                var cmdWhere = string.Empty;
                                foreach (var col in conf.Columns)
                                {
                                    cmdWhere += " and ";
                                    if (baseDTO.TransactionLog.ObjectValue.GetType().IsGenericType)
                                    {
                                        var lstObj = (IList)baseDTO.TransactionLog.ObjectValue;
                                        if (lstObj != null && lstObj.Count > 0)
                                        {
                                            var obj = ((IList)baseDTO.TransactionLog.ObjectValue)[0];
                                            var prop = obj.GetType().GetProperty(col.PKColumnName);
                                            if (prop.PropertyType == typeof(DateTime))
                                            {
                                                cmdWhere += "convert(char(10)," + col.PKColumnName + ",121)";
                                            }
                                            else
                                            {
                                                cmdWhere += col.PKColumnName;
                                            }
                                            cmdWhere += " in (";
                                            foreach (var item in lstObj)
                                            {
                                                var value = GetValueObject(item, col.PKColumnName);
                                                if (value.GetType() == typeof(string))
                                                {
                                                    cmdWhere += "'" + value + "'";
                                                }
                                                else if (value.GetType() == typeof(string) && col.IsChar)
                                                {
                                                    cmdWhere += "'" + value.AsChar(col.CharLength) + "'";
                                                }
                                                else if (value.GetType() == typeof(DateTime))
                                                {
                                                    cmdWhere += "'" + value.AsString("yyyy-MM-dd") + "'";
                                                }
                                                else
                                                {
                                                    cmdWhere += col.PKColumnName + "=" + value;
                                                }
                                                cmdWhere += ",";
                                            }
                                            cmdWhere = cmdWhere.TrimEnd(',');
                                            cmdWhere += ")";
                                        }
                                    }
                                    else
                                    {
                                        var value = GetValueObject(baseDTO.TransactionLog.ObjectValue, col.PKColumnName);
                                        if (value.GetType() == typeof(string))
                                        {
                                            cmdWhere += col.PKColumnName + "='" + value + "'";
                                        }
                                        else if (value.GetType() == typeof(string) && col.IsChar)
                                        {
                                            cmdWhere += col.PKColumnName + "='" + value.AsChar(col.CharLength) + "'";
                                        }
                                        else if (value.GetType() == typeof(DateTime))
                                        {
                                            cmdWhere += "convert(char(10)," + col.PKColumnName + ",121)='" + value.AsString("yyyy-MM-dd") + "'";
                                        }
                                        else
                                        {
                                            cmdWhere += col.PKColumnName + "=" + value;
                                        }
                                    }

                                }
                                var cmd = string.Format("select * from {0}.{1} where 1=1 {2}", conf.Schema, conf.TableName, cmdWhere);
                                var data = _DBMangerNoEF.ExecuteDataSet(cmd, commandType: CommandType.Text);
                                if (mode == "Before" && baseDTO.TransactionLog.ACTIVITY_TYPE == 10074003 || baseDTO.TransactionLog.ACTIVITY_TYPE == 10074004)
                                {
                                    conf.DataBeforeSave = data.OutputDataSet;
                                }
                                else if (mode == "After" && baseDTO.TransactionLog.ACTIVITY_TYPE == 10074002 || baseDTO.TransactionLog.ACTIVITY_TYPE == 10074003)
                                {
                                    conf.DataAfterSave = data.OutputDataSet;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("SetStandardLog : " + ex.Message);
                //throw;
            }
            return baseDTO;
        }
        private BaseDTO SaveStandardLog(BaseDTO baseDTO)
        {
            if (baseDTO.Result.IsResult)
            {
                try
                {
                    if (baseDTO.TransactionLog != null && baseDTO.TransactionLog.SaveLogConfig != null && baseDTO.Result.IsResult)
                    {
                        foreach (var conf in baseDTO.TransactionLog.SaveLogConfig)
                        {
                            //{ "Search",10074001 },
                            //{ "SaveCreate",10074002 },
                            //{ "SaveModify",10074003 },
                            //{ "Delete",10074004 },
                            //{ "Info",10074005 },
                            //{ "Process",10074006 },
                            //{ "Report",10074007 },
                            //{ "Upload",10074008 }
                            if (conf.DoInsertLog)
                            {
                                if (baseDTO.TransactionLog.ACTIVITY_TYPE == 10074003 || baseDTO.TransactionLog.ACTIVITY_TYPE == 10074004)
                                {
                                    string cmd = "insert into " + conf.Schema + "." + conf.TableName + "_LOG (LOG_TYPE,LOG_BY,LOG_DATE,";
                                    var cols = string.Join(",", conf.DataBeforeSave.Tables[0].Columns.Cast<DataColumn>().Select(m => m.ColumnName));
                                    cmd += cols + ")";
                                    var values = "VALUES(@LOG_TYPE,@LOG_BY,@LOG_DATE,@";
                                    var colValues = string.Join(",@", conf.DataBeforeSave.Tables[0].Columns.Cast<DataColumn>().Select(m => m.ColumnName));
                                    values += colValues + ")";
                                    cmd += values;
                                    var logType = "O";
                                    if (baseDTO.TransactionLog.ACTIVITY_TYPE == 10074004)
                                    {
                                        logType = "D";
                                    }
                                    foreach (DataRow row in conf.DataBeforeSave.Tables[0].Rows)
                                    {
                                        var parameters = CreateParameter();
                                        parameters.AddParameter("LOG_TYPE", logType);
                                        parameters.AddParameter("LOG_BY", baseDTO.TransactionLog.CRET_BY);
                                        parameters.AddParameter("LOG_DATE", baseDTO.TransactionLog.CRET_DATE);
                                        foreach (DataColumn col in conf.DataBeforeSave.Tables[0].Columns)
                                        {
                                            parameters.AddParameter(col.ColumnName, row[col.ColumnName]);
                                        }
                                        var beforeResult = _DBMangerNoEF.ExecuteNonQuery(cmd, parameters, CommandType.Text);

                                    }
                                    if (baseDTO.TransactionLog.ACTIVITY_TYPE == 10074003)
                                    {
                                        foreach (DataRow row in conf.DataAfterSave.Tables[0].Rows)
                                        {
                                            var parameters = CreateParameter();
                                            parameters.AddParameter("LOG_TYPE", "N");
                                            parameters.AddParameter("LOG_BY", baseDTO.TransactionLog.CRET_BY);
                                            parameters.AddParameter("LOG_DATE", baseDTO.TransactionLog.CRET_DATE);
                                            foreach (DataColumn col in conf.DataAfterSave.Tables[0].Columns)
                                            {
                                                parameters.AddParameter(col.ColumnName, row[col.ColumnName]);
                                            }
                                            var beforeResult = _DBMangerNoEF.ExecuteNonQuery(cmd, parameters, CommandType.Text);

                                        }
                                    }
                                }
                                else if (baseDTO.TransactionLog.ACTIVITY_TYPE == 10074002)
                                {
                                    string cmd = "insert into " + conf.Schema + "." + conf.TableName + "_LOG (LOG_TYPE,LOG_BY,LOG_DATE,";
                                    var cols = string.Join(",", conf.DataAfterSave.Tables[0].Columns.Cast<DataColumn>().Select(m => m.ColumnName));
                                    cmd += cols + ")";
                                    var values = "VALUES(@LOG_TYPE,@LOG_BY,@LOG_DATE,@";
                                    var colValues = string.Join(",@", conf.DataAfterSave.Tables[0].Columns.Cast<DataColumn>().Select(m => m.ColumnName));
                                    values += colValues + ")";
                                    cmd += values;
                                    foreach (DataRow row in conf.DataAfterSave.Tables[0].Rows)
                                    {
                                        var parameters = CreateParameter();
                                        parameters.AddParameter("LOG_TYPE", "A");
                                        parameters.AddParameter("LOG_BY", baseDTO.TransactionLog.CRET_BY);
                                        parameters.AddParameter("LOG_DATE", baseDTO.TransactionLog.CRET_DATE);
                                        foreach (DataColumn col in conf.DataAfterSave.Tables[0].Columns)
                                        {
                                            parameters.AddParameter(col.ColumnName, row[col.ColumnName]);
                                        }
                                        var beforeResult = _DBMangerNoEF.ExecuteNonQuery(cmd, parameters, CommandType.Text);

                                    }
                                }
                            }
                        }
                    }
                    else if (baseDTO.TransactionLog != null && baseDTO.TransactionLog.DoInsertLog)
                    {
                        //var parameters = CreateParameter();
                        //parameters.AddParameter("RecordCount", null, ParameterDirection.Output);
                        //parameters.AddParameter("sERROR_MSG", null, ParameterDirection.Output);
                        //parameters.AddParameter("pTRANSLOG_DT", baseDTO.TransactionLog.LOG_DATE);
                        //parameters.AddParameter("pUSER_ID", baseDTO.TransactionLog.LOG_BY);
                        //parameters.AddParameter("pEMPLOYEE_ID", baseDTO.TransactionLog.EMPLOYEE_ID);
                        //parameters.AddParameter("pDEPT_CODE", baseDTO.TransactionLog.DEPT_CODE);
                        //parameters.AddParameter("pMS_CORP_ID", baseDTO.TransactionLog.MS_CORP_ID);
                        //parameters.AddParameter("pMS_CORP_CODE", baseDTO.TransactionLog.MS_CORP_CODE);
                        //parameters.AddParameter("pSYSTEM_CODE", baseDTO.TransactionLog.SYSTEM_CODE);
                        //parameters.AddParameter("pSUB_SYSTEM_CODE", baseDTO.TransactionLog.SUB_SYSTEM_CODE);
                        //parameters.AddParameter("pPRG_ID", baseDTO.TransactionLog.PRG_ID);
                        //parameters.AddParameter("pIP_ADDRESS", baseDTO.TransactionLog.IP_ADDRESS);
                        //parameters.AddParameter("pLOG_HEADER", baseDTO.TransactionLog.LOG_HEADER);
                        //parameters.AddParameter("pDETAIL", null);
                        //parameters.AddParameter("pOTHER_DETAIL", baseDTO.TransactionLog.OTHER_DETAIL);
                        //parameters.AddParameter("pRECORD_ID", null);
                        //parameters.AddParameter("pTRANS_TYPE", baseDTO.TransactionLog.TRANS_TYPE);
                        //parameters.AddParameter("pACTIVITY_TYPE", baseDTO.TransactionLog.ACTIVITY_TYPE);
                        //parameters.AddParameter("pCOM_CODE", baseDTO.TransactionLog.COM_CODE);
                        //parameters.AddParameter("pCREATE_DT", baseDTO.TransactionLog.CREATE_DT);
                        //parameters.AddParameter("pCREATE_USER", baseDTO.TransactionLog.CREATE_USER);

                        //var result = _DBManger.ExecuteNonQuery("PKG_SEC_TRANSLOG.SP_INSERT_DATA", parameters);
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            }
            return baseDTO;
        }
        private object GetValueObject(object value, string propertyName)
        {
            Type currentType = value.GetType();

            object val = value;
            foreach (string name in propertyName.Split('.'))
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var nVal = string.Empty;
                    foreach (var item in val as IList)
                    {
                        PropertyInfo property = item.GetType().GetProperty(name);
                        nVal += property.GetValue(item, null).ToString() + ",";
                        currentType = property.PropertyType;
                    }
                    val = nVal.TrimEnd(',');
                }
                else
                {
                    PropertyInfo property = currentType.GetProperty(name);
                    val = property.GetValue(val, null);
                    currentType = property.PropertyType;
                }
            }
            return val;
        }

        #endregion
        private BaseDTO SaveMessageError(BaseDTO baseDTO)
        {
            if (!baseDTO.Result.IsResult)
            {
                //try
                //{
                //    var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;

                //    var lang = "TH";
                //    if (culture.Name == "en-US")
                //    {
                //        lang = "EN";
                //    }

                //    var parameters = CreateParameter();
                //    parameters.AddParameter("pERROR_CODE", null, ParameterDirection.Output);
                //    parameters.AddParameter("pERROR_DES_TH", null, ParameterDirection.Output);
                //    parameters.AddParameter("pERROR_DES", baseDTO.Result.ResultMsg);
                //    parameters.AddParameter("pERRORLOG_DT", baseDTO.TransactionLog.TRANSLOG_DT);
                //    parameters.AddParameter("pUSER_ID", baseDTO.TransactionLog.USER_ID);
                //    parameters.AddParameter("pEMPLOYEE_ID", baseDTO.TransactionLog.USER_ID);
                //    parameters.AddParameter("pDEPT_CODE", baseDTO.TransactionLog.DEPT_CODE);
                //    parameters.AddParameter("pMS_CORP_ID", baseDTO.TransactionLog.MS_CORP_ID);
                //    parameters.AddParameter("pMS_CORP_CODE", baseDTO.TransactionLog.MS_CORP_CODE);
                //    parameters.AddParameter("pSYSTEM_CODE", baseDTO.TransactionLog.SYSTEM_CODE);
                //    parameters.AddParameter("pSUB_SYSTEM_CODE", baseDTO.TransactionLog.SUB_SYSTEM_CODE);
                //    parameters.AddParameter("pPRG_ID", baseDTO.TransactionLog.PRG_ID);
                //    parameters.AddParameter("pIP_ADDRESS", baseDTO.TransactionLog.IP_ADDRESS);
                //    parameters.AddParameter("pLOG_HEADER", baseDTO.TransactionLog.LOG_HEADER);
                //    parameters.AddParameter("pDETAIL", baseDTO.TransactionLog.DETAIL);
                //    parameters.AddParameter("pOTHER_DETAIL", baseDTO.TransactionLog.OTHER_DETAIL);
                //    parameters.AddParameter("pRECORD_ID", baseDTO.TransactionLog.RECORD_ID);
                //    parameters.AddParameter("pCOM_CODE", baseDTO.TransactionLog.COM_CODE);
                //    parameters.AddParameter("pCREATE_DT", baseDTO.TransactionLog.CREATE_DT);
                //    parameters.AddParameter("pCREATE_USER", baseDTO.TransactionLog.CREATE_USER);
                //    parameters.AddParameter("pLANG", lang);

                //    var result = _DBManger.ExecuteNonQuery("PKG_SEC_ERRORLOG.sp_getErrMSG", parameters);
                //    if (result.Status)
                //    {
                //        baseDTO.Result.ResultCode = result.OutputData["pERROR_CODE"].AsString();
                //        baseDTO.Result.ResultMsg = result.OutputData["pERROR_DES_TH"].AsString();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    //throw;
                //} 
            }
            return baseDTO;
        }
        //protected BaseDTO DoInsertFile(BaseDTO baseDTO)
        //{
        //    if (baseDTO.Result.ActionResult >= 0)
        //    {
        //        var notIDS = baseDTO.ExecuteFile.Files.Where(m => !m.ID.IsNullOrEmpty()).Select(m => m.ID).ToList();
        //        var notID = string.Empty;
        //        if (notIDS != null && notIDS.Count > 0)
        //        {
        //            notID = string.Join(",", notIDS);
        //        }
        //        var result = new ExecuteNonQueryResult();

        //        var parameters = CreateParameter();
        //        if (baseDTO.Result.ActionResult >= 0 && baseDTO.ExecuteFile.IsDeleteBeforeSave)
        //        {
        //            parameters.AddParameter("RecordCount", null, ParameterDirection.Output);
        //            parameters.AddParameter("sERROR_MSG", null, ParameterDirection.Output);
        //            parameters.AddParameter("pCOM_CODE", baseDTO.ExecuteFile.COM_CODE);
        //            parameters.AddParameter("pPRG_CODE", baseDTO.ExecuteFile.PRG_CODE);
        //            parameters.AddParameter("pDOCUMENT_TYPE_ID", baseDTO.ExecuteFile.DOCUMENT_TYPE_ID);
        //            parameters.AddParameter("pSECTION_GROUP_ID", baseDTO.ExecuteFile.SECTION_GROUP_ID);
        //            parameters.AddParameter("pHEADER_INPUT_ID", baseDTO.ExecuteFile.HEADER_INPUT_ID);
        //            parameters.AddParameter("pDETAIL_INPUT_ID", baseDTO.ExecuteFile.DETAIL_INPUT_ID);
        //            parameters.AddParameter("pBLOB_LIST_IDs", notID);

        //            result = _DBManger.ExecuteNonQuery("PKG_OIC_COM_BLOB_CTRL.SP_DELETE_BLOB_LIST_NOTIN", parameters);
        //            if (!result.Success(baseDTO))
        //            {
        //                return baseDTO;
        //            }
        //            baseDTO.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
        //            baseDTO.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
        //        }
        //        if (baseDTO.ExecuteFile.IsCopy)
        //        {
        //            parameters = CreateParameter();
        //            parameters.AddParameter("RecordCount", null, ParameterDirection.Output);
        //            parameters.AddParameter("sERROR_MSG", null, ParameterDirection.Output);
        //            parameters.AddParameter("pCOM_CODE", baseDTO.ExecuteFile.COM_CODE);
        //            parameters.AddParameter("pCREATE_USER", baseDTO.ExecuteFile.CREATE_USER);
        //            parameters.AddParameter("pCREATE_DT", baseDTO.ExecuteFile.CREATE_DT);
        //            parameters.AddParameter("ps_HEADER_INPUT_ID", baseDTO.ExecuteFile.OLD_HEADER_INPUT_ID);
        //            parameters.AddParameter("pd_HEADER_INPUT_ID", baseDTO.ExecuteFile.HEADER_INPUT_ID);
        //            parameters.AddParameter("ps_DOCUMENT_TYPE_ID", baseDTO.ExecuteFile.OLD_DOCUMENT_TYPE_ID);
        //            parameters.AddParameter("pd_DOCUMENT_TYPE_ID", baseDTO.ExecuteFile.DOCUMENT_TYPE_ID);
        //            parameters.AddParameter("ps_SECTION_GROUP_ID", baseDTO.ExecuteFile.OLD_SECTION_GROUP_ID);
        //            parameters.AddParameter("pd_SECTION_GROUP_ID", baseDTO.ExecuteFile.SECTION_GROUP_ID);
        //            parameters.AddParameter("ps_DETAIL_INPUT_ID", baseDTO.ExecuteFile.OLD_DETAIL_INPUT_ID);
        //            parameters.AddParameter("pd_DETAIL_INPUT_ID", baseDTO.ExecuteFile.DETAIL_INPUT_ID);
        //            parameters.AddParameter("pBLOB_LIST_IDs", notID);
        //            parameters.AddParameter("pCOVER_SHEET_SEND_ID", baseDTO.ExecuteFile.COVER_SHEET_SEND_ID);

        //            result = _DBManger.ExecuteNonQuery("PKG_OIC_COM_BLOB_CTRL.SP_COPY_BLOB_LIST", parameters);
        //            if (!result.Success(baseDTO))
        //            {
        //                return baseDTO;
        //            }
        //            baseDTO.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
        //            baseDTO.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
        //        }
        //        if (baseDTO.Result.ActionResult >= 0)
        //        {
        //            foreach (var file in baseDTO.ExecuteFile.Files)
        //            {
        //                parameters = CreateParameter();
        //                parameters.AddParameter("RecordCount", null, ParameterDirection.Output);
        //                parameters.AddParameter("sERROR_MSG", null, ParameterDirection.Output);
        //                parameters.AddParameter("pCOM_CODE", baseDTO.ExecuteFile.COM_CODE);
        //                parameters.AddParameter("pACTIVE", baseDTO.ExecuteFile.ACTIVE);
        //                parameters.AddParameter("pSTATUS", baseDTO.ExecuteFile.STATUS);
        //                parameters.AddParameter("pEFFECTIVE_DT ", baseDTO.ExecuteFile.EFFECTIVE_DT);
        //                parameters.AddParameter("pCREATE_USER", baseDTO.ExecuteFile.CREATE_USER);
        //                parameters.AddParameter("pCREATE_DT", baseDTO.ExecuteFile.CREATE_DT);
        //                parameters.AddParameter("pUPDATE_USER", baseDTO.ExecuteFile.UPDATE_USER);
        //                parameters.AddParameter("pUPDATE_DT", baseDTO.ExecuteFile.UPDATE_DT);

        //                parameters.AddParameter("pBLOB_LIST_ID", file.ID);
        //                parameters.AddParameter("pPRG_CODE", baseDTO.ExecuteFile.PRG_CODE);
        //                parameters.AddParameter("pDOCUMENT_TYPE_ID", baseDTO.ExecuteFile.DOCUMENT_TYPE_ID);
        //                parameters.AddParameter("pSECTION_GROUP_ID", baseDTO.ExecuteFile.SECTION_GROUP_ID);
        //                parameters.AddParameter("pHEADER_INPUT_ID", baseDTO.ExecuteFile.HEADER_INPUT_ID);
        //                parameters.AddParameter("pDETAIL_INPUT_ID", baseDTO.ExecuteFile.DETAIL_INPUT_ID);
        //                parameters.AddParameter("pPATH_FILE_UPLOAD", baseDTO.ExecuteFile.PATH_FILE_UPLOAD);
        //                parameters.AddParameter("pFILE_TYPE", file.FILE_TYPE);
        //                parameters.AddParameter("pFILE_SIZE", file.FILE_SIZE);
        //                parameters.AddParameter("pFILE_DATE", file.FILE_DATE);
        //                parameters.AddParameter("pFILE_NAME", file.FILE_NAME);
        //                parameters.AddParameter("pCERTIFICATE_NUMBER", file.CERTIFICATE_NUMBER);
        //                parameters.AddParameter("pSIGNATURE_SIGN", file.SIGNATURE_SIGN);
        //                parameters.AddParameter("pBLOB_FILE", file.BLOB_FILE, SqlDBType.Blob);
        //                parameters.AddParameter("pBLOB_FILE_HASH", file.BLOB_FILE_HASH);
        //                parameters.AddParameter("pCOVER_SHEET_SEND_ID", baseDTO.ExecuteFile.COVER_SHEET_SEND_ID);

        //                result = _DBManger.ExecuteNonQuery("PKG_OIC_COM_BLOB_CTRL.SP_INSERT_BLOB_LIST", parameters);
        //                if (!result.Success(baseDTO))
        //                {
        //                    break;
        //                }
        //                baseDTO.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
        //                baseDTO.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
        //                if (baseDTO.Result.ActionResult < 0)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return baseDTO;
        //}
        protected List<SqlDBParameter> CreateParameter()
        {
            var parameters = new List<SqlDBParameter>();
            parameters.Clear();
            return parameters;
        }

        protected BaseDTO GetJobStatus(BaseDTO baseDTO)
        {
            if (!baseDTO.JobName.IsNullOrEmpty())
            {
                var result = _DBMangerNoEF.ExecuteDataSet(string.Format("exec [msdb].[dbo].[sp_help_job] @job_name='{0}'", baseDTO.JobName), null, CommandType.Text);

                if (result.OutputDataSet.Tables[0].Rows.Count > 0)
                {
                    if (result.OutputDataSet.Tables[0].Rows[0]["current_execution_status"] != DBNull.Value)
                    {
                        baseDTO.JobResult.ActionResult = Convert.ToInt32(result.OutputDataSet.Tables[0].Rows[0]["current_execution_status"]);
                        switch (baseDTO.JobResult.ActionResult)
                        {
                            case 0:
                                baseDTO.JobResult.ResultMsg = "Returns only those jobs that are not idle or suspended.";
                                break;
                            case 1:
                                baseDTO.JobResult.ResultMsg = "Executing.";
                                break;
                            case 2:
                                baseDTO.JobResult.ResultMsg = "Waiting for thread.";
                                break;
                            case 3:
                                baseDTO.JobResult.ResultMsg = "Between retries.";
                                break;
                            case 4:
                                baseDTO.JobResult.ResultMsg = "Idle.";
                                break;
                            case 5:
                                baseDTO.JobResult.ResultMsg = "Suspended.";
                                break;
                            case 7:
                                baseDTO.JobResult.ResultMsg = "Performing completion actions.";
                                break;
                        }
                    }
                }
            }
            return baseDTO;
        }
        protected BaseDTO StartJob(BaseDTO baseDTO)
        {
            if (!baseDTO.JobName.IsNullOrEmpty())
            {
                try
                {
                    baseDTO.JobResult = new DTOResult();
                    baseDTO = GetJobStatus(baseDTO);
                    if (baseDTO.JobResult.ActionResult == 4)
                    {
                        var result = _DBMangerNoEF.ExecuteNonQuery(string.Format("exec [msdb].[dbo].[sp_start_job] N'{0}';", baseDTO.JobName), null, CommandType.Text);
                    }
                    else
                    {
                        baseDTO.JobResult.IsResult = false;
                    }
                }
                catch (Exception ex)
                {
                    baseDTO.Result.IsResult = false;
                    baseDTO.Result.ActionResult = -1;
                    baseDTO.Result.ResultMsg = ex.Message;
                }
            }
            return baseDTO;
        }
        protected BaseDTO StopJob(BaseDTO baseDTO)
        {
            if (!baseDTO.JobName.IsNullOrEmpty())
            {
                try
                {
                    baseDTO.JobResult = new DTOResult();
                    var result = _DBMangerNoEF.ExecuteNonQuery(string.Format("exec [msdb].[dbo].[sp_stop_job] N'{0}';", baseDTO.JobName), null, CommandType.Text);
                }
                catch (Exception ex)
                {
                    baseDTO.Result.IsResult = false;
                    baseDTO.Result.ActionResult = -1;
                    baseDTO.Result.ResultMsg = ex.Message;
                }
            }
            return baseDTO;
        }
    }
}
