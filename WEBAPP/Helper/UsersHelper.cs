using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class UsersHelper
    {
        
        public void GetUserAuth()
        {
           
        }

        public IEnumerable<UserPermission> GetPermission()
        {
            
            return  new List<UserPermission>();
        }
    }

    public class UserPermission
    {
        public string PrgCode { get; set; }

        public string SysCode { get; set; }

        public bool IsSearch { get; set; }

        public bool IsAdd { get; set; }

        public bool IsEdit { get; set; }

        public bool IsDel { get; set; }

        public bool IsPrint { get; set; }
    }


}