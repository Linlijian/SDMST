
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.MIS
{
    [Serializable]
    public class MISS02P001DTO : BaseDTO
    {
        public MISS02P001DTO()
        {
            Model = new MISS02P001Model();   // new โมเดล 
        }

        public MISS02P001Model Model { get; set; }   //model
        public List<MISS02P001Model> Models { get; set; }  //list 
    }

    public class MISS02P001ExecuteType : DTOExecuteType
    {
        public const string GetDetailByID = "GetDetailByID";
        public const string Insert = "Insert"; 
        public const string CallSPInsertExcel = "CallSPInsertExcel";
        public const string Confirm = "Confirm";
        public const string Update = "Update"; 
        public const string ValidateExl = "ValidateExl";
        public const string cd_dup = "cd_dup";
        public const string GetExl = "GetExl";
        

    }
}