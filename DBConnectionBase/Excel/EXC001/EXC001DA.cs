using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.EXC001
{
    public class EXC001DA : BaseDA
    {
        public EXC001DTO DTO { get; set; }

        #region ====Property========
        public EXC001DA()
        {
            DTO = new EXC001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (EXC001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case EXC001ExecuteType.GetQueryAllVal:
                    return GetQueryAllVal(dto);
            }
            return dto;
        }

        private EXC001DTO GetQueryAllVal(EXC001DTO dto)
        {
            dto.Models = (
                        from a in _DBManger.VSMS_EXCEL_DETAIL
                        join b in _DBManger.VSMS_EXCEL on new { a.COM_CODE,a.PRG_CODE } equals new { b.COM_CODE,b.PRG_CODE }
                        where ((dto.Model.COM_CODE == null || string.IsNullOrEmpty(dto.Model.COM_CODE)) || a.COM_CODE == dto.Model.COM_CODE)
                            && ((dto.Model.PRG_CODE == null || string.IsNullOrEmpty(dto.Model.PRG_CODE)) || a.PRG_CODE == dto.Model.PRG_CODE)
                        orderby a.LIST_NO
                        select new EXC001Model
                        {
                            COM_CODE = a.COM_CODE,
                            PRG_CODE = a.PRG_CODE,
                            COL_NAME = a.COL_NAME,
                            LIST_NO = a.LIST_NO,
                            COL_TYPE = a.COL_TYPE,
                            RATIO = a.RATIO,
                            FLG_USE = a.FLG_USE,
                            ISNULL = a.ISNULL,
                            FORMAT_STYLE = a.FORMAT_STYLE,
                            CULTULE_STYLE = a.CULTULE_STYLE,
                            CRET_BY = a.CRET_BY,
                            CRET_DATE = a.CRET_DATE,
                            IS_VALIDATE = a.IS_VALIDATE,
                            IS_SAVE = a.IS_SAVE,
                            DB_COLUMN = a.DB_COLUMN,
                            MAX_LENGTH = a.MAX_LENGTH
                        }).ToList();

            return dto;
        }
        #endregion
    }
}