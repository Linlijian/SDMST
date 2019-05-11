using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECS02P002DA : BaseDA
    {
        public SECS02P002DTO DTO { get; set; }

        #region ====Property========
        public SECS02P002DA()
        {
            DTO = new SECS02P002DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SECS02P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS02P002ExecuteType.GetAll: return GetAll(dto);
                case SECS02P002ExecuteType.GetByID: return GetByID(dto);
                case SECS02P002ExecuteType.GetQuerySearchAll:return GetQuerySearchAll(dto);
                case SECS02P002ExecuteType.GetQueryCheckUserAdmin:return GetQueryCheckUserAdmin(dto);
                case SECS02P002ExecuteType.GetDetailByID: return GetDetailByID(dto);
                case SECS02P002ExecuteType.CheckAdmin: return CheckAdmin(dto);
            }
            return dto;
        }
        private SECS02P002DTO CheckAdmin(SECS02P002DTO dto)
        {
            string sql = @"SELECT USG_LEVEL
                            FROM VSMS_USRGROUP
                            WHERE USG_ID = @USG_ID";

            var parameters = CreateParameter();
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(sql, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model.IS_ADMIN = result.OutputDataSet.Tables[0].Rows[0][0].ToString();
            }

            return dto;
        }
        private SECS02P002DTO GetDetailByID(SECS02P002DTO dto)
        {
            string sql = @"SELECT COM_CODE,
                                MODULE,
                                USER_ID,
                                USG_ID = (SELECT USG_ID FROM VSMS_USER WHERE USER_ID = @USER_ID)
                          FROM VSMS_MODULE
                          WHERE USER_ID = @USER_ID";

            var parameters = CreateParameter();
            parameters.AddParameter("USER_ID", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(sql, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model.Details = result.OutputDataSet.Tables[0].ToList<SECS02P002ModuleAndSystemDetail>();
            }

            return dto;
        }
        private SECS02P002DTO GetAll(SECS02P002DTO dto)
        {
            dto.Models = _DBManger.VSMS_USER
                .Where(m => (m.USER_ID == dto.Model.USER_ID))
                .OrderBy(m => new { m.USER_ID })
                .Select(m => new SECS02P002Model
                {
                    COM_CODE = m.COM_CODE,
                    USER_ID = m.USER_ID,
                    USER_FNAME_TH = m.USER_FNAME_TH,
                    USER_LNAME_TH = m.USER_LNAME_TH,
                    USER_FNAME_EN = m.USER_FNAME_EN,
                    USER_LNAME_EN = m.USER_LNAME_EN,
                    TITLE_ID = m.TITLE_ID,
                    USG_ID = m.USG_ID,
                    USER_PWD = m.USER_PWD,
                    TELEPHONE = m.TELEPHONE,
                    EMAIL = m.EMAIL,
                    USER_STATUS = m.USER_STATUS,
                    CRET_BY = m.CRET_BY,
                    CRET_DATE = m.CRET_DATE,
                    MNT_BY = m.MNT_BY,
                    MNT_DATE = m.MNT_DATE,
                    IS_DISABLED = m.IS_DISABLED,
                    LAST_LOGIN_DATE = m.LAST_LOGIN_DATE,
                }).ToList();
            return dto;
        }
        private SECS02P002DTO GetByID(SECS02P002DTO dto)
        {
            dto.Model = _DBManger.VSMS_USER
                .Where(m => m.USER_ID == dto.Model.USER_ID)
                .FirstOrDefault().ToNewObject(new SECS02P002Model());
            return dto;
        }
        private SECS02P002DTO GetQuerySearchAll(SECS02P002DTO dto)
        {
            dto.Models = (
                        from a in _DBManger.VSMS_USER
                        join c in _DBManger.VSMS_USRGROUP on a.USG_ID equals c.USG_ID
                        into ac
                        from a_c in ac.DefaultIfEmpty()

                        where (dto.Model.USER_ID == null || a.USER_ID.Contains(dto.Model.USER_ID))
                                && (dto.Model.USER_FNAME_TH == null || a.USER_FNAME_TH.Contains(dto.Model.USER_FNAME_TH))
                                && (dto.Model.USER_FNAME_EN == null || a.USER_FNAME_EN.Contains(dto.Model.USER_FNAME_EN))
                                && (dto.Model.USG_ID == null || a.USG_ID == dto.Model.USG_ID)
                                && (dto.Model.IS_DISABLED == null || ((a.IS_DISABLED == null ? "N" : a.IS_DISABLED) == dto.Model.IS_DISABLED))
                        orderby a.USER_ID
                        select new SECS02P002Model
                        {
                            USER_ID = a.USER_ID,
                            USER_FNAME_TH = a.USER_FNAME_TH,
                            USER_LNAME_TH = a.USER_LNAME_TH,
                            USER_FNAME_EN = a.USER_FNAME_TH,
                            USER_LNAME_EN = a.USER_LNAME_EN,
                            USG_NAME_TH = a_c.USG_NAME_TH,
                            IS_DISABLED = a.IS_DISABLED,
                        }).ToList();

            //dto.Models = _DBManger.VSMS_USER
            //    .Where(m => (m.COM_CODE == dto.Model.COM_CODE))
            //    .OrderBy(m => new { m.USER_ID })
            //    .Select(m => new SECS02P002Model
            //    {
            //        COM_CODE = m.COM_CODE,
            //        USER_ID = m.USER_ID,
            //        USER_FNAME_TH = m.USER_FNAME_TH,
            //        USER_LNAME_TH = m.USER_LNAME_TH,
            //        USER_FNAME_EN = m.USER_FNAME_EN,
            //        USER_LNAME_EN = m.USER_LNAME_EN,
            //        TITLE_ID = m.TITLE_ID,
            //        DEPT_ID = m.DEPT_ID,
            //        USG_ID = m.USG_ID,
            //        USER_SPEC_ID = m.USER_SPEC_ID,
            //        USER_PWD = m.USER_PWD,
            //        USER_EFF_DATE = m.USER_EFF_DATE,
            //        USER_EXP_DATE = m.USER_EXP_DATE,
            //        PWD_EXP_DATE = m.PWD_EXP_DATE,
            //        WNING_USER_DATE = m.WNING_USER_DATE,
            //        WNING_PWD_DATE = m.WNING_PWD_DATE,
            //        END_ACT_DATE = m.END_ACT_DATE,
            //        TELEPHONE = m.TELEPHONE,
            //        EMAIL = m.EMAIL,
            //        USER_STATUS = m.USER_STATUS,
            //        IS_FCP = m.IS_FCP,
            //        IS_NCE = m.IS_NCE,
            //        CRET_BY = m.CRET_BY,
            //        CRET_DATE = m.CRET_DATE,
            //        MNT_BY = m.MNT_BY,
            //        MNT_DATE = m.MNT_DATE,
            //        IS_DISABLED = m.IS_DISABLED,
            //        LAST_LOGIN_DATE = m.LAST_LOGIN_DATE
            //    }).ToList();
            return dto;
        }
        private SECS02P002DTO GetQueryCheckUserAdmin(SECS02P002DTO dto)
        {
            string strSQL = @"  SELECT USG_LEVEL 
                                FROM VSMS_USRGROUP a inner join VSMS_USER b on a.COM_CODE = b.COM_CODE and a.USG_ID = b.USG_ID
                                WHERE USG_STATUS = 'E'  ";

            var parameters = CreateParameter();

            if (!dto.Model.COM_CODE.IsNullOrEmpty())
            {
                strSQL += " and b.com_code = @com_code ";
                parameters.AddParameter("com_code", dto.Model.COM_CODE);
            }

            if (!dto.Model.USER_ID.IsNullOrEmpty())
            {
                strSQL += " and b.USER_ID like '%' + @USER_ID + '%'";
                parameters.AddParameter("USER_ID", dto.Model.USER_ID);
            }

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Models = result.OutputDataSet.Tables[0].ToList<SECS02P002Model>();
            }

            return dto;
        }
        //private SECS02P002DTO GetUserCOM(SECS02P002DTO dto)
        //{
        //    string strSQL = @"  select VUC.*
		      //                          ,VC.COM_NAME_E
        //                        from VSMS_USERCOM VUC
	       //                         left join VSMS_COMPANY VC on VUC.COM_CODE = VC.COM_CODE
        //                        where (1=1) ";

        //    var parameters = CreateParameter();
          
        //    if (!dto.Model.USER_ID.IsNullOrEmpty())
        //    {
        //        strSQL += " and VUC.USER_ID = @USER_ID ";
        //        parameters.AddParameter("USER_ID", dto.Model.USER_ID);
        //    }

        //    var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

        //    if (result.Success(dto))
        //    {
        //        dto.Model.ComUserModel = result.OutputDataSet.Tables[0].ToList<SECS02P002_CompanyForUserModel>();
        //    }

        //    return dto;
        //}
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SECS02P002DTO)baseDTO;

            #region  insert VSMS_USER 
            string strSQL1 = @"INSERT INTO [dbo].[VSMS_USER]
                                       ([COM_CODE]
                                       ,[USER_ID]
                                       ,[USER_FNAME_TH]
                                       ,[USER_LNAME_TH]
                                       ,[USER_FNAME_EN]
                                       ,[USER_LNAME_EN]
                                       ,[TITLE_ID]
                                       ,[USG_ID]
                                       ,[USER_PWD]
                                       ,[TELEPHONE]
                                       ,[EMAIL]
                                       ,[USER_STATUS]
                                       ,[IS_DISABLED]
                                       ,[LAST_LOGIN_DATE]
                                       ,[CRET_BY]
                                       ,[CRET_DATE]
                                       ,[MNT_BY]
                                       ,[MNT_DATE])
                            VALUES
                                        (@COM_CODE 
                                        ,@USER_ID 
                                        ,@USER_FNAME_TH 
                                        ,@USER_LNAME_TH 
                                        ,@USER_FNAME_EN 
                                        ,@USER_LNAME_EN 
                                        ,@TITLE_ID 
                                        ,@USG_ID 
                                        ,@USER_PWD 
                                        ,@TELEPHONE 
                                        ,@EMAIL 
                                        ,@USER_STATUS 
                                        ,@IS_DISABLED 
                                        ,@LAST_LOGIN_DATE 
                                        ,@CRET_BY 
                                        ,@CRET_DATE 
                                        ,@MNT_BY 
                                        ,@MNT_DATE)";

            var parameters1 = CreateParameter();
            parameters1.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters1.AddParameter("TITLE_ID", dto.Model.TITLE_NAME_TH);
            parameters1.AddParameter("USER_ID", dto.Model.USER_ID);
            parameters1.AddParameter("USER_FNAME_TH", dto.Model.USER_FNAME_TH);
            parameters1.AddParameter("USER_LNAME_TH", dto.Model.USER_LNAME_TH);
            parameters1.AddParameter("USER_FNAME_EN", dto.Model.USER_FNAME_EN);
            parameters1.AddParameter("USER_LNAME_EN", dto.Model.USER_LNAME_EN);
            parameters1.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters1.AddParameter("USER_PWD", dto.Model.USER_PWD);
            parameters1.AddParameter("TELEPHONE", dto.Model.TELEPHONE);
            parameters1.AddParameter("EMAIL", dto.Model.EMAIL);
            parameters1.AddParameter("USER_STATUS", dto.Model.USER_STATUS);
            parameters1.AddParameter("IS_DISABLED", dto.Model.IS_DISABLED);
            parameters1.AddParameter("LAST_LOGIN_DATE", dto.Model.LAST_LOGIN_DATE);
            parameters1.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters1.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters1.AddParameter("MNT_BY", dto.Model.MNT_BY);
            parameters1.AddParameter("MNT_DATE", dto.Model.MNT_DATE);


            var result = _DBMangerNoEF.ExecuteNonQuery(strSQL1, parameters1, CommandType.Text);
            if (result.Success(dto))
            {
                INSERT_USERCOM(dto);
            }
            #endregion
            

            return dto;
        }

        private bool CHECK_DUP(SECS02P002DTO dto)
        {
            string strSQL = @"  select count(*) as Count 
                                from dbo.vsms_usercom 
                                where com_code = @pcom_code and user_id = @puser_id and usg_id = @pusg_id";

            var parameters = CreateParameter();

            parameters.AddParameter("pcom_code", dto.Model.APP_CODE);
            parameters.AddParameter("puser_id", dto.Model.USER_ID);
            parameters.AddParameter("pusg_id", dto.Model.USG_ID);
            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Model.IS_DUP = result.OutputDataSet.Tables[0].Rows[0][0].AsInt();
            }

            bool is_dup;
            if (dto.Model.IS_DUP > 0)
                is_dup = true;
            else
                is_dup = false;

            return is_dup;
        }

        private SECS02P002DTO INSERT_USERCOM(SECS02P002DTO dto)
        {
           if(dto.Model.IS_ADMIN != "S" && dto.Model.IS_ADMIN != "A" && !dto.Model.IS_ADMIN.IsNullOrEmpty())
            {
                if (!dto.Model.Details.IsNullOrEmpty())
                {
                    var UDG_ID = dto.Model.USG_ID;
                    DELETE_USERCOM(dto);
                    DELETE_MODUlE(dto);

                    foreach (var item in dto.Model.Details)
                    {
                        var parameters1 = CreateParameter();

                        parameters1.AddParameter("error_code", null, ParameterDirection.Output);
                        parameters1.AddParameter("COM_CODE", item.COM_CODE);
                        parameters1.AddParameter("USER_ID", item.USER_ID);
                        parameters1.AddParameter("USG_ID", UDG_ID);
                        parameters1.AddParameter("MODULE", item.MODULE);
                        parameters1.AddParameter("CRET_BY", dto.Model.CRET_BY);
                        parameters1.AddParameter("CRET_DATE", dto.Model.CRET_DATE);

                        var result = _DBMangerNoEF.ExecuteNonQuery("[bond].[SP_SECS02P002_001]", parameters1);

                        if (result.OutputData["error_code"] != "0")
                        {
                            dto.Result.IsResult = false;
                            dto.Result.ResultMsg = result.OutputData["error_code"].Trim();
                            break;
                        }
                    }
                }
            }
            return dto;
        }
        private SECS02P002DTO DELETE_MODUlE(SECS02P002DTO dto)
        {
            string strSQL = @" delete from dbo.VSMS_MODULE
                               where  user_id = @puser_id";

            var parameters = CreateParameter();
            parameters.AddParameter("puser_id", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (!result.Success(dto))
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }

            return dto;
        }
        private SECS02P002DTO DELETE_USERCOM(SECS02P002DTO dto)
        {
            string strSQL = @" delete from dbo.vsms_usercom
                               where  user_id = @puser_id";

            var parameters = CreateParameter();
            parameters.AddParameter("puser_id", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

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
            var dto = (SECS02P002DTO)baseDTO;
            string strSQL1 = @" UPDATE VSMS_USER  SET
                                    USER_FNAME_TH = @USER_FNAME_TH
                                    ,USER_LNAME_TH = @USER_LNAME_TH
                                    ,USER_FNAME_EN = @USER_FNAME_EN
                                    ,USER_LNAME_EN = @USER_LNAME_EN
                                    ,TITLE_ID = @TITLE_ID
                                    ,USG_ID = @USG_ID
                                    ,TELEPHONE = @TELEPHONE
                                    ,EMAIL = @EMAIL
                                    ,USER_STATUS = @USER_STATUS
                                    ,IS_DISABLED = @IS_DISABLED
                                    ,MNT_BY = @MNT_DATE
                                    ,MNT_DATE = @MNT_DATE
                                    ,COM_CODE = @COM_CODE
                              WHERE COM_CODE = @COM_CODE
                                    AND USER_ID = @USER_ID ";

            var parameters1 = CreateParameter();

            parameters1.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters1.AddParameter("USER_ID", dto.Model.USER_ID);

           // parameters1.AddParameter("APP_CODE", dto.Model.APP_CODE);
            parameters1.AddParameter("TITLE_ID", dto.Model.TITLE_NAME_TH);
            parameters1.AddParameter("USER_FNAME_TH", dto.Model.USER_FNAME_TH);
            parameters1.AddParameter("USER_LNAME_TH", dto.Model.USER_LNAME_TH);
            parameters1.AddParameter("USER_FNAME_EN", dto.Model.USER_FNAME_EN);
            parameters1.AddParameter("USER_LNAME_EN", dto.Model.USER_LNAME_EN);
            parameters1.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters1.AddParameter("TELEPHONE", dto.Model.TELEPHONE);
            parameters1.AddParameter("EMAIL", dto.Model.EMAIL);
            parameters1.AddParameter("USER_STATUS", dto.Model.USER_STATUS);
            parameters1.AddParameter("IS_DISABLED", dto.Model.IS_DISABLED);
            parameters1.AddParameter("MNT_BY", dto.Model.MNT_BY);
            parameters1.AddParameter("MNT_DATE", dto.Model.MNT_DATE);

            var result = _DBMangerNoEF.ExecuteNonQuery(strSQL1, parameters1, CommandType.Text);
            if (result.Success(dto))
            {
                INSERT_USERCOM(dto);
            }

            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SECS02P002DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS02P002ExecuteType.Delete: return Delete(dto);
                case SECS02P002ExecuteType.DeleteDetail: return DeleteDetail(dto);
                
            }
            return dto;
        }
        private SECS02P002DTO DeleteDetail(SECS02P002DTO dto)
        {
            if (dto.Models.Count() > 0)
            {
                foreach (var item in dto.Models)
                {
                    var items = _DBManger.VSMS_ISSUE.Where(m => m.COM_CODE == item.COM_CODE && m.MODULE == item.MODULE && m.RESPONSE_BY == item.USER_ID);

                    if (items.Count() > 0)
                    {
                        dto.Result.IsResult = false;
                        dto.Result.ResultMsg = "Data is useed!";
                        break;
                    }
                    else
                    {
                        dto.Result.IsResult = true;
                        //var del = _DBManger.VSMS_MODULE.Where(m => m.COM_CODE == item.COM_CODE && m.MODULE == item.MODULE && m.USER_ID == item.USER_ID);
                        //_DBManger.VSMS_MODULE.RemoveRange(del);
                    }
                }
            }


            return dto;
        }
        private SECS02P002DTO Delete(SECS02P002DTO dto)
        {
            foreach (var item in dto.Models)
            {
                var USER_ID = item.USER_ID.AsString();

                string strSQL = @" DELETE FROM [dbo].[VSMS_USER]
                                   WHERE USER_ID = @USER_ID  ";

                var parameters = CreateParameter();
                parameters.AddParameter("USER_ID", USER_ID);
                _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);

                strSQL = @" DELETE FROM [dbo].[VSMS_USERCOM]
                                   WHERE USER_ID = @USER_ID  ";
                parameters = CreateParameter();
                parameters.AddParameter("USER_ID", USER_ID);
                _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);

                strSQL = @" DELETE FROM [dbo].[VSMS_MODULE]
                                   WHERE USER_ID = @USER_ID  ";
                parameters = CreateParameter();
                parameters.AddParameter("USER_ID", USER_ID);
                _DBMangerNoEF.ExecuteNonQuery(strSQL, parameters, CommandType.Text);

            }
            return dto;
        }
        #endregion
    }
}