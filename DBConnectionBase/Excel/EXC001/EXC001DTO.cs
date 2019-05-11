
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.EXC001
{
    [Serializable]
    public class EXC001DTO : BaseDTO
    {
        public EXC001DTO()
        {
            Model = new UtilityLib.EXC001Model();
        }

        public UtilityLib.EXC001Model Model { get; set; }
        public List<UtilityLib.EXC001Model> Models { get; set; }
    }

    public class EXC001ExecuteType : DTOExecuteType
    {
        public const string GetQueryAllVal = "GetQueryAllVal";
    }
}