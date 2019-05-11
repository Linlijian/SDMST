using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS01P003DTO : BaseDTO
    {
        public SECS01P003DTO()
        {
            Model = new SECS01P003Model();
        }

        public SECS01P003Model Model { get; set; }
        public List<SECS01P003Model> Models { get; set; }
    }

    public class SECS01P003ExecuteType : DTOExecuteType
    {
        public const string CHECK_DB_SERVER_NAME = "CHECK_DB_SERVER_NAME";
    }
}
