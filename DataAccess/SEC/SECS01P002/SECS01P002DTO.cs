
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS01P002DTO : BaseDTO
    {
        public SECS01P002DTO()
        {
            Model = new SECS01P002Model();
        }

        public SECS01P002Model Model { get; set; }
        public List<SECS01P002Model> Models { get; set; }
        public List<SECS01P002_SystemModel> SystemModels { get; set; }
    }

    public class SECS01P002ExecuteType : DTOExecuteType
    {
        public const string GetSystemDetail = "GetSystemDetail";
    }
}