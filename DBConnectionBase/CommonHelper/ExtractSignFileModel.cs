using System.Web;

namespace UtilityLib
{
    public class ExtractSignFileModel
    {
        public HttpPostedFileBase DOCUMENT_UPLOAD { get; set; }
        public byte[] BLOB_FILE { get; set; }
        public string BLOB_FILE_HASH { get; set; }
        public string FILE_SIZE { get; set; }
        public string FILE_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public string CERTIFICATE_NO { get; set; }
        public string SIGNATURE_SIGN { get; set; }
    }


    public class FILE_ATTACH_CONFIG
    {
        public string FILE_TYPE { get; set; }
        public decimal FILE_SIZE { get; set; }
    }



}
