using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS01P005DTO : BaseDTO
    {
        public SECS01P005DTO()
        {
            Model = new SECS01P005Model();
        }

        public SECS01P005Model Model { get; set; }
        public List<SECS01P005Model> Models { get; set; }
    }

    public class SECS01P005ExecuteType : DTOExecuteType
    {
        public const string GetProgram = "GetProgram";
        public const string GetSysPrg = "GetSysPrg";
    }
}
