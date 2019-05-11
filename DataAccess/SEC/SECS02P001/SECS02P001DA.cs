using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECS02P001DA : BaseDA
    {
        public SECS02P001DTO DTO
        {
            get;
            set;
        }

        public SECS02P001DA()
        {
            DTO = new SECS02P001DTO();
        }

        #region Select
        protected override BaseDTO DoSelect(BaseDTO DTO)
        {
            var dto = (SECS02P001DTO)DTO;

            switch (dto.Execute.ExecuteType)
            {
                case SECS02P001ExecuteType.GetAll:
                    return GetAll(dto);
                case SECS02P001ExecuteType.GetByID:
                    return GetByID(dto);
                case SECS02P001ExecuteType.GetUsgPriv:
                    return GetUsgPriv(dto);
                case SECS02P001ExecuteType.GetSysSeq:
                    return GetSysSeq(dto);
                case SECS02P001ExecuteType.GetPrgSeq:
                    return GetPrgSeq(dto); 
            }

            return dto;
        }

        private SECS02P001DTO GetAll(SECS02P001DTO dto)
        {
            dto.Models = (from t1 in _DBManger.VSMS_USRGROUP.AsEnumerable()
                          where (dto.Model.USG_NAME_TH.IsNullOrEmpty() || t1.USG_NAME_TH.Contains(dto.Model.USG_NAME_TH)) &&
                          (dto.Model.USG_NAME_EN.IsNullOrEmpty() || t1.USG_NAME_EN.Contains(dto.Model.USG_NAME_EN)) &&
                          (dto.Model.USG_CODE.IsNullOrEmpty() || t1.USG_NAME_TH.Contains(dto.Model.USG_CODE)) &&
                          (dto.Model.USG_STATUS.IsNullOrEmpty() || t1.USG_STATUS == dto.Model.USG_STATUS)
                          select new SECS02P001Model
                          {
                              COM_CODE = t1.COM_CODE,
                              USG_ID = t1.USG_ID,
                              USG_CODE = t1.USG_CODE,
                              USG_NAME_TH = t1.USG_NAME_TH,
                              USG_NAME_EN = t1.USG_NAME_EN,
                              USG_STATUS = t1.USG_STATUS,
                              USG_LEVEL = t1.USG_LEVEL
                          }).ToList();


            return dto;
        }
        private SECS02P001DTO GetByID(SECS02P001DTO dto)
        {
            dto.Model = _DBManger.VSMS_USRGROUP
                .Where(m => m.COM_CODE == dto.Model.COM_CODE && m.USG_ID == dto.Model.USG_ID)
                .FirstOrDefault().ToNewObject(new SECS02P001Model());
            return dto;
        }

        private SECS02P001DTO GetUsgPriv(SECS02P001DTO dto)
        {
            var cmd = @"
                        select t1.SYS_CODE,
                               t1.PRG_CODE,
                               t1.PRG_SEQ,
                               t4.SYS_SEQ,
                               t2.PRG_NAME_EN,
                               t2.PRG_NAME_TH,
                               t2.PRG_TYPE,
                               ISNULL(t3.ROLE_SEARCH,'F') ROLE_SEARCH,
                               ISNULL(t3.ROLE_ADD,'F') ROLE_ADD,
                               ISNULL(t3.ROLE_EDIT,'F') ROLE_EDIT,
                               ISNULL(t3.ROLE_DEL,'F') ROLE_DEL,
                               ISNULL(t3.ROLE_PRINT,'F') ROLE_PRINT,
                               t3.USRGRPPRIV_ID,
                               t3.USG_ID
                          from (select *
                                  from VSMS_SYS_PGC
                                 where COM_CODE = 'VSM' --FIX
                                   and SYS_CODE = @SYS_CODE) t1
                         inner join VSMS_PROGRAM t2
                            on t1.PRG_CODE = t2.PRG_CODE
                          left join (select * from VSMS_USRGRPPRIV where USG_ID = @USG_ID) t3
                            on t1.COM_CODE = t3.COM_CODE
                           and t1.SYS_CODE = t3.SYS_CODE
                           and t1.PRG_CODE = t3.PRG_CODE
						  inner join VSMS_SYSTEM t4
						    on t1.COM_CODE = t4.COM_CODE
						   and t1.SYS_CODE = t4.SYS_CODE
                           order by t1.PRG_SEQ
                        ";
            var parameters = CreateParameter();
           // parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("SYS_CODE", dto.Model.SYS_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SECS02P00101Model>();
            }
            return dto;
        }

        private SECS02P001DTO GetSysSeq(SECS02P001DTO dto)
        {
            var cmd = @"
                    select t1.COM_CODE, t1.SYS_CODE, t1.SYS_SEQ, t2.SYS_NAME_TH, t2.SYS_NAME_EN
                      from (select distinct t1.COM_CODE, t1.SYS_CODE, t1.SYS_SEQ
                              from VSMS_USRGRPPRIV t1
                             inner join VSMS_CONFIG_GENERAL t2
                                on t1.COM_CODE = t2.COM_CODE
                               and t1.SYS_CODE = t2.SYS_CODE
                             where t1.COM_CODE = @COM_CODE
                               and t1.USG_ID = @USG_ID
                               and t2.NAME = @SYS_GROUP_NAME) t1
                     inner join VSMS_SYSTEM t2
                        on t1.COM_CODE = t2.COM_CODE
                       and t1.SYS_CODE = t2.SYS_CODE
                     order by SYS_SEQ
                    ";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters.AddParameter("SYS_GROUP_NAME", dto.Model.SYS_GROUP_NAME);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SECS02P00101Model>();
            }
            return dto;
        }
        private SECS02P001DTO GetPrgSeq(SECS02P001DTO dto)
        {
            var cmd = @"
                    select t1.COM_CODE, t1.PRG_CODE, t1.PRG_SEQ, t2.PRG_NAME_TH, t2.PRG_NAME_EN
                      from (select t1.COM_CODE, t1.PRG_CODE, t1.PRG_SEQ
                              from VSMS_USRGRPPRIV t1
                             inner join VSMS_CONFIG_GENERAL t2
                                on t1.COM_CODE = t2.COM_CODE
                               and t1.SYS_CODE = t2.SYS_CODE
                             where t1.COM_CODE = @COM_CODE
                               and t1.USG_ID = @USG_ID
                               and t1.SYS_CODE = @SYS_CODE
                               and t2.NAME = @SYS_GROUP_NAME) t1
                     inner join VSMS_PROGRAM t2
                        on t1.COM_CODE = t2.COM_CODE
                       and t1.PRG_CODE = t2.PRG_CODE
                     order by PRG_SEQ
                    ";
            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters.AddParameter("SYS_CODE", dto.Model.SYS_CODE);
            parameters.AddParameter("SYS_GROUP_NAME", dto.Model.SYS_GROUP_NAME);

            var result = _DBMangerNoEF.ExecuteDataSet(cmd, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Model.PRIV_MODEL = result.OutputDataSet.Tables[0].ToList<SECS02P00101Model>();
            }
            return dto;
        }
        #endregion

        #region Insert
        protected override BaseDTO DoInsert(BaseDTO DTO)
        {
            var dto = (SECS02P001DTO)DTO;

            switch (dto.Execute.ExecuteType)
            {
                case SECS02P001ExecuteType.InsertData:
                    return InsertData(dto);
            }

            return dto;
        }

        private SECS02P001DTO InsertData(SECS02P001DTO dto)
        {
            //var model = dto.Model.ToNewObject(new VSMS_USRGROUP());
            //_DBManger.VSMS_USRGROUP.Add(model);
            string sql = @"INSERT INTO [dbo].[VSMS_USRGROUP]
                                       ([COM_CODE]
                                        ,[USG_CODE]
                                        ,[USG_NAME_TH]
                                        ,[USG_NAME_EN]
                                        ,[USG_STATUS]
                                        ,[USG_LEVEL]
                                        ,[CRET_BY]
                                        ,[CRET_DATE]
                                        ,[MNT_BY]
                                        ,[MNT_DATE])
                            VALUES
                                        (@COM_CODE 
                                        ,@USG_CODE 
                                        ,@USG_NAME_TH 
                                        ,@USG_NAME_EN 
                                        ,@USG_STATUS
                                        ,@USG_LEVEL 
                                        ,@CRET_BY 
                                        ,@CRET_DATE 
                                        ,@MNT_BY
                                        ,@MNT_DATE)";

            var parameters1 = CreateParameter();

            parameters1.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters1.AddParameter("USG_CODE", dto.Model.USG_CODE);
            parameters1.AddParameter("USG_NAME_TH", dto.Model.USG_NAME_TH);
            parameters1.AddParameter("USG_NAME_EN", dto.Model.USG_NAME_EN);
            parameters1.AddParameter("USG_STATUS", dto.Model.USG_STATUS);
            parameters1.AddParameter("USG_LEVEL", dto.Model.USG_LEVEL);
            parameters1.AddParameter("CRET_BY", dto.Model.CRET_BY);
            parameters1.AddParameter("CRET_DATE", dto.Model.CRET_DATE);
            parameters1.AddParameter("MNT_BY", dto.Model.MNT_BY);
            parameters1.AddParameter("MNT_DATE", dto.Model.MNT_DATE);


            var result = _DBMangerNoEF.ExecuteNonQuery(sql, parameters1, CommandType.Text);
            if (!result.Success(dto))
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }

            return dto;
        }
        #endregion

        #region Update
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SECS02P001DTO)DTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS02P001ExecuteType.UpdateData:
                    return UpdateData(dto);
                case SECS02P001ExecuteType.UpdateUsgPriv:
                    return UpdateUsgPriv(dto);
                case SECS02P001ExecuteType.UpdateSysSeq:
                    return UpdateSysSeq(dto);
                case SECS02P001ExecuteType.UpdatePrgSeq:
                    return UpdatePrgSeq(dto);
            }

            return dto;
        }

        private SECS02P001DTO UpdateData(SECS02P001DTO dto)
        {
            string sql = @"UPDATE [dbo].[VSMS_USRGROUP] SET 
                                         USG_NAME_TH = @USG_NAME_TH
                                        ,USG_NAME_EN = @USG_NAME_EN
                                        ,USG_STATUS = @USG_STATUS
                                        ,USG_LEVEL = @USG_LEVEL
                                        ,MNT_BY = @MNT_BY
                                        ,MNT_DATE = @MNT_DATE
                                WHERE COM_CODE = @COM_CODE
                                AND USG_ID = @USG_ID";

            var parameters1 = CreateParameter();

            parameters1.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters1.AddParameter("USG_ID", dto.Model.USG_ID);
            parameters1.AddParameter("USG_NAME_TH", dto.Model.USG_NAME_TH);
            parameters1.AddParameter("USG_NAME_EN", dto.Model.USG_NAME_EN);
            parameters1.AddParameter("USG_STATUS", dto.Model.USG_STATUS);
            parameters1.AddParameter("USG_LEVEL", dto.Model.USG_LEVEL);
            parameters1.AddParameter("MNT_BY", dto.Model.CRET_BY);
            parameters1.AddParameter("MNT_DATE", dto.Model.CRET_DATE);

            var result = _DBMangerNoEF.ExecuteNonQuery(sql, parameters1, CommandType.Text);
            if (!result.Success(dto))
            {
                dto.Result.IsResult = false;
                dto.Result.ResultMsg = result.ErrorMessage;
            }

            return dto;
        }
        private SECS02P001DTO UpdateUsgPriv(SECS02P001DTO dto)
        {
            var dels = dto.Model.PRIV_MODEL.Where(m => m.ROLE_SEARCH != "T" && !m.USRGRPPRIV_ID.IsNullOrEmpty()).Select(m => m.USRGRPPRIV_ID);
            if (dels != null && dels.Count() > 0)
            {
                var del = _DBManger.VSMS_USRGRPPRIV.Where(m =>
                                            m.COM_CODE == dto.Model.COM_CODE &&
                                            m.USG_ID == dto.Model.USG_ID &&
                                            m.SYS_CODE == dto.Model.SYS_CODE &&
                                            dels.Contains(m.USRGRPPRIV_ID));
                _DBManger.VSMS_USRGRPPRIV.RemoveRange(del);
            }
            var update = dto.Model.PRIV_MODEL.Where(m => m.ROLE_SEARCH == "T");
            if (update != null && update.Count() > 0)
            {
                foreach (var item in update)
                {
                    var model = _DBManger.VSMS_USRGRPPRIV.Where(m => m.COM_CODE == dto.Model.COM_CODE && m.USG_ID == dto.Model.USG_ID && m.SYS_CODE == dto.Model.SYS_CODE && m.USRGRPPRIV_ID == item.USRGRPPRIV_ID).FirstOrDefault();
                    if (model != null)
                    {
                        model = model.MergeObject(item);
                        model = model.MergeObject(dto.Model);
                    }
                    else
                    {
                        var newModel = item.ToNewObject(new VSMS_USRGRPPRIV());
                        newModel.PRG_STATUS = "E";
                        newModel.SYS_STATUS = "E";
                        newModel = newModel.MergeObject(dto.Model);
                        _DBManger.VSMS_USRGRPPRIV.Add(newModel);
                    }
                }
            }
            return dto;
        }
        private SECS02P001DTO UpdateSysSeq(SECS02P001DTO dto)
        {
            var update = from t1 in _DBManger.VSMS_USRGRPPRIV
                         join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                         where t1.COM_CODE == dto.Model.COM_CODE && t1.USG_ID == dto.Model.USG_ID && t2.NAME == dto.Model.SYS_GROUP_NAME
                         select t1;
            if (update != null && update.Any())
            {
                var i = 0;
                foreach (var item in dto.Model.PRIV_MODEL)
                {
                    var models = update.Where(m => m.SYS_CODE == item.SYS_CODE);
                    if (models != null && models.Any())
                    {
                        models.ToList().ForEach(m => m.SYS_SEQ = i);
                    }
                    i++;
                }
            }
            return dto;
        }

        private SECS02P001DTO UpdatePrgSeq(SECS02P001DTO dto)
        {
            var update = from t1 in _DBManger.VSMS_USRGRPPRIV
                         join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                         where t1.COM_CODE == dto.Model.COM_CODE && t1.USG_ID == dto.Model.USG_ID && t1.SYS_CODE == dto.Model.SYS_CODE && t2.NAME == dto.Model.SYS_GROUP_NAME
                         select t1;
            if (update != null && update.Any())
            {
                var i = 0;
                foreach (var item in dto.Model.PRIV_MODEL)
                {
                    var models = update.Where(m => m.PRG_CODE == item.PRG_CODE);
                    if (models != null && models.Any())
                    {
                        models.ToList().ForEach(m => m.PRG_SEQ = i);
                    }
                    i++;
                }
            }
            return dto;
        }
        #endregion

        #region Delete
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SECS02P001DTO)DTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS02P001ExecuteType.DeleteData:
                    return DeleteData(dto);
            }
            return dto;
        }
        private SECS02P001DTO DeleteData(SECS02P001DTO dto)
        {
            foreach (var item in dto.Models)
            {
                var model = _DBManger.VSMS_USRGROUP.Where(m => m.COM_CODE == item.COM_CODE && m.USG_ID == item.USG_ID);
                _DBManger.VSMS_USRGROUP.RemoveRange(model);

                var model2 = _DBManger.VSMS_USRGRPPRIV.Where(m => m.COM_CODE == item.COM_CODE && m.USG_ID == item.USG_ID);
                _DBManger.VSMS_USRGRPPRIV.RemoveRange(model2);
            }
            return dto;
        }
        #endregion
    }
}
