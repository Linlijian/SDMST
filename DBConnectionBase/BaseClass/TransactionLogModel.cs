using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class TransactionLogModel : StandardModel
    {
        public string LOG_HEADER { get; set; }
        public string LOG_TYPE { get; set; }
        public decimal? ACTIVITY_TYPE { get; set; }
        public string IP_ADDRESS { get; set; }
        public string SYS_CODE { get; set; }
        public string PRG_CODE { get; set; }
        public List<SaveLogConfig> SaveLogConfig { get; set; }
        public object ObjectValue { get; set; }
        public bool DoInsertLog { get; set; }
    }

    public class SaveLogConfig
    {
        public SaveLogConfig()
        {

        }

        public string Schema { get; set; }
        public string TableName { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
        public DataSet DataAfterSave { get; set; }
        public DataSet DataBeforeSave { get; set; }
        public List<LogColumnConfig> Columns { get; set; }
        public SaveLogType LogType { get; set; }
        public List<string> PKColumns { get; set; }
        public bool DoInsertLog { get; set; }
    }
    public enum SaveLogType
    {
        Insert,
        Update,
        Delete
    }
    public class LogColumnConfig
    {
        public LogColumnConfig()
        {

        }
        public LogColumnConfig(string pkColumnName, bool isChar = false, int charLength = -1)
        {
            PKColumnName = pkColumnName;
            IsChar = isChar;
            CharLength = charLength;
        }
        public string PKColumnName { get; set; }
        public bool IsChar { get; set; }
        public int CharLength { get; set; }
    }
    public class DataBeforeSave
    {
        public string DETAIL { get; set; }
        public string RECORD_ID { get; set; }
    }
}
