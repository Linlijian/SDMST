using System;

namespace DataAccess
{
    [Serializable]
    public class DDLCenterParamModel
    {
        public string FIXHEADER_ID { get; set; }
        public string FIXDETAIL_VALUE { get; set; }

        public string error_code { get; set; }
        public string KEY_ID { get; set; }
        public string ParameterValues { get; set; }
        public string WhereClause { get; set; }
        public string OrderBy { get; set; }
    }
}
