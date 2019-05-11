using System;
using System.Data;
using System.Globalization;
using System.Linq;
using UtilityLib;

namespace DataAccess.MST
{
    public class MSTS02P001DA : BaseDA
    {
        public MSTS02P001DTO DTO { get; set; }

        #region ====Property========
        public MSTS02P001DA()
        {
            DTO = new MSTS02P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (MSTS02P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS02P001ExecuteType.GetAll: return GetAll(dto);
                case MSTS02P001ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }

        private MSTS02P001DTO GetAll(MSTS02P001DTO dto)
        {
            string strSql = @"SELECT * FROM [dbo].[VSMS_MANDAY_T] WHERE (1=1) ";

            var parameters = CreateParameter();

            if (!dto.Model.APP_CODE.IsNullOrEmpty())
            {
                strSql += " AND COM_CODE = @APP_CODE";
                parameters.AddParameter("APP_CODE", dto.Model.APP_CODE); //checked
            }
            if (!dto.Model.YEAR.IsNullOrEmpty())
            {
                strSql += " AND YEAR = @YEAR";
                parameters.AddParameter("YEAR", dto.Model.YEAR);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSql, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<MSTS02P001Model>();
            }

            return dto;
        }
        private MSTS02P001DTO GetByID(MSTS02P001DTO dto)
        {
            string strSql = @"SELECT * FROM [dbo].[VSMS_MANDAY_T] 
                                WHERE (1=1) 
                                AND COM_CODE = @COM_CODE 
                                AND YEAR = @YEAR";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE); //checked
            parameters.AddParameter("YEAR", dto.Model.YEAR);

            var result = _DBMangerNoEF.ExecuteDataSet(strSql, parameters, commandType: CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model = result.OutputDataSet.Tables[0].ToObject<MSTS02P001Model>();
                dto.Model.APP_CODE = dto.Model.COM_CODE;
            }

            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (MSTS02P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS02P001ExecuteType.Insert: return Insert(dto);

            }
            return dto;
        }
        private MSTS02P001DTO Insert(MSTS02P001DTO dto)
        {
            string strSql = @"INSERT INTO [dbo].[VSMS_MANDAY_T]
                                ([COM_CODE]
                                ,[YEAR]
                                ,[MANDAY_VAL]
                                ,[IS_USE]
                                ,[CRET_BY]
                                ,[CRET_DATE]
                                ,[MNT_BY]
                                ,[MNT_DATE])
                             VALUES 
                                (@COM_CODE
                                ,@YEAR
                                ,CONVERT(decimal(8,4), @MANDAY_VAL)
                                ,@IS_USE
                                ,@CRET_BY
                                ,@CRET_DATE
                                ,@MNT_BY
                                ,@MNT_DATE)";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE); //checked
            parameters.AddParameter("YEAR", dto.Model.YEAR);
            parameters.AddParameter("MANDAY_VAL", dto.Model.MANDAY_VAL);
            parameters.AddParameter("IS_USE", dto.Model.IS_USE);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters.AddParameter("MNT_BY", dto.Model.MNT_BY);
            parameters.AddParameter("MNT_DATE", dto.Model.MNT_DATE);

            var result = _DBMangerNoEF.ExecuteNonQuery(strSql, parameters, CommandType.Text);
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
            var dto = (MSTS02P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case MSTS02P001ExecuteType.Update: return Update(dto);
            }
            return dto;
        }
        private MSTS02P001DTO Update(MSTS02P001DTO dto)
        {
            string strSql = @"UPDATE [dbo].[VSMS_MANDAY_T] SET
                                    MANDAY_VAL = @MANDAY_VAL,
                                    MNT_BY = @CRET_BY,
                                    MNT_DATE = @CRET_DATE
                             WHERE COM_CODE = @COM_CODE
                                    AND YEAR = @YEAR";

            var parameters = CreateParameter();
            parameters.AddParameter("YEAR", dto.Model.YEAR);
            parameters.AddParameter("COM_CODE", dto.Model.APP_CODE);

            parameters.AddParameter("MANDAY_VAL", dto.Model.MANDAY_VAL);
            parameters.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteNonQuery(strSql, parameters, CommandType.Text);
            if (!result.Success(dto))
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
            var dto = (MSTS02P001DTO)baseDTO;
            if (dto.Models.Count() > 0)
            {
                foreach (var item in dto.Models)
                {
                    if(item.IS_USE != "T")
                    {
                        string sql = @"DELETE FROM [dbo].[VSMS_MANDAY_T] 
                                        WHERE COM_CODE = @COM_CODE
                                        AND YEAR = @YEAR";
                        var parameters = CreateParameter();
                        parameters.AddParameter("YEAR", item.YEAR);
                        parameters.AddParameter("COM_CODE", item.COM_CODE); //checked

                        var result = _DBMangerNoEF.ExecuteNonQuery(sql, parameters, CommandType.Text);
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
                        dto.Result.ResultMsg = "Man/day MA All Application is used. Can not delete!";
                        break;
                    }
                }
            }

            return dto;
        }
        #endregion
    }
}