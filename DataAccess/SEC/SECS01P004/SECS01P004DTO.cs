
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS01P004DTO : BaseDTO
    {
        public SECS01P004DTO()
        {
            Model = new SECS01P004Model();
        }

        public SECS01P004Model Model { get; set; }
        public List<SECS01P004Model> Models { get; set; }
    }

    public class SECS01P004ExecuteType : DTOExecuteType
    {
        public const string GetSeqMax = "GetSeqMax"; 
    }
}