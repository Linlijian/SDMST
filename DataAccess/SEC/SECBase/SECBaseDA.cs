using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SECBaseDA : BaseDA
    {
        public SECBaseDTO DTO
        {
            get;
            set;
        }
        public SECBaseDA()
        {
            DTO = new SECBaseDTO();
        }
        protected override BaseDTO DoSelect(BaseDTO BaseDTO)
        {
            var dto = (SECBaseDTO)BaseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SECBaseExecuteType.GetMenu: return GetMenu(dto);
            }
            return dto;
        }

        private SECBaseDTO GetMenu(SECBaseDTO dto)
        {
            dto.Menus = (from t1 in _DBManger.VSMS_USRGRPPRIV
                         join t2 in _DBManger.VSMS_CONFIG_GENERAL on new { t1.COM_CODE, t1.SYS_CODE } equals new { t2.COM_CODE, t2.SYS_CODE }
                         join t3 in _DBManger.VSMS_SYSTEM on new { t1.COM_CODE, t1.SYS_CODE } equals new { t3.COM_CODE, t3.SYS_CODE }
                         join t4 in _DBManger.VSMS_PROGRAM on new { t1.COM_CODE, t1.PRG_CODE } equals new { t4.COM_CODE, t4.PRG_CODE }
                         where t1.ROLE_SEARCH == "T" && t1.USG_LEVEL == dto.Menu.USG_LEVEL && t1.COM_CODE == dto.Menu.COM_CODE && t2.NAME == dto.Menu.SYS_GROUP_NAME
                         orderby t1.SYS_SEQ, t1.PRG_SEQ
                         select new MenuModel
                         {
                             COM_CODE = t1.COM_CODE,
                             PRG_CODE = t1.PRG_CODE,
                             SYS_CODE = t1.SYS_CODE,
                             SYS_SEQ = t1.SYS_SEQ,
                             PRG_SEQ = t1.PRG_SEQ,
                             ROLE_SEARCH = t1.ROLE_SEARCH,
                             ROLE_ADD = t1.ROLE_ADD,
                             ROLE_EDIT = t1.ROLE_EDIT,
                             ROLE_DEL = t1.ROLE_DEL,
                             ROLE_PRINT = t1.ROLE_PRINT,
                             IsROLE_SEARCH = t1.ROLE_SEARCH == "T",
                             IsROLE_ADD = t1.ROLE_ADD == "T",
                             IsROLE_EDIT = t1.ROLE_EDIT == "T",
                             IsROLE_DEL = t1.ROLE_DEL == "T",
                             IsROLE_PRINT = t1.ROLE_PRINT == "T",
                             SYS_NAME_TH = t3.SYS_NAME_TH,
                             SYS_NAME_EN = t3.SYS_NAME_EN,
                             PRG_NAME_TH = t4.PRG_NAME_TH,
                             PRG_NAME_EN = t4.PRG_NAME_EN,
                             PRG_AREA = t4.PRG_AREA.Trim(),
                             PRG_CONTROLLER = t4.PRG_CONTROLLER.Trim(),
                             PRG_ACTION = t4.PRG_ACTION.Trim(),
                             PRG_IMG = t4.PRG_IMG,
                             PRG_PARAMETER = t4.PRG_PARAMETER
                         }).ToList();

            return dto;
        }
    }

}