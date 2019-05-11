
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.MST
{
    [Serializable]
    public class MSTS03P002DTO : BaseDTO
    {
        public MSTS03P002DTO()
        {
            Model = new MSTS03P002Model();   // new โมเดล 
        }

        public MSTS03P002Model Model { get; set; }   //model
        public List<MSTS03P002Model> Models { get; set; }  //list 
    }

    public class MSTS03P002ExecuteType : DTOExecuteType
    {
        public const string Insert = "Insert";
        public const string Update = "Update";
    }
}