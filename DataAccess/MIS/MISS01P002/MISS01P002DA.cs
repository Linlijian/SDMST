using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MIS
{
    public class MISS01P002DA : BaseDA
    {
        public MISS01P002DTO DTO { get; set; }

        #region ====Property========
        public MISS01P002DA()
        {
            DTO = new MISS01P002DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MISS01P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MISS01P002ExecuteType.GetAllOpening: return GetAllOpening(dto);
                case MISS01P002ExecuteType.GetAllOnProcess: return GetAllOnProcess(dto);
                case MISS01P002ExecuteType.GetByIdOpening: return GetByIdOpening(dto);
                case MISS01P002ExecuteType.GetFiexd: return GetFiexd(dto);
                case MISS01P002ExecuteType.GetAllFixed: return GetAllFixed(dto);
                case MISS01P002ExecuteType.GetAllFollowUp: return GetAllFollowUp(dto);
                case MISS01P002ExecuteType.GetAllGolive: return GetAllGolive(dto);
                case MISS01P002ExecuteType.GetFilePacket: return GetFilePacket(dto);
                case MISS01P002ExecuteType.GetAllClose: return GetAllClose(dto);
            }
            return dto;
        }
        private MISS01P002DTO GetAllClose(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT t.COM_CODE
	                                ,t.ISE_NO
	                                ,t.RESPONSE_BY
	                                ,t.ISE_DATE_CLOSE
	                                ,t.ASSIGN_STATUS
	                                ,tt.COM_NAME_E
	                                ,t.ISE_KEY
	                                ,(
		                                CASE t.ISE_STATUS
			                                WHEN 'S'
				                                THEN 'Close'
			                                ELSE 'Cancel'
			                                END
		                             ) ISE_STATUS
	                                ,t.ISE_DATE_OPENING
	                                ,t.ISSUE_BY
                                FROM VSMS_COMPANY tt
                                INNER JOIN (
	                                SELECT t.COM_CODE
		                                ,t.ISE_NO
		                                ,tt.RESPONSE_BY
		                                ,t.ISE_DATE_CLOSE
		                                ,t.ASSIGN_STATUS
		                                ,t.ISE_KEY
		                                ,t.ISE_STATUS
		                                ,t.ISE_DATE_OPENING
		                                ,tt.ISSUE_BY
                                        ,t.USER_ID
	                                FROM VSMS_ISSTATOPSS t
	                                INNER JOIN VSMS_ISSUE tt ON t.COM_CODE = tt.COM_CODE
		                                AND t.ISE_NO = tt.NO
	                                ) t ON t.COM_CODE = tt.COM_CODE
                                WHERE (1 = 1)
	                                AND t.ASSIGN_STATUS in ('E', 'C')
	                                AND t.ISE_STATUS in ('S', 'C') 
                                     ";

            var parameters = CreateParameter();

            if (!dto.Model.COM_CODE.IsNullOrEmpty())
            {
                strSQL += " AND t.COM_CODE = @COM_CODE";
                parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            }
            if (!dto.Model.ASSIGN_USER.IsNullOrEmpty())
            {
                strSQL += " AND t.USER_ID = @ASSIGN_USER";
                parameters.AddParameter("ASSIGN_USER", dto.Model.ASSIGN_USER);
            }
            if (!dto.Model.ISSUE_DATE_F.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_CLOSE >= @ISSUE_DATE_F";
                parameters.AddParameter("ISSUE_DATE_F", dto.Model.ISSUE_DATE_F);
            }
            if (!dto.Model.ISSUE_DATE_T.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_CLOSE <= @ISSUE_DATE_T";
                parameters.AddParameter("ISSUE_DATE_T", dto.Model.ISSUE_DATE_T);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetAllGolive(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE);
            parameters.AddParameter("RESPONSE_BY", dto.Model.ASSIGN_USER);
            parameters.AddParameter("ISSUE_DATE_F", dto.Model.ISSUE_DATE_F);
            parameters.AddParameter("ISSUE_DATE_T", dto.Model.ISSUE_DATE_T);
            parameters.AddParameter("TIMEOUT", dto.Model.TIMEOUT);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_005]", parameters);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetAllFollowUp(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT t.COM_CODE
	                                ,t.ISE_NO
	                                ,t.RESPONSE_BY
	                                ,t.ISE_DATE_FOLLOWUP
	                                ,t.ASSIGN_STATUS
	                                ,tt.COM_NAME_E
	                                ,t.ISE_KEY
	                                ,t.ISE_STATUS
	                                ,t.ISE_DATE_OPENING
	                                ,t.USER_ID
	                                ,t.ISSUE_BY
	                                ,t.DEPLOY_QA
	                                ,t.DEPLOY_PD
                                FROM VSMS_COMPANY tt
                                INNER JOIN (
	                                SELECT t.COM_CODE
		                                ,t.ISE_NO
		                                ,tt.RESPONSE_BY
		                                ,t.ISE_DATE_FOLLOWUP
		                                ,t.ASSIGN_STATUS
		                                ,t.ISE_KEY
		                                ,t.ISE_STATUS
		                                ,t.ISE_DATE_OPENING
		                                ,t.USER_ID
		                                ,tt.ISSUE_BY
		                                ,tt.DEPLOY_QA
		                                ,tt.DEPLOY_PD
	                                FROM VSMS_ISSTATOPSS t
	                                INNER JOIN VSMS_ISSUE tt ON t.COM_CODE = tt.COM_CODE
		                                AND t.ISE_NO = tt.NO
	                                ) t ON t.COM_CODE = tt.COM_CODE
                                WHERE (1 = 1)
	                                AND t.ASSIGN_STATUS = 'E'
	                                AND t.ISE_STATUS = 'F' ";

            var parameters = CreateParameter();

            if (!dto.Model.COM_CODE.IsNullOrEmpty())
            {
                strSQL += " AND t.COM_CODE = @COM_CODE";
                parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            }
            if (!dto.Model.ASSIGN_USER.IsNullOrEmpty())
            {
                strSQL += " AND t.USER_ID = @ASSIGN_USER";
                parameters.AddParameter("ASSIGN_USER", dto.Model.ASSIGN_USER);
            }
            if (!dto.Model.ISSUE_DATE_F.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_FOLLOWUP >= @ISSUE_DATE_F";
                parameters.AddParameter("ISSUE_DATE_F", dto.Model.ISSUE_DATE_F);
            }
            if (!dto.Model.ISSUE_DATE_T.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_FOLLOWUP <= @ISSUE_DATE_T";
                parameters.AddParameter("ISSUE_DATE_T", dto.Model.ISSUE_DATE_T);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetAllOnProcess(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT t.COM_CODE
	                                ,t.ISE_NO
	                                ,t.RESPONSE_BY
	                                ,t.ISE_DATE_ONPROCESS
	                                ,t.ASSIGN_STATUS
	                                ,tt.COM_NAME_E
	                                ,t.ISE_KEY
	                                ,t.ISE_STATUS
                                    ,t.ISE_DATE_OPENING
                                    ,t.USER_ID
                                FROM VSMS_COMPANY tt
                                INNER JOIN (
	                                SELECT t.COM_CODE
		                                ,t.ISE_NO
		                                ,tt.RESPONSE_BY
		                                ,t.ISE_DATE_ONPROCESS
		                                ,t.ASSIGN_STATUS
		                                ,t.ISE_KEY
		                                ,t.ISE_STATUS
                                        ,t.ISE_DATE_OPENING
                                        ,t.USER_ID
	                                FROM VSMS_ISSTATOPSS t
	                                INNER JOIN VSMS_ISSUE tt ON t.COM_CODE = tt.COM_CODE
		                                AND t.ISE_NO = tt.NO
	                                ) t ON t.COM_CODE = tt.COM_CODE
                                WHERE (1=1) 
                                AND t.ASSIGN_STATUS <> 'W' 
                                AND t.ISE_STATUS = 'P' ";

            var parameters = CreateParameter();

            if (!dto.Model.COM_CODE.IsNullOrEmpty())
            {
                strSQL += " AND t.COM_CODE = @COM_CODE";
                parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            }
            if (!dto.Model.ASSIGN_USER.IsNullOrEmpty())
            {
                strSQL += " AND t.USER_ID = @ASSIGN_USER";
                parameters.AddParameter("ASSIGN_USER", dto.Model.ASSIGN_USER);
            }
            if (!dto.Model.ISSUE_DATE_F.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_ONPROCESS >= @ISSUE_DATE_F";
                parameters.AddParameter("ISSUE_DATE_F", dto.Model.ISSUE_DATE_F);
            }
            if (!dto.Model.ISSUE_DATE_T.IsNullOrEmpty())
            {
                strSQL += " AND t.ISE_DATE_ONPROCESS <= @ISSUE_DATE_T";
                parameters.AddParameter("ISSUE_DATE_T", dto.Model.ISSUE_DATE_T);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetAllFixed(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT *
                                FROM VSMS_ISSTATOPSS
                                WHERE USER_ID = @USER_ID
                                ";

            var parameters = CreateParameter();
            parameters.AddParameter("USER_ID", dto.Model.ASSIGN_USER);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetAllOpening(MISS01P002DTO dto)
        {
            //string strSQL = "	SELECT --distinct
            //                     t.COM_CODE
            //                     ,t.no
            //                     ,t.RESPONSE_BY as RESPONSE_BY
            //                     ,t.ISSUE_DATE as ISSUE_DATE_T
            //                     ,tt.USER_ID as ASSIGN_USER
            //                    FROM VSMS_ISSUE t
            //                    INNER JOIN VSMS_ISSTATOPSS tt ON t.COM_CODE = tt.COM_CODE
            //                     AND tt.ise_no = t.NO
            //                    WHERE (1 = 1)
            //                     AND tt.ASSIGN_STATUS != 'C' ";


            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("APP_CODE", dto.Model.APP_CODE);
            parameters.AddParameter("RESPONSE_BY", dto.Model.ASSIGN_USER);
            parameters.AddParameter("ISSUE_DATE_F", dto.Model.ISSUE_DATE_F);
            parameters.AddParameter("ISSUE_DATE_T", dto.Model.ISSUE_DATE_T);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_001]", parameters);
            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetByIdOpening(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT *
                                FROM [dbo].[VSMS_ISSUE]
                                WHERE (1 = 1)
                                AND COM_CODE = @COM_CODE
                                AND NO = @NO";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE); //checked
            parameters.AddParameter("NO", dto.Model.NO);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model = result.OutputDataSet.Tables[0].ToObject<MISS01P002Model>();
            }
            return dto;
        }
        private MISS01P002DTO GetFiexd(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("Total", null, ParameterDirection.Output);
            parameters.AddParameter("Complete", null, ParameterDirection.Output);
            parameters.AddParameter("Incomplete", null, ParameterDirection.Output);
            parameters.AddParameter("USER_ID", dto.Model.ASSIGN_USER);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_002]", parameters);
            if (result.Success(dto))
            {
                dto.Model.Total = result.OutputData["Total"].AsInt();
                dto.Model.Complete = result.OutputData["Complete"].AsInt();
                dto.Model.Incomplete = result.OutputData["Incomplete"].AsInt();
            }
            return dto;
        }
        private MISS01P002DTO GetFilePacket(MISS01P002DTO dto)
        {
            string strSQL = @"	SELECT *
                                FROM VSMS_ISSUE
                                WHERE COM_CODE = @COM_CODE
                                AND NO = @NO";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("NO", dto.Model.ISE_NO); //cheked

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model = result.OutputDataSet.Tables[0].ToObject<MISS01P002Model>();
            }

            return dto;
        }

        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MISS01P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                //case MISS01P002ExecuteType.Insert: return Insert(dto);
            }
            return dto;
        }
        private MISS01P002DTO Insert(MISS01P002DTO dto)
        {

            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (MISS01P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MISS01P002ExecuteType.UpdateAssignment: return UpdateAssignment(dto);
                case MISS01P002ExecuteType.ConfirmTest: return ConfirmTest(dto);
                case MISS01P002ExecuteType.MoveToFollowUp: return ConfirmTest(dto);
                case MISS01P002ExecuteType.MoveToGolive: return ConfirmTest(dto);
                case MISS01P002ExecuteType.UpdateFilePacket: return UpdateFilePacket(dto);
                case MISS01P002ExecuteType.TimeStemp: return TimeStemp(dto);
                case MISS01P002ExecuteType.MoveToClose: return ConfirmTest(dto);
                case MISS01P002ExecuteType.MoveToCancel: return ConfirmCancel(dto);
                case MISS01P002ExecuteType.ReDo: return ConfirmReDo(dto);
            }
            return dto;
        }
        private MISS01P002DTO ConfirmReDo(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("ISE_KEY", dto.Model.ISE_KEY);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_007]", parameters);
            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
                dto.Model.FLAG = "N";
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                    dto.Model.FLAG = result.OutputData["error_code"].ToString().Trim();
                }
                else
                {
                    dto.Model.FLAG = "Y";
                }
            }

            return dto;
        }
        private MISS01P002DTO ConfirmCancel(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("FLAG", dto.Model.FLAG);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_006]", parameters);
            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
                dto.Model.FLAG = "N";
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                    dto.Model.FLAG = result.OutputData["error_code"].ToString().Trim();
                }
                else
                {
                    dto.Model.FLAG = "Y";
                }
            }

            return dto;
        }
        private MISS01P002DTO TimeStemp(MISS01P002DTO dto)
        {
            if (dto.Models.Count() > 0)
            {
                var FLAG = dto.Model.FLAG;
                foreach (var item in dto.Models)
                {
                    var parameters = CreateParameter();
                    parameters.AddParameter("error_code", null, ParameterDirection.Output);
                    parameters.AddParameter("COM_CODE", item.COM_CODE);
                    parameters.AddParameter("NO",item.ISE_NO);
                    parameters.AddParameter("FALG", FLAG);
                    parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);

                    var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_004]", parameters);
                    if (!result.Status)
                    {
                        dto.Result.IsResult = false;
                        dto.Result.ResultMsg = result.ErrorMessage;
                        dto.Model.FLAG = "N";
                        break;
                    }
                    else
                    {
                        if (result.OutputData["error_code"].ToString().Trim() != "0")
                        {
                            dto.Result.IsResult = false;
                            dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                            dto.Model.FLAG = result.OutputData["error_code"].ToString().Trim();
                            break;
                        }
                        else
                        {
                            dto.Model.FLAG = "Y";
                        }
                    }
                }
            }
            else
            {
                dto.Model.FLAG = "Are you select list yet?";
            }
            
            return dto;
        }
        private MISS01P002DTO ConfirmTest(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("ISE_KEY", dto.Model.ISE_KEY);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("FALG", dto.Model.FLAG);
            parameters.AddParameter("NO", dto.Model.ISE_NO);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSTATOPSS_003]", parameters);
            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
                dto.Model.FLAG = "N";
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    dto.Result.IsResult = false;
                    dto.Result.ResultMsg = result.OutputData["error_code"].ToString().Trim();
                    dto.Model.FLAG = result.OutputData["error_code"].ToString().Trim();
                }
                else
                {
                    dto.Model.FLAG = "Y";
                }
            }

            return dto;
        }
        private MISS01P002DTO UpdateAssignment(MISS01P002DTO dto)
        {

            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE);
            parameters.AddParameter("ASSIGN_USER", dto.Model.ASSIGN_USER);
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_002]", parameters, CommandType.StoredProcedure);

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
        private MISS01P002DTO UpdateFilePacket(MISS01P002DTO dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("FILE_ID", dto.Model.FILE_ID);
            parameters.AddParameter("NO", dto.Model.NO);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_ISSUE_003]", parameters, CommandType.StoredProcedure);

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
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (MISS02P001DTO)baseDTO;
            if (dto.Models.Count() > 0)
            {
                foreach (var item in dto.Models)
                {

                }
            }

            return dto;
        }
        #endregion
    }
}