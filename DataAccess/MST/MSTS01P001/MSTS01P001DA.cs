using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MST
{
    public class MSTS01P001DA : BaseDA
    {
        public MSTS01P001DTO DTO { get; set; }

        #region ====Property========
        public MSTS01P001DA()
        {
            DTO = new MSTS01P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MSTS01P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS01P001ExecuteType.GetAll: return GetAll(dto);
                case MSTS01P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private MSTS01P001DTO GetAll(MSTS01P001DTO dto)
        {
            string strSQL = @"      SELECT *
                                    FROM [dbo].[VSMS_MANDAY]
                                    WHERE (1=1) ";

            var parameters = CreateParameter();

            if (!dto.Model.APP_CODE.IsNullOrEmpty()) //checked
            {
                strSQL += " AND COM_CODE = @COM_CODE";
                parameters.AddParameter("COM_CODE", dto.Model.APP_CODE);
            }
            if (!dto.Model.ISSUE_TYPE.IsNullOrEmpty())
            {
                strSQL += " AND ISSUE_TYPE = @ISSUE_TYPE";
                parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            }
            if (!dto.Model.TYPE_RATE.IsNullOrEmpty())
            {
                strSQL += " AND TYPE_RATE = @TYPE_RATE";
                parameters.AddParameter("TYPE_RATE", dto.Model.TYPE_RATE);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MSTS01P001Model>();
            }
            return dto;
        }
        private MSTS01P001DTO GetByID(MSTS01P001DTO dto)
        {
            string strSQL = @"      SELECT *
                                    FROM [dbo].[VSMS_MANDAY]
                                    WHERE (1=1) AND COM_CODE = @COM_CODE
                                        AND TYPE_RATE = @TYPE_RATE
                                        AND ISSUE_TYPE = @ISSUE_TYPE";

            var parameters = CreateParameter();
            parameters.AddParameter("TYPE_RATE", dto.Model.TYPE_RATE);
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE); //checked

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model = result.OutputDataSet.Tables[0].ToObject<MSTS01P001Model>();
                dto.Model.APP_CODE = dto.Model.COM_CODE; //checked
            }

            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MSTS01P001DTO)baseDTO;

            string strSQL1 = @"INSERT INTO [dbo].[VSMS_MANDAY]
                                       ([COM_CODE]
                                        ,[ISSUE_TYPE]
                                        ,[TYPE_RATE]
                                        ,[MAN_PLM_SA]
                                        ,[MAN_PLM_QA]
                                        ,[MAN_PLM_PRG]
                                        ,[CRET_BY]
                                        ,[CRET_DATE]
                                        ,[MNT_BY]
                                        ,[MNT_DATE])
                            VALUES
                                        (@COM_CODE
                                        ,@ISSUE_TYPE
                                        ,@TYPE_RATE
                                        ,@MAN_PLM_SA
                                        ,@MAN_PLM_QA
                                        ,@MAN_PLM_PRG
                                        ,@CRET_BY
                                        ,@CRET_DATE
                                        ,@MNT_BY
                                        ,@MNT_DATE)";

            var parameters1 = CreateParameter();
            parameters1.AddParameter("COM_CODE", dto.Model.COM_CODE); //checked
            parameters1.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters1.AddParameter("TYPE_RATE", dto.Model.TYPE_RATE);
            parameters1.AddParameter("MAN_PLM_SA", dto.Model.MAN_PLM_SA);
            parameters1.AddParameter("MAN_PLM_QA", dto.Model.MAN_PLM_QA);
            parameters1.AddParameter("MAN_PLM_PRG", dto.Model.MAN_PLM_PRG);
            parameters1.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters1.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters1.AddParameter("MNT_BY", dto.Model.MNT_BY);
            parameters1.AddParameter("MNT_DATE", dto.Model.MNT_DATE);

            var result = _DBMangerNoEF.ExecuteNonQuery(strSQL1, parameters1, CommandType.Text);
            if (!result.Success(dto))
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }

            return dto;
        }
        #endregion

        #region ====Update==========

        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (MSTS01P001DTO)baseDTO;

            var parameters = CreateParameter();

            parameters.AddParameter("error_code", null, ParameterDirection.Output);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE); //checked
            parameters.AddParameter("ISSUE_TYPE", dto.Model.ISSUE_TYPE);
            parameters.AddParameter("TYPE_RATE", dto.Model.TYPE_RATE);
            parameters.AddParameter("MAN_PLM_SA", dto.Model.MAN_PLM_SA);
            parameters.AddParameter("MAN_PLM_QA", dto.Model.MAN_PLM_QA);
            parameters.AddParameter("MAN_PLM_PRG", dto.Model.MAN_PLM_PRG);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteDataSet("[bond].[SP_VSMS_MANDAY_001]", parameters, CommandType.StoredProcedure);

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
            var dto = (MSTS01P001DTO)baseDTO;
            if (dto.Models.Count() > 0)
            {
                foreach (var item in dto.Models)
                {
                    if (!CheckUse(item))
                    {
                        string strSQL = @" DELETE FROM VSMS_MANDAY 
                                                    WHERE COM_CODE = @COM_CODE 
                                                    AND ISSUE_TYPE = @ISSUE_TYPE 
                                                    AND TYPE_RATE = @TYPE_RATE";

                        var parameters = CreateParameter();
                        parameters.AddParameter("COM_CODE", item.COM_CODE); //checked
                        parameters.AddParameter("ISSUE_TYPE", item.ISSUE_TYPE);
                        parameters.AddParameter("TYPE_RATE", item.TYPE_RATE);

                        var result = _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);
                        if (!result.Status)
                        {
                            dto.Result.IsResult = false;
                            dto.Result.ResultMsg = result.ErrorMessage;
                            break;
                        }
                    }
                    else
                    {
                        dto.Result.IsResult = false;
                        dto.Result.ResultMsg = "Standard Rate of MA Waranty is used";
                    }

                }
            }        
            return dto;
        }
        private bool CheckUse(MSTS01P001Model dto)
        {
            string strSQL = @" SELECT COUNT(*)
                                FROM VSMS_ISSUE A JOIN VSMS_MANDAY S
                                ON A.COM_CODE = S.COM_CODE
                                AND A.DEFECT = S.ISSUE_TYPE 
                                WHERE A.COM_CODE = @COM_CODE
                                AND A.DEFECT = @DEFECT";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.COM_CODE); //checked
            parameters.AddParameter("DEFECT", dto.ISSUE_TYPE);

           // var result = _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);
            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, commandType: CommandType.Text);

            bool isUse;
            string AA = string.Empty;
            if (result.Status)
            {
                 AA = result.OutputDataSet.Tables[0].Rows[0][0].AsString();
            }

            if (AA == "0")
                isUse = false;
            else
                isUse = true;

            return isUse;
        }
        #endregion
    }
}