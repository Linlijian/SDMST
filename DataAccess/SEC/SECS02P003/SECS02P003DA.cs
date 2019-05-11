using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECS02P003DA : BaseDA
    {
        public SECS02P003DTO DTO { get; set; }

        #region ====Property========
        public SECS02P003DA()
        {
            DTO = new SECS02P003DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SECS02P003DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECS02P003ExecuteType.GetAll: return GetAll(dto);
                case SECS02P003ExecuteType.GetByID: return GetByID(dto);
            }
            return dto;
        }
        private SECS02P003DTO GetAll(SECS02P003DTO dto)
        {
            dto.Models = _DBManger.VSMS_TITLE
                .Where(m => ((dto.Model.TITLE_NAME == null || dto.Model.TITLE_NAME == string.Empty) || m.TITLE_NAME_TH.Contains(dto.Model.TITLE_NAME) || m.TITLE_NAME_EN.Contains(dto.Model.TITLE_NAME)))
                .OrderBy(m => new { m.TITLE_NAME_TH, m.TITLE_NAME_EN })
                .Select(m => new SECS02P003Model
                {
                    TITLE_ID = m.TITLE_ID,
                    TITLE_NAME_TH = m.TITLE_NAME_TH,
                    TITLE_NAME_EN = m.TITLE_NAME_EN
                }).ToList();
            return dto;
        }
        private SECS02P003DTO GetByID(SECS02P003DTO dto)
        {
            dto.Model = _DBManger.VSMS_TITLE
                .Where(m => m.TITLE_ID == dto.Model.TITLE_ID)
                .FirstOrDefault().ToNewObject(new SECS02P003Model());
            return dto;
        }
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SECS02P003DTO)baseDTO;
            dto.ModelEF = dto.Model.ToNewObject(new VSMS_TITLE());
            _DBManger.VSMS_TITLE.Add(dto.ModelEF);
            return dto;
        }
        protected override BaseDTO DoAfterInsert(BaseDTO baseDTO)
        {
            var dto = (SECS02P003DTO)baseDTO;
            dto.Model.TITLE_ID = dto.ModelEF.TITLE_ID;
            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SECS02P003DTO)baseDTO;
            var TITLE_ID = dto.Model.TITLE_ID.AsDecimal();
            var model = _DBManger.VSMS_TITLE.First(m => m.TITLE_ID == TITLE_ID);
            model.MergeObject(dto.Model);
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SECS02P003DTO)baseDTO;
            foreach (var item in dto.Models)
            {
                var TITLE_ID = item.TITLE_ID.AsDecimal();
                var items = _DBManger.VSMS_TITLE.Where(m => m.TITLE_ID == TITLE_ID);
                _DBManger.VSMS_TITLE.RemoveRange(items);
            }
            return dto;
        }
        #endregion
    }
}