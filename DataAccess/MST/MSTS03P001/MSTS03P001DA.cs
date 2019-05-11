using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MST
{
    public class MSTS03P001DA : BaseDA
    {
        public MSTS03P001DTO DTO { get; set; }

        #region ====Property========
        public MSTS03P001DA()
        {
            DTO = new MSTS03P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MSTS03P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P001ExecuteType.GetAll: return GetAll(dto);
                case MSTS03P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private MSTS03P001DTO GetAll(MSTS03P001DTO dto)
        {
            string strSQL = @"SELECT * FROM [dbo].[VSMS_PIT_DATA] 
                                WHERE (1=1)
                                AND KEY_ID = 'P' ";
            var parameters = CreateParameter();

            if (!dto.Model.RES_TYPE.IsNullOrEmpty())
            {
                strSQL += " AND RES_TYPE = @RES_TYPE";
                parameters.AddParameter("RES_TYPE", dto.Model.RES_TYPE);
            }

            if (!dto.Model.T_RES_TYPE.IsNullOrEmpty())
            {
                strSQL += " AND T_RES_TYPE = @T_RES_TYPE";
                parameters.AddParameter("T_RES_TYPE", dto.Model.T_RES_TYPE);
            }

            if (!dto.Model.APP_CODE.IsNullOrEmpty())
            {
                strSQL += " AND COM_CODE = @APP_CODE";
                parameters.AddParameter("APP_CODE", dto.Model.APP_CODE);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MSTS03P001Model>();
            }

            return dto;
        }
        private MSTS03P001DTO GetByID(MSTS03P001DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("code", null, ParameterDirection.Output);
            parameters.AddParameter("PIT_ID", dto.Model.PIT_ID);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_PIT_DATA_005]", parameters, CommandType.StoredProcedure);

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
                    dto.Model = result.OutputDataSet.Tables[0].ToObject<MSTS03P001Model>();
                    dto.Model.APP_CODE = dto.Model.COM_CODE;
                    if(result.OutputData["code"].ToString().Trim() == "0")
                        dto.Model.IS_USED = true;
                }
            }
         
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MSTS03P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P001ExecuteType.Insert: return Insert(dto);
            }
            return dto;
        }
        private MSTS03P001DTO Insert(MSTS03P001DTO dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE ", dto.Model.APP_CODE);
            parameters.AddParameter("KEY_ID", "P");
            parameters.AddParameter("PRIORITY_NAME", dto.Model.PRIORITY_NAME);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("RES_TYPE", dto.Model.RES_TYPE);
            parameters.AddParameter("T_RES_TYPE", dto.Model.T_RES_TYPE);
            parameters.AddParameter("RES_TIME", dto.Model.RES_TIME);
            parameters.AddParameter("T_RES_TIME", dto.Model.T_RES_TIME);
            parameters.AddParameter("IS_FREE", dto.Model.IS_FREE);
            parameters.AddParameter("IS_CONS", dto.Model.IS_FREE);
            parameters.AddParameter("REMASK", dto.Model.REMASK);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_PIT_DATA_001]", parameters, CommandType.StoredProcedure);

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

        #region ====Update==========
        
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (MSTS03P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P001ExecuteType.Update: return Update(dto);
            }
            return dto;
        }
        private MSTS03P001DTO Update(MSTS03P001DTO dto)
        {
            string SQL = @"UPDATE VSMS_PIT_DATA SET
				                [PRIORITY_NAME] = @PRIORITY_NAME
				                ,[RES_TIME] = @RES_TIME
				                ,[RES_TYPE] = @RES_TYPE
				                ,[T_RES_TIME] = @T_RES_TIME
				                ,[T_RES_TYPE] = @T_RES_TYPE
				                ,[REMASK] = @REMASK
				                ,[MNT_BY] = @CRET_BY
				                ,[MNT_DATE] = @CRET_DATE
			                WHERE PIT_ID = @PIT_ID
			                AND COM_CODE = @COM_CODE";

            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE ", dto.Model.COM_CODE);
            parameters.AddParameter("PIT_ID", dto.Model.PIT_ID);
            parameters.AddParameter("PRIORITY_NAME", dto.Model.PRIORITY_NAME);
            parameters.AddParameter("RES_TIME", dto.Model.RES_TIME);
            parameters.AddParameter("RES_TYPE", dto.Model.RES_TYPE);
            parameters.AddParameter("T_RES_TIME", dto.Model.T_RES_TIME);
            parameters.AddParameter("T_RES_TYPE", dto.Model.T_RES_TYPE);
            parameters.AddParameter("REMASK", dto.Model.REMASK);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet(SQL, parameters, commandType: CommandType.Text);

            if (!result.Status)
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (MSTS03P001DTO)baseDTO;
            if (dto.Models.Count() > 0)
            {
                foreach (var item in dto.Models)
                {
                    if (CheckUse(item))
                    {
                        string strSQL1 = @"DELETE FROM [dbo].[VSMS_PIT_DATA] 
                                            WHERE COM_CODE = @COM_CODE 
                                            AND PIT_ID = @PIT_ID";

                        var parameters1 = CreateParameter();

                        parameters1.AddParameter("COM_CODE", item.COM_CODE); //checked
                        parameters1.AddParameter("PIT_ID", item.PIT_ID);

                        var result = _DBMangerNoEF.ExecuteNonQuery(strSQL1, parameters1, CommandType.Text);
                        if (!result.Success(dto))
                        {
                            dto.Result.IsResult = false;
                            dto.Result.ResultMsg = result.ErrorMessage;
                            break;
                        }
                    }
                    else
                    {
                        dto.Result.IsResult = false;
                        dto.Result.ResultMsg = "Priority is used!";
                        break;
                    }
                }
            }

            return dto;
        }
        private bool CheckUse(MSTS03P001Model dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE ", dto.COM_CODE); //checked
            parameters.AddParameter("PRIORITY_NAME", dto.PRIORITY_NAME);
            parameters.AddParameter("ISSUE_TYPE", dto.ISSUE_TYPE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_PIT_DATA_003]", parameters, CommandType.StoredProcedure);

            bool isUse;
            if (!result.Status)
            {
                isUse = false;
            }
            else
            {
                if (result.OutputData["error_code"].ToString().Trim() != "0")
                {
                    isUse = false;
                }
                else
                {
                    isUse = true;
                }
            }
            return isUse;
        }
        #endregion
    }
}