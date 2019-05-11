using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib
{
    public class ZipDataModel
    {
        public byte[] BLOB_FILE { get; set; }
        public string BLOB_FILE_HASH { get; set; }
        public string FILE_NAME { get; set; }
    }
}
