using System.Collections.Generic;
using System.ComponentModel;

namespace DataAccess
{
    public enum Connection
    {
        [Description("Connection")]
        Connection,
        [Description("Connection2")]
        Connection2,
        [Description("Connection3")]
        Connection3
    }

    #region Class : DTOResult
    public class DTOResult
    {
        #region Member
        private int actionResult = 0;
        private bool isResult = true;
        //private string strResultMsg_En = string.Empty;
        private string strResultCode = string.Empty;
        private string strResultMsg = string.Empty;

        private string strActionMethod = string.Empty;
        #endregion

        #region Property
        public bool IsResult
        {
            get { return isResult; }
            set { isResult = value; }
        }

        public int ActionResult
        {
            get { return actionResult; }
            set
            {
                if (value < 0)
                {
                    isResult = false;
                }
                actionResult = value;
            }
        }

        public string ResultCode
        {
            get { return strResultCode; }
            set { strResultCode = value; }
        }

        public string ResultMsg
        {
            get { return strResultMsg; }
            set { strResultMsg = value; }
        }

        #endregion
    }
    #endregion

    #region Class : DTOQuery
    public class DTOExecute
    {
        #region Member
        private Dictionary<string, object> htParameter = new Dictionary<string, object>();
        private string executeType;
        #endregion

        #region Property
        public Dictionary<string, object> ParameterList
        {

            get { return htParameter; }
            set { htParameter = value; }
        }
        public string ExecuteType
        {
            get { return executeType; }
            set { executeType = value; }
        }
        #endregion
    }
    #endregion

    #region Class : DTOQueryType
    public class DTOExecuteType
    {
        public const string GetAll = "GetAll";
        public const string GetByID = "GetByID";
    }
    #endregion

    #region Class : BaseDTO
    public class BaseDTO
    {
        #region Member
        private DTOExecute dtoExecute = new DTOExecute();
        private DTOResult dtoResult = new DTOResult();
        private DTOResult jobResult = new DTOResult();
        private Connection _ConnectTo = Connection.Connection;
        private bool _IsTransaction = true;
        #endregion

        #region Constructor

        public BaseDTO()
        {

        }


        #endregion

        #region Property           
        /// <summary>
        /// เป็น Property ที่ใช้เก็บข้อมูลการ Error หรือจำนวน Action Result
        /// </summary>
        public DTOResult Result
        {
            get { return dtoResult; }
            set { dtoResult = value; }
        }

        public DTOExecute Execute
        {
            get { return dtoExecute; }
            set { dtoExecute = value; }
        }

        public Connection ConnectTo
        {
            get { return _ConnectTo; }
            set { _ConnectTo = value; }
        }


        private TransactionLogModel transactionLog = new TransactionLogModel();

        public TransactionLogModel TransactionLog
        {
            get { return transactionLog; }
            set { transactionLog = value; }
        }

        private FileModel _ExecuteFile = new FileModel();

        public FileModel ExecuteFile
        {
            get { return _ExecuteFile; }
            set { _ExecuteFile = value; }
        }

        public bool IsTransaction
        {
            get
            {
                return _IsTransaction;
            }
            set
            {
                _IsTransaction = value;
            }
        }

        public string JobName { get; set; }
        public DTOResult JobResult
        {
            get { return jobResult; }
            set { jobResult = value; }
        }
        #endregion

        #region Operator : Parameter
        /// <summary>
        /// เพิ่มข้อมูล Object เข้า ItemList
        /// </summary>
        /// <param name="Name">ชื่อข้อมูล</param>
        /// <param name="Item">ข้อมูล</param>
        public void ParameterAdd(string Name, object Item)
        {
            if (dtoExecute.ParameterList.ContainsKey(Name))
            {
                dtoExecute.ParameterList.Remove(Name); // ถ้ามี Name นี่อยู่ก่อนแล้วจะลบก่อนแล้วค่อยเพิ่ม
            }
            dtoExecute.ParameterList.Add(Name, Item);
        }

        /// <summary>
        /// ตรวจสอบข้อมูลตาม ชื่อข้อมูล ที่ต้องการ
        /// </summary>
        /// <param name="Name">ชื่อข้อมูล</param>
        /// <returns>Object</returns>
        public bool ParameterContains(string Name)
        {
            return dtoExecute.ParameterList.ContainsKey(Name);
        }

        /// <summary>
        /// Get ข้อมูลตาม ชื่อข้อมูล ที่ต้องการ
        /// </summary>
        /// <param name="Name">ชื่อข้อมูล</param>
        /// <returns>Object</returns>
        public object ParameterGet(string Name)
        {
            object objItem = null;
            if (dtoExecute.ParameterList.ContainsKey(Name))
            {
                objItem = dtoExecute.ParameterList[Name];
            }
            return objItem;
        }

        /// <summary>
        /// Edit ข้อมูลตาม ชื่อข้อมูล ที่ต้องการ
        /// </summary>
        /// <param name="Name">ชื่อข้อมูล</param>
        /// <returns>bool</returns>
        public bool ParameterEdit(string Name, object newItem)
        {
            bool isEdit = false;
            if (dtoExecute.ParameterList.ContainsKey(Name))
            {
                dtoExecute.ParameterList[Name] = newItem;
            }
            return isEdit;
        }

        /// <summary>
        /// ลบข้อมูลจาก Item List จากชื่อที่ต้องการ
        /// </summary>
        /// <param name="Name">ชื่อข้อมูล</param>
        public void ParameterRemove(string Name)
        {
            dtoExecute.ParameterList.Remove(Name);
        }

        /// <summary>
        /// ลบข้อมูลจาก Item List ทั้งหมด
        /// </summary>
        public void ParameterRemoveAll()
        {
            dtoExecute.ParameterList.Clear();
        }
        #endregion Operator : Parameter

        #region Copy

        #endregion
    }
    #endregion

}
