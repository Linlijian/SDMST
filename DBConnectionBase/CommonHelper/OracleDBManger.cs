using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using UtilityLib;

namespace DataAccess
{
    public class OracleDBManger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private OracleTransaction _Transaction;
        private OracleConnection _connection = null;

        private bool IsCaseSensitive
        {
            get
            {
                if (WebConfigurationManager.AppSettings["IsCaseSensitive"] == null)
                    return true; // Default Case Sensitive

                return Convert.ToBoolean(WebConfigurationManager.AppSettings["IsCaseSensitive"]);
            }
        }

        private string _ConnectionName = "Connection";
        public string ConnectionName
        {
            get
            {
                return _ConnectionName;
            }
            set
            {
                _ConnectionName = value;
            }
        }

        private bool _IsOpened = false;
        public bool IsOpened
        {
            get { return _IsOpened; }
        }

        private string ConnectionString
        {
            get
            {
                return ConnectionName.GetConnectionString(); ;
            }
        }
        private OracleConnection CreateConnection()
        {
            return new OracleConnection(ConnectionString);
        }
        public ExecuteResult ExecuteDataSet(string commandText, List<OracleDBParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var result = new ExecuteResult();
            if (_connection == null)
            {
                _connection = CreateConnection();
            }
            try
            {
                if (!_IsOpened)
                {
                    _connection.Open();
                    _IsOpened = true;
                }

                var command = new OracleCommand { Connection = _connection, BindByName = true };

                if (!IsCaseSensitive)
                {
                    SetNoCaseSensitive(command);
                }

                command.CommandText = commandText;
                command.CommandType = commandType;

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new OracleParameter();
                        if (param.DBType != OracleDBType.None)
                        {
                            parameter.OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), param.DBType.ToString());
                        }
                        parameter.Direction = param.Direction;
                        parameter.ParameterName = param.ParameterName;
                        parameter.Size = param.Size;

                        if (string.IsNullOrEmpty(Convert.ToString(param.Value)))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        else
                        {
                            parameter.Value = param.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
                try
                {
                    result.OutputDataSet = new DataSet();
                    using (var dataAdapter = new OracleDataAdapter())
                    {
                        dataAdapter.SelectCommand = command;
                        dataAdapter.Fill(result.OutputDataSet);
                    }
                    var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);
                    if (output.Any())
                    {
                        result.OutputData = new Dictionary<string, string>();
                        foreach (var item in output)
                        {
                            result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                        }
                    }
                    result.Status = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("existing state of package body"))
                    {
                        result.OutputDataSet = new DataSet();
                        using (var dataAdapter = new OracleDataAdapter())
                        {
                            dataAdapter.SelectCommand = command;
                            dataAdapter.Fill(result.OutputDataSet);
                        }

                        var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);
                        if (output.Any())
                        {
                            result.OutputData = new Dictionary<string, string>();
                            foreach (var item in output)
                            {
                                result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                            }
                        }
                        result.Status = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                log.Error(Environment.NewLine + "ExecuteDataSet:" + commandText, ex);
                //throw ex;
            }
            finally
            {
                if (_Transaction == null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    _IsOpened = false;
                }
            }
            return result;
        }

        public ExecuteResult ExecuteNonQuery(string commandText, List<OracleDBParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            var result = new ExecuteResult();
            bool IsUseTran = true;
            if (_connection == null)
            {
                _connection = CreateConnection();
                if (!_IsOpened)
                {
                    _connection.Open();
                    _IsOpened = true;
                }
                IsUseTran = false;
            }
            try
            {
                var command = new OracleCommand { Connection = _connection, BindByName = true };

                if (!IsCaseSensitive)
                {
                    SetNoCaseSensitive(command);
                }

                command.CommandText = commandText;
                command.CommandType = commandType;

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new OracleParameter();
                        if (param.DBType != OracleDBType.None)
                        {
                            parameter.OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), param.DBType.ToString());
                        }
                        parameter.Direction = param.Direction;
                        parameter.ParameterName = param.ParameterName;
                        parameter.Size = param.Size;

                        if (string.IsNullOrEmpty(Convert.ToString(param.Value)))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        else if (param.DBType == OracleDBType.None && param.Value.GetType() == typeof(string))
                        {
                            parameter.Value = Convert.ToString(param.Value).Trim();
                        }
                        else
                        {
                            parameter.Value = param.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
                if (IsUseTran)
                {
                    command.Transaction = _Transaction;
                }
                try
                {
                    var status = command.ExecuteNonQuery();
                    var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);

                    if (output.Any())
                    {
                        if (output.Where(m => m.DBType == OracleDBType.RefCursor).Any())
                        {
                            result.OutputDataSet = new DataSet();
                        }
                        result.OutputData = new Dictionary<string, string>();
                        foreach (var item in output)
                        {
                            if (command.Parameters[item.ParameterName].OracleDbType == OracleDbType.RefCursor)
                            {
                                var value = (Oracle.ManagedDataAccess.Types.OracleRefCursor)command.Parameters[item.ParameterName].Value;
                                if (!value.IsNull)
                                {
                                    var dataReader = value.GetDataReader();
                                    var dt = new DataTable();
                                    dt.Load(dataReader);
                                    result.OutputDataSet.Tables.Add(dt);
                                }
                            }
                            else
                            {
                                result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                            }
                        }
                    }
                    result.Status = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("existing state of package body"))
                    {
                        var status = command.ExecuteNonQuery();
                        var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);

                        if (output.Any())
                        {
                            if (output.Where(m => m.DBType == OracleDBType.RefCursor).Any())
                            {
                                result.OutputDataSet = new DataSet();
                            }
                            result.OutputData = new Dictionary<string, string>();
                            foreach (var item in output)
                            {
                                if (command.Parameters[item.ParameterName].OracleDbType == OracleDbType.RefCursor)
                                {
                                    var value = (Oracle.ManagedDataAccess.Types.OracleRefCursor)command.Parameters[item.ParameterName].Value;
                                    if (!value.IsNull)
                                    {
                                        var dataReader = value.GetDataReader();
                                        var dt = new DataTable();
                                        dt.Load(dataReader);
                                        result.OutputDataSet.Tables.Add(dt);
                                    }
                                }
                                else
                                {
                                    result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                                }
                            }
                        }
                        result.Status = true;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                log.Error(Environment.NewLine + "ExecuteNonQuery:" + commandText, ex);
                //throw ex;
            }
            finally
            {
                if (!IsUseTran)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    _IsOpened = false;
                }
            }
            return result;
        }

        public void BeginTransaction()
        {
            _connection = CreateConnection();
            if (!_IsOpened)
            {
                _connection.Open();
                _IsOpened = true;
            }
            _Transaction = _connection.BeginTransaction();
        }
        public void CommitTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Commit();
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                _IsOpened = false;
            }
        }
        public void RollbackTransaction()
        {
            if (_Transaction != null)
            {
                _Transaction.Rollback();
                _connection.Close();
                _connection = null;
                _IsOpened = false;
            }
        }
        private void SetNoCaseSensitive(OracleCommand command)
        {
            command.CommandText = "alter session set NLS_COMP=LINGUISTIC";
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            //command.CommandText = "alter session set NLS_SORT=THAI_DICTIONARY_CI";
            command.CommandText = "alter session set NLS_SORT=BINARY_CI";
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();

            command.CommandText = string.Empty;
        }

    }
}
