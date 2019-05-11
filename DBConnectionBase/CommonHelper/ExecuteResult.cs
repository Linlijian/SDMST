using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ExecuteResult
    {
        public object data { get; set; }
        public bool Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> OutputData { get; set; }
        public DataSet OutputDataSet { get; set; }
    }
}
