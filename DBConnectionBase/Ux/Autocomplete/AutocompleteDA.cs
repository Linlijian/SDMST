using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.Ux
{
    public class AutocompleteDA : BaseDA
    {
        public AutocompleteDTO DTO
        {
            get;
            set;
        }

        public AutocompleteDA()
        {
            DTO = new AutocompleteDTO();
        }

        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (AutocompleteDTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case AutocompleteExecuteType.GetAll: return GetAll(dto);
                case AutocompleteExecuteType.GetStructure: return GetStructure(dto);
                case AutocompleteExecuteType.GetDataOnly: return GetDataOnly(dto);
                case AutocompleteExecuteType.GetValidate: return GetValidate(dto);
            }
            return dto;
        }
        private AutocompleteDTO GetAll(AutocompleteDTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("record_count", null, ParameterDirection.Output);
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("total_rows", null, ParameterDirection.Output);
            parameters.AddParameter("page_size", dto.pageSize);
            parameters.AddParameter("page_index", dto.pageIndex);
            parameters.AddParameter("pKEY_ID", dto.Parameter.KeySource);
            parameters.AddParameter("pSearchText", dto.Parameter.SearchTerm);
            parameters.AddParameter("pParameterValues", dto.Parameter.ParameterValue);
            parameters.AddParameter("pFitterBy", dto.Parameter.Sort);

            var result = _DBMangerNoEF.ExecuteDataSet("SP_AUTOCOMPTP0201", parameters);
            var record_count = result.OutputData["record_count"].AsInt();
            if (result.Success(dto) && record_count >= 0)
            {
                dto.totalcount = result.OutputData["total_rows"].AsInt();
                dto.rows = result.OutputDataSet.Tables[0].ToListDictionary();
                dto.colModel = result.OutputDataSet.Tables[1].ToList<AutocompleteColumnModel>();
            }
            return dto;
        }
        private AutocompleteDTO GetStructure(AutocompleteDTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("record_count", null, ParameterDirection.Output);
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("pKEY_ID", dto.Parameter.KeySource);

            var result = _DBMangerNoEF.ExecuteDataSet("SP_AUTOCOMPTP0204", parameters);
            var record_count = result.OutputData["record_count"].AsInt();
            if (result.Success(dto) && record_count >= 0)
            {
                dto.colModel = result.OutputDataSet.Tables[0].ToList<AutocompleteColumnModel>();
                if (result.OutputDataSet.Tables.Count > 1)
                {
                    dto.colKeyModel = result.OutputDataSet.Tables[1].ToList<AutocompleteColumnModel>();
                }
            }
            return dto;
        }
        private AutocompleteDTO GetDataOnly(AutocompleteDTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("record_count", null, ParameterDirection.Output);
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("total_rows", null, ParameterDirection.Output);
            parameters.AddParameter("page_size", dto.pageSize);
            parameters.AddParameter("page_index", dto.pageIndex);
            parameters.AddParameter("pKEY_ID", dto.Parameter.KeySource);
            parameters.AddParameter("pSearchText", dto.Parameter.SearchTerm);
            parameters.AddParameter("pParameterValues", dto.Parameter.ParameterValue);
            parameters.AddParameter("pFitterBy", dto.Parameter.Sort);
            parameters.AddParameter("pClientID", dto.Parameter.ClientID);
            parameters.AddParameter("pFitterData", dto.Parameter.FitterData);

            var result = _DBMangerNoEF.ExecuteDataSet("SP_AUTOCOMPTP0202", parameters);
            var record_count = result.OutputData["record_count"].AsInt();
            if (result.Success(dto) && record_count >= 0)
            {
                dto.totalcount = result.OutputData["total_rows"].AsInt();
                dto.rows = result.OutputDataSet.Tables[0].ToListDictionary();
                //dto.colModel = result.OutputDataSet.Tables[1].ToList<AutocompleteColumnModel>();
            }
            return dto;
        }
        private AutocompleteDTO GetValidate(AutocompleteDTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("record_count", null, ParameterDirection.Output);
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("pKEY_ID", dto.Parameter.KeySource);
            parameters.AddParameter("pParameterValues", dto.Parameter.ParameterValue);

            var result = _DBMangerNoEF.ExecuteDataSet("SP_AUTOCOMPTP0203", parameters);
            var record_count = result.OutputData["record_count"].AsInt();
            if (result.Success(dto) && record_count >= 0)
            {
                dto.rows = result.OutputDataSet.Tables[0].ToListDictionary();
            }
            return dto;
        }

        #region Insert
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (AutocompleteDTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case AutocompleteExecuteType.InsertNotInTable:
                    return InsertNotInTable(dto);
            }

            return dto;
        }

        private AutocompleteDTO InsertNotInTable(AutocompleteDTO dto)
        {
            DeleteNotInTable(dto);

            if (dto.Result.IsResult)
            {
                foreach (var item in dto.Parameter.DataNotIn)
                {
                    string strSQL = string.Format(@"    
                                                INSERT INTO VSMS_AUTOCOMPLETE_NOTIN (
                                                        CLIENT_ID,
                                                        COLUMN_NAME,
                                                        DATA_VALUE,
                                                        CRET_BY,
                                                        CRET_DATE
                                                ) VALUES (
                                                        @CLIENT_ID,
                                                        @COLUMN_NAME,
                                                        @DATA_VALUE,
                                                        @CRET_BY,
                                                        @CRET_DATE
                                                )");

                    var parameters = CreateParameter();

                    parameters.AddParameter("CLIENT_ID", dto.Parameter.ClientID);
                    parameters.AddParameter("COLUMN_NAME", item.COLUMN_NAME);
                    parameters.AddParameter("DATA_VALUE", item.DATA_VALUE.TrimEnd(','));
                    parameters.AddParameter("CRET_BY", dto.Parameter.CRET_BY);
                    parameters.AddParameter("CRET_DATE", dto.Parameter.CRET_DATE);

                    var result = _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);

                    if (!result.Success(dto))
                    {
                        break;
                    }
                }
            }

            return dto;
        }
        private AutocompleteDTO DeleteNotInTable(AutocompleteDTO dto)
        {
            string strSQL = string.Format(@"    delete from [dbo].[VSMS_AUTOCOMPLETE_NOTIN]
                                                where (1=1) ");

            var parameters = CreateParameter();
            if (!dto.Parameter.ClientID.IsNullOrEmpty())
            {
                strSQL += " and CLIENT_ID = @CLIENT_ID ";
                parameters.AddParameter("CLIENT_ID", dto.Parameter.ClientID);
            }
            else
            {
                strSQL += " and (1=2) ";
            }

            var result = _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
            }

            return dto;
        }
        #endregion

        #region delete
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (AutocompleteDTO)baseDTO;

            DeleteNotInTable(dto);

            return dto;
        }
        #endregion
    }
}
