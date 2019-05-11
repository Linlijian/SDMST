
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess.MIS
{
    [Serializable]
    public class MISS01P002DTO : BaseDTO
    {
        public MISS01P002DTO()
        {
            Model = new MISS01P002Model();   // new โมเดล 
        }

        public MISS01P002Model Model { get; set; }   //model
        public List<MISS01P002Model> Models { get; set; }  //list 
    }

    public class MISS01P002ExecuteType : DTOExecuteType
    {
        
        public const string MoveToFollowUp = "MoveToFollowUp";
        public const string ConfirmTest = "ConfirmTest";
        public const string SearchOnProcess = "SearchOnProcess";
        public const string GetAllOpening = "GetAllOpening";
        public const string GetByIdOpening = "GetByIdOpening";
        public const string GetFiexd = "GetFiexd";
        public const string GetAllFixed = "GetAllFixed";
        public const string UpdateAssignment = "UpdateAssignment";
        public const string GetAllOpening2 = "GetAllOpening2"; 
        public const string GetAllOnProcess = "GetAllOnProcess";
        public const string GetAllFollowUp = "GetAllFollowUp";
        public const string GetFilePacket = "GetFilePacket"; 
        public const string UpdateFilePacket = "UpdateFilePacket";
        public const string MoveToGolive = "MoveToGolive"; 
        public const string TimeStemp = "TimeStemp"; 
        public const string GetAllGolive = "GetAllGolive";
        public const string GetAllClose = "GetAllClose"; 
        public const string MoveToClose = "MoveToClose";
        public const string MoveToCancel = "MoveToCancel";
        public const string ReOpen = "ReOpen";
        public const string ReDo = "ReDo";
    }
}