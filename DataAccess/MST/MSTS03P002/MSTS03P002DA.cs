using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MST
{
    public class MSTS03P002DA : BaseDA
    {
        public MSTS03P002DTO DTO { get; set; }

        #region ====Property========
        public MSTS03P002DA()
        {
            DTO = new MSTS03P002DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MSTS03P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P002ExecuteType.GetAll: return GetAll(dto);
                case MSTS03P002ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private MSTS03P002DTO GetAll(MSTS03P002DTO dto)
        {
            string strSQL = @"SELECT * FROM [dbo].[VSMS_PIT_DATA] WHERE (1=1) AND KEY_ID = 'I' ";
            var parameters = CreateParameter();

            if (!dto.Model.ISSUE_TYPE.IsNullOrEmpty())
            {
                strSQL += "AND ISSUE_TYPE like @ISSUE_TYPE";
                parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            }

            if (!dto.Model.APP_CODE.IsNullOrEmpty())
            {
                strSQL += " AND COM_CODE = @APP_CODE";
                parameters.AddParameter("APP_CODE", dto.Model.APP_CODE);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MSTS03P002Model>();
            }

            return dto;
        }
        private MSTS03P002DTO GetByID(MSTS03P002DTO dto)
        {
            var parameters = CreateParameter();
            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("code", null, ParameterDirection.Output);
            parameters.AddParameter("PIT_ID", dto.Model.PIT_ID);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_PIT_DATA_004]", parameters, CommandType.StoredProcedure);

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
                    dto.Model = result.OutputDataSet.Tables[0].ToObject<MSTS03P002Model>();
                    dto.Model.APP_CODE = dto.Model.COM_CODE;
                    if (result.OutputData["code"].ToString().Trim() == "0")
                        dto.Model.IS_USED = true;
                }
            }
         
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MSTS03P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P002ExecuteType.Insert: return Insert(dto);
            }
            return dto;
        }
        private MSTS03P002DTO Insert(MSTS03P002DTO dto)
        {
            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE ", dto.Model.APP_CODE); //checked
            parameters.AddParameter("KEY_ID", "I");
            parameters.AddParameter("PRIORITY_NAME", dto.Model.PRIORITY_NAME);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("RES_TYPE", dto.Model.RES_TYPE);
            parameters.AddParameter("T_RES_TYPE", dto.Model.T_RES_TYPE);
            parameters.AddParameter("RES_TIME", dto.Model.RES_TIME);
            parameters.AddParameter("T_RES_TIME", dto.Model.T_RES_TIME);
            parameters.AddParameter("IS_FREE", dto.Model.IS_FREE);
            parameters.AddParameter("IS_CONS", dto.Model.IS_CONS);
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
            var dto = (MSTS03P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS03P002ExecuteType.Update: return Update(dto);
            }
            return dto;
        }
        private MSTS03P002DTO Update(MSTS03P002DTO dto)
        {
            string SQL = @"UPDATE VSMS_PIT_DATA SET
			                    [ISSUE_TYPE] = @ISSUE_TYPE
			                    ,[REMASK] = @REMASK
			                    ,[IS_FREE] = @IS_FREE
                                ,[IS_CONS] = @IS_CONS
			                    ,[MNT_BY] = @CRET_BY
			                    ,[MNT_DATE] = @CRET_DATE
			                WHERE PIT_ID = @PIT_ID
			                AND COM_CODE = @COM_CODE";

            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE ", dto.Model.COM_CODE); //checked
            parameters.AddParameter("PIT_ID", dto.Model.PIT_ID);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("IS_FREE", dto.Model.IS_FREE);
            parameters.AddParameter("IS_CONS", dto.Model.IS_CONS);
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
            var dto = (MSTS03P002DTO)baseDTO;
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
                        dto.Result.ResultMsg = "Issue type is used!";
                        break;
                    }
                }
            }

            return dto;
        }
        private bool CheckUse(MSTS03P002Model dto)
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