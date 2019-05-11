using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using UtilityLib;
using System.Threading;
using System.Configuration;
using System.Web;

namespace DataAccess
{
    public static class DADataHelper
    {
        public static void AddParameter(this List<SqlDBParameter> parameters, string parameterName, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            var param = new SqlDBParameter();
            param.DBType = SqlDBType.None;
            param.Direction = parameterDirection;
            param.ParameterName = parameterName;
            param.Size = (parameterDirection == ParameterDirection.Output || parameterDirection == ParameterDirection.InputOutput) ? 2000 : 0;
            param.Value = !value.IsNullOrEmpty() ? value : null;
            parameters.Add(param);
        }

        public static void AddParameter(this List<SqlDBParameter> parameters, string parameterName, object value, ParameterDirection parameterDirection, int size)
        {
            var param = new SqlDBParameter();
            param.DBType = SqlDBType.None;
            param.Direction = parameterDirection;
            param.ParameterName = parameterName;
            param.Size = size;
            param.Value = !value.IsNullOrEmpty() ? value : null;
            parameters.Add(param);
        }

        public static void AddParameter(this List<SqlDBParameter> parameters, string parameterName, object value, SqlDBType dbType, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            var param = new SqlDBParameter();
            param.DBType = dbType;
            param.Direction = parameterDirection;
            param.ParameterName = parameterName;
            param.Value = !value.IsNullOrEmpty() ? value : null;
            parameters.Add(param);
        }

        public static int GetValue(this SqlDBParameter parameter)
        {
            return !parameter.Value.IsNullOrEmpty() ? Convert.ToInt32(Convert.ToString(parameter.Value)) : 0;
        }

        public static bool Success(this ExecuteResult dataSetResult, BaseDTO dto)
        {
            dto.Result.IsResult = dataSetResult.Status;
            if (!dataSetResult.Status)
            {
                dto.Result = new DTOResult();
                dto.Result.ActionResult = -1;
                dto.Result.IsResult = dataSetResult.Status;
                dto.Result.ResultMsg = dataSetResult.ErrorMessage;
            }
            return dataSetResult.Status;
        }
        public static List<DDLCenterModel> ToListDDLCenter(this DataTable table)
        {
            try
            {
                var list = new List<DDLCenterModel>();
                foreach (var row in table.AsEnumerable())
                {
                    var obj = new DDLCenterModel();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);

                            if (prop.Name.Equals("Text"))
                            {
                                var culture = Thread.CurrentThread.CurrentUICulture;

                                var colTB = "DDL_TEXT_TH";
                                if (culture.Name == "en-US")
                                {
                                    colTB = "DDL_TEXT_EN";
                                }
                                if (table.Columns.Contains(colTB))
                                    propertyInfo.SetValue(obj, row[colTB] == DBNull.Value || Extensions.IsNullOrEmpty(row[colTB]) ? null : Convert.ChangeType(row[colTB], Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);

                            }
                            else if (prop.Name.Equals("Value"))
                            {
                                var colTB = "DDL_VALUE";
                                if (table.Columns.Contains(colTB))
                                    propertyInfo.SetValue(obj, row[colTB] == DBNull.Value || Extensions.IsNullOrEmpty(row[colTB]) ? null : Convert.ChangeType(row[colTB], Extensions.IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType), null);

                            }
                            else
                            {
                                if (table.Columns.Contains(prop.Name))
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

        public static List<TreeModel> ToTreeView(this List<TreeModel> data)
        {
            try
            {
                var newData = new List<TreeModel>();
                var parent = data.Where(m => m.parent_id == null).ToList();
                if (parent.Count > 0)
                {
                    newData.AddRange(parent);
                    foreach (var item in parent)
                    {
                        AddNodeToTreeView(item, data);
                    }
                }


                return newData;
            }
            catch (Exception ex)
            {
                //ex.Log();
                return null;
            }
        }

        public static TreeModel AddNodeToTreeView(TreeModel node, List<TreeModel> allData)
        {
            node.state.@checked = node.selected;
            node.state.expanded = node.selected;
            node.state.selected = node.selected;
            var nodes = allData.Where(m => m.parent_id == node.id).ToList();
            if (nodes.Count > 0)
            {
                node.nodes = nodes;
                foreach (var item in nodes)
                {
                    AddNodeToTreeView(item, allData);
                }
            }
            return node;
        }

        public static string GetConnectionString(this string connectionName)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DBConnnectionConfig>>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/DBConnection.json")));
            var connection = json.Where(m => m.Name == connectionName).Select(m => m.ConnectionString).FirstOrDefault();
            var isEncript = ConfigurationManager.AppSettings["EncriptDBConnect"];
            if (isEncript == "true")
            {
                connection = connection.ToAsciiString();
            }
            return connection;
        }
        public static string GetConnectionStringEF(this string connectionName, string connectionNameEF)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DBConnnectionConfig>>(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/DBConnection.json")));
            var connectionEF = json.Where(m => m.Name == connectionNameEF).Select(m => m.ConnectionString).FirstOrDefault();
            var connection = json.Where(m => m.Name == connectionName).Select(m => m.ConnectionString).FirstOrDefault();
            var isEncript = ConfigurationManager.AppSettings["EncriptDBConnect"];
            if (isEncript == "true")
            {
                connection = connection.ToAsciiString();
            }
            connection = connectionEF.Replace("{tpConnectionString}", connection);
            return connection;
        }

        public static string ConcatSqlCommand(this string cmd, string pameterName, object value, int charLength = -1)
        {
            if (!value.IsNullOrEmpty())
            {
                if (value.AsString().StartsWith("%") || value.AsString().EndsWith("%"))
                {
                    cmd += " and " + pameterName + " like '" + value + "'";
                }
                else
                {
                    if (value.GetType() == typeof(string))
                    {
                        if (charLength > 0)
                        {
                            cmd += " and " + pameterName + "='" + value.AsChar(charLength) + "'";
                        }
                        else
                        {
                            cmd += " and " + pameterName + "='" + value + "'";
                        }
                    }
                    else if (value.GetType() == typeof(DateTime))
                    {
                        cmd += " and " + pameterName + "='" + value.AsString("yyyy-MM-dd") + "'";
                    }
                    else
                    {
                        cmd += " and " + pameterName + "=" + value;
                    }
                }
            }
            return cmd;
        }
        public static string GetOrderBy(this List<GridOrderModel> orders, List<GridColumnModel> columns,string defaultOrderBy)
        {
            if ((orders != null && orders.Count > 0)
                && (columns!=null&& columns.Count>0))
            {
                defaultOrderBy = string.Empty;
                foreach (var item in orders)
                {
                    defaultOrderBy += columns[item.column].data;
                    if (item.dir == "desc")
                    {
                        defaultOrderBy += " desc";
                    }
                    defaultOrderBy += ",";
                }
                defaultOrderBy = defaultOrderBy.TrimEnd(',');
            }
            return defaultOrderBy;
        }
    }
}
