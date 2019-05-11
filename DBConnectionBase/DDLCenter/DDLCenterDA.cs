using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using UtilityLib;

namespace DataAccess
{
    public class DDLCenterDA : BaseDA
    {
        public DDLCenterDTO DTO
        {
            get;
            private set;
        }

        public DDLCenterDA()
        {
            DTO = new DDLCenterDTO();
        }

        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (DDLCenterDTO)baseDTO;
            var parameters = CreateParameter();

            parameters.AddParameter("record_count", null, ParameterDirection.Output);
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("pKEY_ID", dto.Parameter.KEY_ID);
            parameters.AddParameter("pParameterValues", dto.Parameter.ParameterValues);
            parameters.AddParameter("pWhereClause", dto.Parameter.WhereClause);
            parameters.AddParameter("pOrderBy", dto.Parameter.OrderBy);

            var result = _DBMangerNoEF.ExecuteDataSet("SP_DROPDOWNLIST", parameters);
            var error_code = result.OutputData["error_code"];
            var record_count = result.OutputData["record_count"];

            if (result.Success(dto) && record_count.AsInt() > -1)
            {
                dto.DDLCenters = ToListDDLCenter(result.OutputDataSet.Tables[0]);
            }
            return DTO;
        }

        public List<DDLCenterModel> ToListDDLCenter(DataTable table)
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
    }
}
