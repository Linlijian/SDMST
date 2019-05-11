
using System;

namespace DataAccess.Ux
{
    [Serializable]
    public class FileDTO : BaseDTO
    {
        public FileDTO()
        {
            ESignModel = new ElectronicSignModel();
        }

        public ElectronicSignModel ESignModel { get; set; }
        //public List<FileUpload> Models { get; set; }
    }

    public class FileExecuteType : DTOExecuteType
    {
        public const string GetBlobList = "GetBlobList";
        public const string GetBlobFileList = "GetBlobFileList";
        public const string GetBlobFileByID = "GetBlobFileByID";
        public const string GetElectronicSign= "GetElectronicSign";
    }
}