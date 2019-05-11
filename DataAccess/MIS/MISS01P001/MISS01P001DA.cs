using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MIS
{
    public class MISS01P001DA : BaseDA
    {
        public MISS01P001DTO DTO { get; set; }

        #region ====Property========
        public MISS01P001DA()
        {
            DTO = new MISS01P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MISS01P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MISS01P001ExecuteType.GetAll: return GetAll(dto);
                case MISS01P001ExecuteType.GetByID: return GetByID(dto);
                case MISS01P001ExecuteType.GetNo: return GetNo(dto);
                case MISS01P001ExecuteType.GetReOpen: return GetReOpen(dto);
                case MISS01P001ExecuteType.GetExl: return GetExl(dto);
            }
            return dto;
        }
        private MISS01P001DTO GetExl(MISS01P001DTO dto)
        {
            string sql = @"SELECT COM_CODE
	                            ,NO
	                            ,FALG FLAG
	                            ,MSG ERROR_CODE
                            FROM VSMS_ISSUE_EXCEL";

            var result = _DBMangerNoEF.ExecuteDataSet(sql, null, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P001Model>();
            }

            return dto;
        }
        private MISS01P001DTO GetNo(MISS01P001DTO dto)
        {
            string strSQL = @"	SELECT TOP 1 NO
                                FROM VSMS_ISSUE
                                WHERE COM_CODE = @COM_CODE
                                ORDER BY No DESC";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model.NO = result.OutputDataSet.Tables[0].Rows[0][0].AsInt();
            }

            return dto;
        }
        private MISS01P001DTO GetAll(MISS01P001DTO dto)
        {
            string strSQL = @"	SELECT *
	                            FROM VSMS_ISSUE
	                            WHERE (1=1) ";
            var parameters = CreateParameter();            

            if (!dto.Model.APP_CODE.IsNullOrEmpty()) //checked
            {
                strSQL += " AND COM_CODE = @APP_CODE";
                parameters.AddParameter("APP_CODE", dto.Model.APP_CODE);
            }
            if (!dto.Model.NO.IsNullOrEmpty())
            {
                strSQL += " AND NO = @NO";
                parameters.AddParameter("NO", dto.Model.NO);
            }
            if (!dto.Model.MODULE.IsNullOrEmpty())
            {
                strSQL += " AND MODULE = @DEFECT";
                parameters.AddParameter("MODULE", dto.Model.DEFECT);
            }
            if (!dto.Model.STATUS.IsNullOrEmpty())
            {
                strSQL += " AND STATUS = @STATUS";
                parameters.AddParameter("STATUS", dto.Model.STATUS);
            }
            if (!dto.Model.RESPONSE_BY.IsNullOrEmpty())
            {
                strSQL += " AND RESPONSE_BY = @RESPONSE_BY";
                parameters.AddParameter("RESPONSE_BY", dto.Model.RESPONSE_BY);
            }
            if (!dto.Model.PRIORITY.IsNullOrEmpty())
            {
                strSQL += " AND PRIORITY = @PRIORITY";
                parameters.AddParameter("PRIORITY", dto.Model.PRIORITY);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P001Model>();
            }

            return dto;
        }
        private MISS01P001DTO GetByID(MISS01P001DTO dto)
        {
            string strSQL = @"	SELECT *
	                            FROM VSMS_ISSUE
	                            WHERE (1=1)
	                            AND COM_CODE = @COM_CODE
                                AND NO = @NO";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE); //checked
            parameters.AddParameter("NO", dto.Model.NO);
            
            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model = result.OutputDataSet.Tables[0].ToObject<MISS01P001Model>();
                dto.Model.APP_CODE = dto.Model.COM_CODE; //checked=
            }
            return dto;
        }
        private MISS01P001DTO GetReOpen(MISS01P001DTO dto)
        {
            dto.Model.COM_CODE = dto.Model.APP_CODE;
            GetNo(dto);

            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MISS01P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MISS01P001ExecuteType.Insert: return Insert(dto);
                case MISS01P001ExecuteType.SaveExl: return SaveExl(dto);
                case MISS01P001ExecuteType.CallSPInsertExcel: return CallSPInsertExcel(dto);
            }
            return dto;
        }
        private MISS01P001DTO Insert(MISS01P001DTO dto)
        {
            var parameters = CreateParameter();
            SplitDate(dto);

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE); //checked
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("ISSUE_DATE", dto.Model.ISSUE_DATE);
            parameters.AddParameter("ISSUE_DATE_PERIOD", dto.Model.ISSUE_DATE_PERIOD);
            parameters.AddParameter("MODULE", dto.Model.MODULE);
            parameters.AddParameter("DETAIL", dto.Model.DETAIL);
            parameters.AddParameter("ROOT_CAUSE", dto.Model.ROOT_CAUSE);
            parameters.AddParameter("SOLUTION", dto.Model.SOLUTION);
            parameters.AddParameter("EFFECTS", dto.Model.EFFECTS);
            parameters.AddParameter("ISSUE_BY", dto.Model.ISSUE_BY);
            parameters.AddParameter("RESPONSE_BY", dto.Model.RESPONSE_BY);
            parameters.AddParameter("TARGET_DATE", dto.Model.TARGET_DATE);
            parameters.AddParameter("RESPONSE_DATE", dto.Model.RESPONSE_DATE);
            parameters.AddParameter("RESPONSE_TARGET", dto.Model.RESPONSE_TARGET);
            parameters.AddParameter("RESOLUTION_TARGET", dto.Model.RESOLUTION_TARGET);
            parameters.AddParameter("ESSR_NO", dto.Model.ESSR_NO);
            parameters.AddParameter("CLOSE_DATE", dto.Model.CLOSE_DATE);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("DEFECT", dto.Model.DEFECT);
            parameters.AddParameter("PRIORITY", dto.Model.PRIORITY);
            parameters.AddParameter("REMARK", dto.Model.REMARK);
            parameters.AddParameter("MAN_PLM_SA", dto.Model.MAN_PLM_SA);
            parameters.AddParameter("MAN_PLM_QA", dto.Model.MAN_PLM_QA);
            parameters.AddParameter("MAN_PLM_PRG", dto.Model.MAN_PLM_PRG);
            parameters.AddParameter("MAN_PLM_PL", dto.Model.MAN_PLM_PL);
            parameters.AddParameter("MAN_PLM_DBA", dto.Model.MAN_PLM_DBA);
            parameters.AddParameter("ISSUE_IMG", dto.Model.ISSUE_IMG);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters.AddParameter("REF_NO", dto.Model.REF_NO);
            parameters.AddParameter("ISS_TYPE", dto.Model.ISS_TYPE);
            parameters.AddParameter("ISS_YEAR", dto.Model.ISS_YEAR);


            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_001]", parameters, CommandType.StoredProcedure);

            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                }
            }

            return dto;
        }
        public MISS01P001DTO SplitDate(MISS01P001DTO item)
        {
           item.Model.ISS_TYPE = item.Model.ISSUE_TYPE.Substring(0,1);
           item.Model.ISS_YEAR = item.Model.ISSUE_TYPE.Substring(1);            

            return item;
        }
        private MISS01P001DTO SaveExl(MISS01P001DTO dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("CLIENT_ID", dto.Model.CLIENT_ID);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_007]", parameters, CommandType.StoredProcedure);

            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                }
            }

            return dto;
        }
        private MISS01P001DTO CallSPInsertExcel(MISS01P001DTO dto)
        {
            if (dto.Result.IsResult)
            {
                DelTempExl();

                var ds = dto.Model.ds;
                var CLIENT_ID = dto.Model.CLIENT_ID;

                foreach (var item in ds.Tables[0].ToList<MISS01P001Model>())
                {
                    var parameters = CreateParameter();

                    parameters.AddParameter("error_code", null, ParameterDirection.Output);
                    parameters.AddParameter("APP_CODE", item.COM_CODE);
                    parameters.AddParameter("SEQ", null);
                    parameters.AddParameter("CLIENT_ID", CLIENT_ID);
                    parameters.AddParameter("NO", item.NO);
                    parameters.AddParameter("REF_NO", item.REF_NO);
                    parameters.AddParameter("STATUS", item.STATUS);
                    parameters.AddParameter("ISSUE_DATE", item.ISSUE_DATE);
                    parameters.AddParameter("ISSUE_DATE_PERIOD", item.ISSUE_DATE_PERIOD);
                    parameters.AddParameter("MODULE", item.MODULE);
                    parameters.AddParameter("DETAIL", item.DETAIL);
                    parameters.AddParameter("ROOT_CAUSE", item.ROOT_CAUSE);
                    parameters.AddParameter("SOLUTION", item.SOLUTION);
                    parameters.AddParameter("EFFECTS", item.EFFECTS);
                    parameters.AddParameter("ISSUE_BY", item.ISSUE_BY);
                    parameters.AddParameter("RESPONSE_BY", item.RESPONSE_BY);
                    parameters.AddParameter("TARGET_DATE", item.TARGET_DATE);
                    parameters.AddParameter("RESPONSE_DATE", item.RESPONSE_DATE);
                    parameters.AddParameter("FILE_ID", item.FILE_ID);
                    parameters.AddParameter("RESPONSE_TARGET", item.RESPONSE_TARGET);
                    parameters.AddParameter("RESOLUTION_TARGET", item.RESOLUTION_TARGET);
                    parameters.AddParameter("DEPLOY_QA", item.DEPLOY_QA);
                    parameters.AddParameter("DEPLOY_PD", item.DEPLOY_PD);
                    parameters.AddParameter("ESSR_NO", item.ESSR_NO);
                    parameters.AddParameter("CLOSE_DATE", item.CLOSE_DATE);
                    parameters.AddParameter("ISSUE_TYPE", item.ISSUE_TYPE);
                    parameters.AddParameter("DEFECT", item.DEFECT);
                    parameters.AddParameter("PRIORITY", item.PRIORITY);
                    parameters.AddParameter("REMARK", item.REMARK);
                    parameters.AddParameter("MAN_PLM_SA", item.MAN_PLM_SA);
                    parameters.AddParameter("MAN_PLM_QA", item.MAN_PLM_QA);
                    parameters.AddParameter("MAN_PLM_PRG", item.MAN_PLM_PRG);
                    parameters.AddParameter("MAN_PLM_PL", item.MAN_PLM_PL);
                    parameters.AddParameter("MAN_PLM_DBA", item.MAN_PLM_DBA);
                    parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);

                    var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_005]", parameters, CommandType.StoredProcedure);
                    //dto.Model.ERROR_CODE = result.OutputData["error_code"].ToString().Trim();

                    if (!result.Status)
                    {
                        dto.Result.IsResult = false;
                        dto.Result.ResultMsg = result.ErrorMessage;
                        break;
                    }
                    else
                    {
                        if (result.OutputData["error_code"].ToString().Trim() != "0")
                        {
                            dto.Result.IsResult = false;
                            dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                        }
                    }
                }
            }

            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (MISS01P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MISS01P001ExecuteType.Update: return Update(dto);
                case MISS01P001ExecuteType.ValidateExl: return ValidateExl(dto);
            }
            return dto;
        }
        private MISS01P001DTO Update(MISS01P001DTO dto)
        {
            var parameters = CreateParameter();
            SplitDate(dto);

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE); //checked
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("ISSUE_DATE", dto.Model.ISSUE_DATE);
            parameters.AddParameter("ISSUE_DATE_PERIOD", dto.Model.ISSUE_DATE_PERIOD);
            parameters.AddParameter("MODULE", dto.Model.MODULE);
            parameters.AddParameter("DETAIL", dto.Model.DETAIL);
            parameters.AddParameter("ROOT_CAUSE", dto.Model.ROOT_CAUSE);
            parameters.AddParameter("SOLUTION", dto.Model.SOLUTION);
            parameters.AddParameter("EFFECTS", dto.Model.EFFECTS);
            parameters.AddParameter("ISSUE_BY", dto.Model.ISSUE_BY);
            parameters.AddParameter("RESPONSE_BY", dto.Model.RESPONSE_BY);
            parameters.AddParameter("RESPONSE_DATE", dto.Model.RESPONSE_DATE);
            parameters.AddParameter("RESPONSE_TARGET", dto.Model.RESPONSE_TARGET);
            parameters.AddParameter("RESOLUTION_TARGET", dto.Model.RESOLUTION_TARGET);
            parameters.AddParameter("ESSR_NO", dto.Model.ESSR_NO);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("DEFECT", dto.Model.DEFECT);
            parameters.AddParameter("PRIORITY", dto.Model.PRIORITY);
            parameters.AddParameter("REMARK", dto.Model.REMARK);
            parameters.AddParameter("MAN_PLM_SA", dto.Model.MAN_PLM_SA);
            parameters.AddParameter("MAN_PLM_QA", dto.Model.MAN_PLM_QA);
            parameters.AddParameter("MAN_PLM_PRG", dto.Model.MAN_PLM_PRG);
            parameters.AddParameter("MAN_PLM_PL", dto.Model.MAN_PLM_PL);
            parameters.AddParameter("MAN_PLM_DBA", dto.Model.MAN_PLM_DBA);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters.AddParameter("REF_NO", dto.Model.REF_NO);
            parameters.AddParameter("CLOSE_DATE", dto.Model.CLOSE_DATE);
            parameters.AddParameter("DEPLOY_QA", dto.Model.DEPLOY_QA);
            parameters.AddParameter("DEPLOY_PD", dto.Model.DEPLOY_PD);
            parameters.AddParameter("ISS_TYPE", dto.Model.ISS_TYPE);
            parameters.AddParameter("ISS_YEAR", dto.Model.ISS_YEAR);
            parameters.AddParameter("TARGET_DATE", dto.Model.TARGET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_004]", parameters, CommandType.StoredProcedure);

            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                }
            }

            return dto;
        }
        private MISS01P001DTO ValidateExl(MISS01P001DTO dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE);
            parameters.AddParameter("CLIENT_ID ", dto.Model.CLIENT_ID);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_006]", parameters, CommandType.StoredProcedure);

            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                }
                else
                {
                    dto.Model.ERROR_CODE = result.OutputData["error_code"].ToString().Trim();
                }
            }

            return dto;
        }
        private void DelTempExl()
        {
            //clear data
            string strSQL;
            strSQL = "DELETE FROM VSMS_ISSUE_EXCEL";
            var result = _DBMangerNoEF.ExecuteNonQuery(strSQL, null, CommandType.Text);
        }
        #endregion
    }
}