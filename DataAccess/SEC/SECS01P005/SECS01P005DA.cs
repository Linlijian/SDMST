using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECS01P005DA : BaseDA
    {
        public SECS01P005DTO DTO { get; set; }

        public SECS01P005DA()
        {
            DTO = new SECS01P005DTO();
        }

        #region Select
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SECS01P005DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS01P005ExecuteType.GetAll: return GetAll(dto);
                case SECS01P005ExecuteType.GetProgram: return GetProgram(dto);
                case SECS01P005ExecuteType.GetSysPrg: return GetSysPrg(dto);
            }
            return dto;
        }
        private SECS01P005DTO GetAll(SECS01P005DTO dto)
        {
            dto.Models = (from t1 in _DBManger.VSMS_COMPANY.AsEnumerable()
                          join t2 in _DBManger.VSMS_SYSTEM on t1.COM_CODE equals t2.COM_CODE
                          where (string.IsNullOrEmpty(dto.Model.COM_CODE) || t1.COM_CODE == dto.Model.COM_CODE) &&
                          (string.IsNullOrEmpty(dto.Model.SYS_CODE) || t2.SYS_CODE == dto.Model.SYS_CODE)
                          orderby t1.COM_CODE, t2.SYS_CODE
                          select new SECS01P005Model
                          {
                              COM_CODE = t1.COM_CODE,
                              COM_NAME = t1.COM_NAME_T.GetSingleValue(t1.COM_NAME_E),
                              COM_NAME_TH = t1.COM_NAME_T,
                              COM_NAME_EN = t1.COM_NAME_E,
                              SYS_CODE = t2.SYS_CODE,
                              SYS_NAME_TH = t2.SYS_NAME_TH,
                              SYS_NAME_EN = t2.SYS_NAME_EN
                          }).ToList();
            return dto;
        }

        private SECS01P005DTO GetProgram(SECS01P005DTO dto)
        {
            string strSQL = @"
                    SELECT A.PRG_CODE, A.PRG_NAME_EN, A.PRG_NAME_TH
                      FROM dbo.VSMS_PROGRAM A
                      LEFT JOIN dbo.VSMS_SYS_PGC B
                        ON A.COM_CODE = B.COM_CODE
                       AND A.PRG_CODE = B.PRG_CODE
                       AND B.SYS_CODE = @SYS_CODE
                     WHERE A.PRG_STATUS = 'E'
                       AND A.COM_CODE = @COM_CODE
                       AND B.PRG_CODE IS NULL
                    order by PRG_CODE, A.PRG_NAME_EN, A.PRG_NAME_TH
                    ";

            var parameters = CreateParameter();
            parameters.AddParameter("COM_CODE", dto.Model.COM_CODE);
            parameters.AddParameter("SYS_CODE", dto.Model.SYS_CODE);

            var result = _DBMangerNoEF.ExecuteDataSet(strSQL, parameters, CommandType.Text);
            if (result.Success(dto))
            {
                dto.Models =result.OutputDataSet.Tables[0].ToList<SECS01P005Model>();
            }

            return dto;
        }

        private SECS01P005DTO GetSysPrg(SECS01P005DTO dto)
        {
            dto.Models = (from t1 in _DBManger.VSMS_SYS_PGC.AsEnumerable()
                          join t2 in _DBManger.VSMS_PROGRAM on new { t1.COM_CODE, t1.PRG_CODE } equals new { t2.COM_CODE, t2.PRG_CODE }
                          where (t1.COM_CODE == dto.Model.COM_CODE) && (t1.SYS_CODE == dto.Model.SYS_CODE)
                          orderby t1.PRG_SEQ
                          select new SECS01P005Model
                          {
                              COM_CODE = t1.COM_CODE,
                              PRG_CODE = t1.PRG_CODE,
                              PRG_NAME = t2.PRG_NAME_TH.GetSingleValue(t2.PRG_NAME_EN),
                              PRG_NAME_TH = t2.PRG_NAME_TH,
                              PRG_NAME_EN = t2.PRG_NAME_EN
                          }).ToList();

            return dto;
        }
        #endregion

        #region Update
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            //Delete
            var dto = (SECS01P005DTO)baseDTO;
            var deletes = _DBManger.VSMS_SYS_PGC.Where(m => m.COM_CODE == dto.Model.COM_CODE && m.SYS_CODE == dto.Model.SYS_CODE);
            _DBManger.VSMS_SYS_PGC.RemoveRange(deletes);

            var i = 0;
            foreach (var item in dto.Models)
            {
                var model = item.ToNewObject<SECS01P005Model, VSMS_SYS_PGC>();
                model.PRG_SEQ = i;
                model.COM_CODE = dto.Model.COM_CODE;
                model.SYS_CODE = dto.Model.SYS_CODE;
                _DBManger.VSMS_SYS_PGC.Add(model);
                i++;
            }
            return dto;
        }
        #endregion
    }
}
