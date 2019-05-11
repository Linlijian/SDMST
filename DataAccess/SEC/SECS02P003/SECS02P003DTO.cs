
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS02P003DTO : BaseDTO
    {
        public SECS02P003DTO()
        {
            Model = new SECS02P003Model();
        }

        public SECS02P003Model Model { get; set; }
        public VSMS_TITLE ModelEF { get; set; }
        public List<SECS02P003Model> Models { get; set; }
    }

    public class SECS02P003ExecuteType : DTOExecuteType
    {
    }
}