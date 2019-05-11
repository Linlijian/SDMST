using System;
using System.Collections.Generic;

namespace DataAccess.SEC
{
    [Serializable]
    public class SECS02P001DTO : BaseDTO
    {
        public SECS02P001DTO()
        {
            Model = new SECS02P001Model();
            Models = new List<SECS02P001Model>();
        }
        public SECS02P001Model Model { get; set; }
        public List<SECS02P001Model> Models { get; set; }
    }

    public class SECS02P001ExecuteType : DTOExecuteType
    {
        public const string GetUsgPriv = "GetUsgPriv";
        public const string GetSysSeq = "GetSysSeq";
        public const string GetPrgSeq = "GetPrgSeq"; 
        public const string InsertData = "InsertData";
        public const string UpdateData = "UpdateData";
        public const string UpdateUsgPriv = "UpdateUsgPriv";
        public const string UpdateSysSeq = "UpdateSysSeq";
        public const string UpdatePrgSeq = "UpdatePrgSeq";
        public const string DeleteData = "DeleteData";
    }
}
