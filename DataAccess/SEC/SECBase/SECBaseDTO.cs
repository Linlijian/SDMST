using System.Collections.Generic;

namespace DataAccess.SEC
{
    public class SECBaseDTO : BaseDTO
    {
        public SECBaseDTO()
        {
            Menu = new MenuModel();
            //Certificate = new SEC_SECM00401Model();
        }
        public MenuModel Menu { get; set; }
        public List<MenuModel> Menus { get; set; }
        //public SEC_SECM00401Model Certificate { get; set; }
        public int TotalRows { get; set; }
    }

    public class SECBaseExecuteType : DTOExecuteType
    {
        public const string GetMenu = "GetMenu";
        public const string GetBlobCert = "GetBlobCert";
    }
}
