using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using UtilityLib;

namespace DataAccess
{
    public class SqlDBManger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction _Transaction;
        private SqlConnection _connection = null;

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

        private bool _IsTransaction = false;
        public bool IsTransaction
        {
            get { return _IsTransaction; }
        }

        private string ConnectionString
        {
            get
            {
                return ConnectionName.GetConnectionString();
            }
        }
        private SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        public ExecuteResult ExecuteCmdDataSetPaging(string commandText, string orderby, int start, int length, List<SqlDBParameter> parameters = null)
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

                var command = new SqlCommand
                {
                    Connection = _connection,
                    CommandType = CommandType.Text,
                    CommandTimeout = 0
                };

                if (_IsTransaction && _Transaction != null)
                {
                    command.Transaction = _Transaction;
                }

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new SqlParameter();
                        if (param.DBType != SqlDBType.None)
                        {
                            parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), param.DBType.ToString());
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
                command.CommandText = "select count(*) from (" + commandText + ") a";
                result.data = command.ExecuteScalar();

                command.CommandText = commandText + " order by " + orderby + " OFFSET " + start + " rows FETCH NEXT " + length + " ROWS ONLY";
                result.OutputDataSet = new DataSet();
                using (var dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(result.OutputDataSet);
                }
                #region  Change
                //command.CommandText = commandText + " order by " + orderby + " OFFSET " + start + " rows FETCH NEXT " + (length + 1) + " ROWS ONLY";
                //var dataSet = new DataSet();              
                //using (var dataAdapter = new SqlDataAdapter())
                //{
                //    dataAdapter.SelectCommand = command;
                //    dataAdapter.Fill(dataSet);             
                //}
                //result.data = dataSet.Tables[0].Rows.Count;
                //if (dataSet.Tables[0].Rows.Count > length)
                //{
                //    dataSet.Tables[0].Rows[length].Delete();
                //}
                //result.OutputDataSet = dataSet;
                #endregion
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
                result.Status = false;
                result.ErrorMessage = ex.Message;
                log.Error(Environment.NewLine + "ExecuteDataSet " + commandText, ex);
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
        public ExecuteResult ExecuteDataSet(string commandText, List<SqlDBParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure)
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

                //if (parameters != null && parameters.Count > 0)
                //{
                //    foreach (var param in parameters)
                //    {
                //        if (param.Value.GetType().Name.ToLower() == "string")
                //        {
                //            string name = "@" + param.ParameterName;
                //            commandText = commandText.Replace(name, "+N''+ " + name);
                //        }
                //    }
                //}

                var command = new SqlCommand
                {
                    Connection = _connection,
                    CommandText = commandText,
                    CommandType = commandType,
                    CommandTimeout = 0
                };

                if (_IsTransaction && _Transaction != null)
                {
                    command.Transaction = _Transaction;
                }

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new SqlParameter();
                        if (param.DBType != SqlDBType.None)
                        {
                            parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), param.DBType.ToString());
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
                result.OutputDataSet = new DataSet();
                using (var dataAdapter = new SqlDataAdapter())
                {
                    dataAdapter.SelectCommand = command;
                    dataAdapter.Fill(result.OutputDataSet);
                }
                if (parameters != null && parameters.Count > 0)
                {
                    var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);
                    if (output.Any())
                    {
                        result.OutputData = new Dictionary<string, string>();
                        foreach (var item in output)
                        {
                            result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                        }
                    }
                }
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                log.Error(Environment.NewLine + "ExecuteDataSet " + commandText, ex);
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
        public ExecuteResult ExecuteScalar(string commandText, List<SqlDBParameter> parameters = null, CommandType commandType = CommandType.Text)
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

                var command = new SqlCommand
                {
                    Connection = _connection,
                    CommandText = commandText,
                    CommandType = commandType,
                    CommandTimeout = 0
                };

                if (_IsTransaction && _Transaction != null)
                {
                    command.Transaction = _Transaction;
                }

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new SqlParameter();
                        if (param.DBType != SqlDBType.None)
                        {
                            parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), param.DBType.ToString());
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
                result.data = command.ExecuteScalar();
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = ex.Message;
                log.Error(Environment.NewLine + "ExecuteDataSet " + commandText, ex);
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
        public ExecuteResult ExecuteNonQuery(string commandText, List<SqlDBParameter> parameters = null, CommandType commandType = CommandType.StoredProcedure, bool existDataSet = false)
        {
            var result = new ExecuteResult();
            if (_connection == null)
            {
                _connection = CreateConnection();
                if (!_IsOpened)
                {
                    _connection.Open();
                    _IsOpened = true;
                }
            }
            try
            {
                //if (parameters != null && parameters.Count > 0)
                //{
                //    foreach (var param in parameters)
                //    {
                //        if (param.Value.GetType().Name.ToLower() == "string")
                //        {
                //            string name = "@" + param.ParameterName;
                //            commandText = commandText.Replace(name, "+N''+ " + name);
                //        }
                //    }
                //}

                var command = new SqlCommand
                {
                    Connection = _connection,
                    CommandText = commandText,
                    CommandType = commandType,
                    CommandTimeout = 0
                };

                if (parameters != null && parameters.Count > 0)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new SqlParameter();
                        if (param.DBType != SqlDBType.None)
                        {
                            parameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), param.DBType.ToString());
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
                if (_IsTransaction)
                {
                    command.Transaction = _Transaction;
                }
                var status = command.ExecuteNonQuery();
                if (existDataSet)
                {
                    result.OutputDataSet = new DataSet();
                    using (var dataAdapter = new SqlDataAdapter())
                    {
                        dataAdapter.SelectCommand = command;
                        dataAdapter.Fill(result.OutputDataSet);
                    }
                }
                if (parameters != null && parameters.Count > 0)
                {
                    var output = parameters.Where(m => m.Direction == ParameterDirection.InputOutput || m.Direction == ParameterDirection.Output);
                    if (output.Any())
                    {
                        result.OutputData = new Dictionary<string, string>();
                        foreach (var item in output)
                        {
                            result.OutputData.Add(item.ParameterName, Convert.ToString(command.Parameters[item.ParameterName].Value));
                        }
                    }
                }
                result.data = status;
                result.Status = status != -1;
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
                if (!_IsTransaction)
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
            if (_connection == null)
            {
                _connection = CreateConnection();
                if (!_IsOpened)
                {
                    _connection.Open();
                    _IsOpened = true;
                }
            }
            _Transaction = _connection.BeginTransaction();
            _IsTransaction = true;
        }
        public void CommitTransaction()
        {
            if (_IsTransaction && _Transaction != null)
            {
                _Transaction.Commit();
                _connection.Close();
                _connection.Dispose();
                _connection = null;
                _IsOpened = false;
                _IsTransaction = false;
                _Transaction = null;
            }
        }
        public void RollbackTransaction()
        {
            if (_IsTransaction && _Transaction != null)
            {
                _Transaction.Rollback();
                _connection.Close();
                _connection = null;
                _IsOpened = false;
                _IsTransaction = false;
                _Transaction = null;
            }
        }
    }
}
