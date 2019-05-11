
using System;
using System.Collections.Generic;

namespace DataAccess
{
    [Serializable]
    public class DDLCenterDTO : BaseDTO
    {
        public DDLCenterDTO()
        {
            Parameter = new DDLCenterParamModel();
        }

        public List<DDLCenterModel> DDLCenters { get; set; }
        public DDLCenterParamModel Parameter
        {
            get;
            set;
        }

    }
}