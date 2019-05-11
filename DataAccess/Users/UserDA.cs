using DataAccess.SEC;
using System;
using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.Users
{
    public class UserDA : BaseDA
    {
        private UserDTO _DTO = null;
        public UserDTO DTO
        {
            get
            {
                if (_DTO == null)
                {
                    _DTO = new UserDTO();
                }
                return _DTO;
            }
        }

        #region Select
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            UserDTO dto = (UserDTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case UserExecuteType.GetUser: return GetUser(dto);
                case UserExecuteType.GetConfigGeraral: return GetConfigGeraral(dto);
                case UserExecuteType.GetConfigSys: return GetConfigSys(dto);
                case UserExecuteType.GetApp: return GetApp(dto);
                case UserExecuteType.GetNotification: return GetNotification(dto);
                case UserExecuteType.GetNotificationCount: return GetNotificationCount(dto);
                case UserExecuteType.FatchNotification: return FatchNotification(dto);
            }
            return dto;
        }
        private UserDTO GetNotificationCount(UserDTO dto)
        {
            string strSQL = @"  SELECT COUNT(*)
                                  FROM [SDDB].[dbo].[VSMS_NOTIFICATION]
                                  WHERE USER_ID = @USER_ID AND FLAG = 'T'";

            var parameters = CreateParameter();

            parameters.AddParameter("USER_ID", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Notification.NO = result.OutputDataSet.Tables[0].Rows[0][0].ToString();
            }

            return dto;
        }
        private UserDTO GetNotification(UserDTO dto)
        {
            string strSQL = @"  SELECT TOP 6 *
                                  FROM [SDDB].[dbo].[VSMS_NOTIFICATION]
                                  WHERE CRET_DATE < GETDATE() AND USER_ID = @USER_ID
                                  ORDER BY CRET_DATE DESC";

            var parameters = CreateParameter();

            parameters.AddParameter("USER_ID", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Notifications = result.OutputDataSet.Tables[0].ToList<NotificationModel>();
            }

            return dto;
        }
        private UserDTO FatchNotification(UserDTO dto)
        {
            string strSQL = @"  SELECT TOP 6 *
                                  FROM [SDDB].[dbo].[VSMS_NOTIFICATION]
                                  WHERE CRET_DATE < GETDATE() AND USER_ID = @USER_ID
                                  ORDER BY CRET_DATE DESC";

            var parameters = CreateParameter();

            parameters.AddParameter("USER_ID", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Notifications = result.OutputDataSet.Tables[0].ToList<NotificationModel>();
            }

            return dto;
        }
        private UserDTO GetApp(UserDTO dto)
        {
            string strSQL = @"  select t1.*
		                                ,t2.COM_NAME_E as COM_NAME
		                                ,t2.COM_NAME_E
		                                ,t2.COM_NAME_T
                                from VSMS_USERCOM t1
		                                left join VSMS_COMPANY t2 on t1.COM_CODE = t2.COM_CODE
                                where (1=1) and USER_ID = @USER_ID";

            var parameters = CreateParameter();

            parameters.AddParameter("USER_ID", dto.Model.USER_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                dto.Apps = result.OutputDataSet.Tables[0].ToList<AppModel>();
            }

            return dto;
        }
        private UserDTO GetUser(UserDTO dto)
        {
            dto.Model = (from t1 in _DBManger.VSMS_USER
                         join t2 in _DBManger.VSMS_COMPANY on t1.COM_CODE equals t2.COM_CODE into t2_join
                         from t2 in t2_join.DefaultIfEmpty()
                         join t3 in _DBManger.VSMS_USRGROUP on new { t1.COM_CODE, USG_ID = t1.USG_ID } equals new { t3.COM_CODE, USG_ID = (decimal?)t3.USG_ID }
                         where t1.USER_ID == dto.Model.USER_ID && t3.USG_STATUS == "Y"
                         orderby t1.USER_FNAME_TH
                         select new UserModel
                         {
                             COM_CODE = t1.COM_CODE,
                             USER_ID = t1.USER_ID,
                             USER_FNAME_TH = t1.USER_FNAME_TH,
                             USER_LNAME_TH = t1.USER_LNAME_TH,
                             USER_NAME_TH = ((t1.USER_FNAME_TH + " " ?? "") + t1.USER_LNAME_TH),
                             USER_NAME_EN = ((t1.USER_FNAME_EN + " " ?? "") + t1.USER_LNAME_EN),
                             USG_ID = t1.USG_ID,
                             COM_NAME_T = t2.COM_NAME_T,
                             COM_NAME_E = t2.COM_NAME_E,
                             USG_LEVEL = t3.USG_LEVEL
                         }).FirstOrDefault();

            return dto;
        }
        //private UserDTO GetConfigGeraral(UserDTO dto)
        //{
        //    dto.ConfigGerarals = (from t1 in _DBManger.VSMS_USRGRPPRIV
        //                          join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
        //                          where t1.COM_CODE == dto.Model.COM_CODE && ((decimal?)t1.USG_ID) == dto.Model.USG_ID
        //                          orderby t2.SEQUENCE
        //                          select new ModuleModel
        //                          {
        //                              NAME = t2.NAME,
        //                              SEQUENCE = t2.SEQUENCE,
        //                              IMG = t2.IMG,
        //                              IMG_COLOR = t2.IMG_COLOR
        //                          }).Distinct().ToList();

        //    return dto;
        //}
        private UserDTO GetConfigGeraral(UserDTO dto)
        {
            dto.ConfigGerarals = (from t1 in _DBManger.VSMS_USRGRPPRIV
                                  join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                                  where t1.COM_CODE == dto.Model.COM_CODE && t1.USG_LEVEL == dto.Model.USG_LEVEL
                                  orderby t2.SEQUENCE
                                  select new ModuleModel
                                  {
                                      NAME = t2.NAME,
                                      SEQUENCE = t2.SEQUENCE,
                                      IMG = t2.IMG,
                                      IMG_COLOR = t2.IMG_COLOR
                                  }).Distinct().ToList();

            return dto;
        }
        private UserDTO GetConfigSys(UserDTO dto)
        {
            dto.ConfigGerarals = (from t1 in _DBManger.VSMS_CONFIG_SYS                                  
                                  orderby t1.SEQUENCE
                                  select new ModuleModel
                                  {
                                      NAME = t1.NAME,
                                      SEQUENCE = t1.SEQUENCE,
                                      IMG = t1.IMG,
                                      IMG_COLOR = t1.IMG_COLOR,
                                      SYS_CODE = t1.SYS_CODE
                                  }).Distinct().ToList();

            return dto;
        }

        #endregion Select

        #region Insert
        protected override BaseDTO DoInsert(BaseDTO DTO)
        {
            var status = 0;
            var sErrorMsg = string.Empty;
            UserDTO dto = (UserDTO)DTO;
            //if (dto.Execute.ExecuteType == UserExecuteType.InsertForgotPWD)
            //{
            //    var parameters = CreateParameter();
            //    parameters.AddParameter("RecordCount", status, ParameterDirection.Output);
            //    parameters.AddParameter("sERROR_MSG", sErrorMsg, ParameterDirection.Output);

            //    parameters.AddParameter("pFORGOT_PWD_LOG_ID", dto.ParameterGet("pFORGOT_PWD_LOG_ID"));
            //    parameters.AddParameter("pUSER_ID", dto.ParameterGet("pUSER_ID"));
            //    parameters.AddParameter("pEMAIL", dto.ParameterGet("pEMAIL"));
            //    parameters.AddParameter("pTOKEN", dto.ParameterGet("pTOKEN"));
            //    parameters.AddParameter("pTOKEN_DURATION", dto.ParameterGet("pTOKEN_DURATION"));
            //    parameters.AddParameter("pCREATED_DT", dto.ParameterGet("pCREATED_DT"));

            //    var result = _DBManger.ExecuteNonQuery("PKG_SECMBASE.SP_INSERT_FORGOT_PWD", parameters);
            //    if (!result.Success(dto))
            //    {
            //        return dto;
            //    }
            //    dto.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
            //    dto.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();

            //    if (dto.Result.ActionResult >= 0)
            //    {
            //        parameters = CreateParameter();
            //        parameters.AddParameter("RecordCount", status, ParameterDirection.Output);
            //        parameters.AddParameter("sERROR_MSG", sErrorMsg, ParameterDirection.Output);

            //        parameters.AddParameter("pEMAIL_TO", dto.ParameterGet("pEMAIL_TO"));
            //        parameters.AddParameter("pSUBJECT", dto.ParameterGet("pSUBJECT"));
            //        parameters.AddParameter("pBODY_MAIL", dto.ParameterGet("pBODY_MAIL"));
            //        parameters.AddParameter("pCREATE_USER", dto.ParameterGet("pUSER_ID"));
            //        parameters.AddParameter("pCREATE_DT", DateTime.Now);

            //        result = _DBManger.ExecuteNonQuery("PKG_SECMBASE.SP_INSERT_EMAIL_LIST", parameters);
            //        if (result.Success(dto))
            //        {
            //            dto.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
            //            dto.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
            //        }
            //    }
            //}
            //else
            //{
            //    var parameters = CreateParameter();

            //    parameters.AddParameter("RecordCount", status, ParameterDirection.Output);
            //    parameters.AddParameter("sERROR_MSG", sErrorMsg, ParameterDirection.Output);

            //    parameters.AddParameter("pAPP_CODE", dto.User.COM_CODE != null ? dto.User.COM_CODE : "OIC");
            //    parameters.AddParameter("pMS_CORP_GROUP", dto.User.MS_CORP_GROUP);
            //    parameters.AddParameter("pMS_CORP_ID", dto.User.MS_CORP_ID);
            //    parameters.AddParameter("pUSER_ID", dto.User.USER_ID);
            //    parameters.AddParameter("pCARD_REF_TYPE", dto.User.CARD_REF_TYPE);
            //    parameters.AddParameter("pID_CARD_NO", dto.User.ID_CARD_NO);
            //    parameters.AddParameter("pEMPLOYEE_ID", dto.User.EMPLOYEE_ID);
            //    parameters.AddParameter("pTITLE_ID", dto.User.TITLE_ID);
            //    parameters.AddParameter("pUSER_FNAME_TH", dto.User.USER_FNAME_TH);
            //    parameters.AddParameter("pUSER_LNAME_TH", dto.User.USER_LNAME_TH);
            //    parameters.AddParameter("pUSER_FNAME_EN", dto.User.USER_FNAME_EN);
            //    parameters.AddParameter("pUSER_LNAME_EN", dto.User.USER_LNAME_EN);
            //    parameters.AddParameter("pPOSITION", dto.User.POSITION);
            //    parameters.AddParameter("pSID", dto.User.SID);

            //    parameters.AddParameter("pUSG_GROUP", dto.User.USER_GROUP);
            //    parameters.AddParameter("pUSG_LEVEL", dto.User.USER_LEVEL);
            //    parameters.AddParameter("pUSER_PWD", dto.User.USER_PWD);
            //    parameters.AddParameter("pUSER_EFF_DATE", dto.User.USER_EFF_DATE);
            //    parameters.AddParameter("pUSER_EXP_DATE", dto.User.USER_EXP_DATE);
            //    parameters.AddParameter("pPWD_EXP_DATE", dto.User.PWD_EXP_DATE);
            //    parameters.AddParameter("pWNING_USER_DATE", dto.User.WNING_USER_DATE);
            //    parameters.AddParameter("pWNING_PWD_DATE", dto.User.WNING_PWD_DATE);
            //    parameters.AddParameter("pTELEPHONE", dto.User.TELEPHONE);
            //    parameters.AddParameter("pFAX", dto.User.FAX);
            //    parameters.AddParameter("pEMAIL", dto.User.EMAIL);
            //    parameters.AddParameter("pIS_DISABLED", dto.User.IS_DISABLED);
            //    parameters.AddParameter("pREASON", null);
            //    parameters.AddParameter("pREMARK", dto.User.REMARK);
            //    parameters.AddParameter("pPROXY_FILE", null);
            //    parameters.AddParameter("pDOC_FILE", null);
            //    parameters.AddParameter("pAPPROVE_STATUS", "10040001");
            //    parameters.AddParameter("pAPPROVE_DT", null);
            //    parameters.AddParameter("pAPRROVE_BY", null);
            //    parameters.AddParameter("pREASON_TYPE_ID", dto.User.REASON);
            //    parameters.AddParameter("pREASON_REJECT", null);
            //    parameters.AddParameter("pSTATUS", "N");
            //    parameters.AddParameter("pACTIVE", "N");
            //    parameters.AddParameter("pEFFECTIVE_DT", null);
            //    parameters.AddParameter("pCREATED_USER", dto.User.CREATED_USER);
            //    parameters.AddParameter("pCREATED_DT", dto.User.CREATED_DT);
            //    parameters.AddParameter("pUPDATE_USER", null);
            //    parameters.AddParameter("pUPDATE_DT", null);

            //    //ส่วน คปภ.
            //    parameters.AddParameter("pPREFIX_CODE", dto.User.PREFIX_CODE);
            //    parameters.AddParameter("pPREFIX_NAME", dto.User.PREFIX_NAME);
            //    parameters.AddParameter("pPREFIX_DIGNITY", dto.User.PREFIX_DIGNITY);
            //    parameters.AddParameter("pHI_EMP_NAME", dto.User.HI_EMP_NAME);
            //    parameters.AddParameter("pTTL_CODE", dto.User.TTL_CODE);
            //    parameters.AddParameter("pSECRETARY_GENERAL_CODE", dto.User.SECRETARY_GENERAL_CODE);
            //    parameters.AddParameter("pGENERAL_CODE", dto.User.GENERAL_CODE);
            //    parameters.AddParameter("pGROUP_CODE", dto.User.GROUP_CODE);
            //    parameters.AddParameter("pDEPT_CODE", dto.User.DEPT_CODE);
            //    parameters.AddParameter("pDEPT_ABBVT", dto.User.DEPT_ABBVT);
            //    parameters.AddParameter("pDIVISION_CODE", dto.User.DIVISION_CODE);
            //    parameters.AddParameter("pSECTION_CODE", dto.User.SECTION_CODE);
            //    parameters.AddParameter("pEMP_TYPE_CODE", dto.User.EMP_TYPE_CODE);

            //    parameters.AddParameter("pTTL_TNAME", dto.User.TTL_NAME);
            //    parameters.AddParameter("pSECRETARY_GENERAL_NAME", dto.User.SECRETARY_GENERAL_NAME);
            //    parameters.AddParameter("pGENERAL_NAME", dto.User.GENERAL_NAME);
            //    parameters.AddParameter("pGROUP_NAME", dto.User.GROUP_NAME);
            //    parameters.AddParameter("pDEPT_NAME", dto.User.DEPT_NAME);
            //    parameters.AddParameter("pDIVISION_NAME", dto.User.DIVISION_NAME);
            //    parameters.AddParameter("pSECTION_NAME", dto.User.DE_SECTION_NAME);
            //    parameters.AddParameter("pEMP_TYPE_NAME", dto.User.EMP_TYPE_NAME);
            //    parameters.AddParameter("pSTART_DATE", dto.User.START_DATE);
            //    parameters.AddParameter("pEMP_STATUS", dto.User.EMP_STATUS);
            //    parameters.AddParameter("pGENDER", dto.User.GENDER);
            //    parameters.AddParameter("pFILE_SIZE", null);
            //    parameters.AddParameter("pFILE_DATE", null);
            //    parameters.AddParameter("pAGENCY_NAME", dto.User.AGENCY_NAME);


            //    var result = _DBManger.ExecuteNonQuery("PKG_SEC_SECM00500.SP_INSERT_DATA", parameters);
            //    if (!result.Success(dto))
            //    {
            //        return dto;
            //    }
            //    dto.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
            //    dto.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();

            //    if (dto.Result.ActionResult >= 0)
            //    {
            //        parameters = CreateParameter();

            //        parameters.AddParameter("RecordCount", status, ParameterDirection.Output);
            //        parameters.AddParameter("sERROR_MSG", sErrorMsg, ParameterDirection.Output);
            //        parameters.AddParameter("pUSER_ID", dto.User.USER_ID);

            //        result = _DBManger.ExecuteNonQuery("PKG_SEC_SECM00500.SP_DELETE_USERAPP", parameters);
            //        if (!result.Success(dto))
            //        {
            //            return dto;
            //        }
            //        dto.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
            //        dto.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
            //        if (dto.Result.ActionResult >= 0)
            //        {
            //            foreach (var item in dto.User.UserAppModel)
            //            {
            //                parameters = CreateParameter();

            //                parameters.AddParameter("RecordCount", status, ParameterDirection.Output);
            //                parameters.AddParameter("sERROR_MSG", sErrorMsg, ParameterDirection.Output);
            //                parameters.AddParameter("pAPP_CODE", item.COM_CODE);
            //                parameters.AddParameter("pUSER_ID", dto.User.USER_ID);
            //                parameters.AddParameter("pUSG_ID", item.USG_ID);
            //                parameters.AddParameter("pMS_CORP_GROUP", dto.User.MS_CORP_GROUP);

            //                parameters.AddParameter("pCREATED_USER", dto.User.CREATED_USER);
            //                parameters.AddParameter("pCREATED_DT", dto.User.CREATED_DT);
            //                parameters.AddParameter("pUPDATE_USER", dto.User.UPDATE_USER);
            //                parameters.AddParameter("pUPDATE_DT", dto.User.UPDATE_DT);

            //                result = _DBManger.ExecuteNonQuery("PKG_SEC_SECM00500.SP_INSERT_USERAPP", parameters);
            //                if (!result.Success(dto))
            //                {
            //                    break;
            //                }
            //                dto.Result.ActionResult = result.OutputData["RecordCount"].AsInt();
            //                dto.Result.ResultMsg = result.OutputData["sERROR_MSG"].AsString();
            //                if (dto.Result.ActionResult < 0)
            //                {
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}
            return dto;
        }

        #endregion Insert

        #region Update

        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            UserDTO dto = (UserDTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case UserExecuteType.UpdateFlag: return UpdateFlag(dto);
            }
            return dto;
        }
        private UserDTO UpdateFlag(UserDTO dto)
        {
            string strSQL = @"   UPDATE VSMS_NOTIFICATION SET 
                                  FLAG = 'F'
                                  WHERE FLAG = 'T'";

            var parameters = CreateParameter();

            //parameters.AddParameter("NTF_KEY", dto.Notification.NTF_KEY);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);

            if (result.Success(dto))
            {
                
            }

            return dto;
        }
        #endregion
    }
}
