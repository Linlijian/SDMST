
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.MST
{
    [Serializable]
    public class MSTS02P001DTO : BaseDTO
    {
        public MSTS02P001DTO()
        {
            Model = new MSTS02P001Model();   // new โมเดล 
        }

        public MSTS02P001Model Model { get; set; }   //model
        public List<MSTS02P001Model> Models { get; set; }  //list 
    }

    public class MSTS02P001ExecuteType : DTOExecuteType
    {
        public const string Insert = "Insert";
        public const string Update = "Update";
    }
}