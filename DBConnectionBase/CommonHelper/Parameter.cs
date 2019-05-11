using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Parameter
    {
        public string ParameterName { get; set; }
        public ParameterDirection Direction { get; set; }
        public int Size { get; set; }
        public object Value { get; set; }
    }
    public class OracleDBParameter : Parameter
    {
        public OracleDBType DBType { get; set; }
    }
    public class SqlDBParameter : Parameter
    {
        public SqlDBType DBType { get; set; }
    }
}
