using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECS01P004DA : BaseDA
    {
        public SECS01P004DTO DTO { get; set; }

        #region ====Property========
        public SECS01P004DA()
        {
            DTO = new SECS01P004DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SECS01P004DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS01P004ExecuteType.GetAll: return GetAll(dto);
                case SECS01P004ExecuteType.GetByID: return GetByID(dto);
                case SECS01P004ExecuteType.GetSeqMax: return GetSeqMax(dto);
            }
            return dto;
        }
        private SECS01P004DTO GetAll(SECS01P004DTO dto)
        {
            dto.Models = _DBManger.VSMS_SYSTEM
                .Where(m => 
                    ((dto.Model.SYS_CODE == null || dto.Model.SYS_CODE == string.Empty) || m.SYS_CODE.Contains(dto.Model.SYS_CODE))
                    && ((dto.Model.SYS_SEQ == null) || m.SYS_SEQ == dto.Model.SYS_SEQ)
                    && ((dto.Model.SYS_NAME_TH == null || dto.Model.SYS_NAME_TH == string.Empty) || m.SYS_NAME_TH.Contains(dto.Model.SYS_NAME_TH))
                    && ((dto.Model.SYS_NAME_EN == null || dto.Model.SYS_NAME_EN == string.Empty) || m.SYS_NAME_EN.Contains(dto.Model.SYS_NAME_EN))
                    && ((dto.Model.SYS_STATUS == null || dto.Model.SYS_STATUS == string.Empty) || m.SYS_STATUS.Contains(dto.Model.SYS_STATUS))
                    )
                .OrderBy(m => new { m.SYS_SEQ })
                .Select(m => new SECS01P004Model
                {
                    SYS_CODE = m.SYS_CODE,
                    SYS_SEQ = m.SYS_SEQ,
                    SYS_NAME_TH = m.SYS_NAME_TH,
                    SYS_NAME_EN = m.SYS_NAME_EN,
                    SYS_STATUS = m.SYS_STATUS
                }).ToList();
            return dto;
        }

        private SECS01P004DTO GetByID(SECS01P004DTO dto)
        {
            dto.Model = _DBManger.VSMS_SYSTEM
                .Where(m => (m.COM_CODE == dto.Model.COM_CODE) 
                    && (m.SYS_CODE == dto.Model.SYS_CODE)
                )
                .FirstOrDefault().ToNewObject(new SECS01P004Model());

            int? sys_no = _DBManger.VSMS_SYSTEM.Max(m => m.SYS_SEQ).AsIntNull();
            dto.Model.SYS_NO = sys_no;

            return dto;
        }

        private SECS01P004DTO GetSeqMax(SECS01P004DTO dto)
        {
            int? sys_no = _DBManger.VSMS_SYSTEM.Max(m => m.SYS_SEQ).AsIntNull();
            dto.Model.SYS_NO = sys_no;
            return dto;
        }

        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SECS01P004DTO)baseDTO;
            if (dto.Model.SYS_STATUS != null)
            {
                dto.Model.SYS_STATUS = dto.Model.SYS_STATUS.Trim();
            }

            var model = dto.Model.ToNewObject(new VSMS_SYSTEM());
            _DBManger.VSMS_SYSTEM.Add(model);

            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SECS01P004DTO)baseDTO;
            if (dto.Model.SYS_STATUS != null)
            {
                dto.Model.SYS_STATUS = dto.Model.SYS_STATUS.Trim();
            }

            var SYS_CODE = dto.Model.SYS_CODE;
            var model = _DBManger.VSMS_SYSTEM.First(m => m.SYS_CODE == SYS_CODE);
            model.MergeObject(dto.Model);

            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SECS01P004DTO)baseDTO;
            foreach (var item in dto.Models)
            {
                var items = _DBManger.VSMS_SYSTEM.Where(m => m.COM_CODE == dto.Model.COM_CODE && m.SYS_CODE == item.SYS_CODE);
                _DBManger.VSMS_SYSTEM.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}