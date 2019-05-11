
using System;
using System.Collections.Generic;

namespace DataAccess.Ux
{
    [Serializable]
    public class AutocompleteDTO : BaseDTO
    {
        public AutocompleteDTO()
        {
            Parameter = new AutocompleteParameterModel();
        }
        
        public int totalcount { get; set; }

        private int _pageIndex = 0;
        public int pageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        private int _pageSize = 10;
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public List<AutocompleteColumnModel> colModel { get; set; }
        public List<AutocompleteColumnModel> colKeyModel { get; set; }
        public List<Dictionary<string, object>> rows { get; set; }
        public AutocompleteParameterModel Parameter { get; set; }
    }

    public class AutocompleteExecuteType : DTOExecuteType
    {
        public const string GetStructure = "GetStructure";
        public const string GetDataOnly = "GetDataOnly";
        public const string GetValidate = "GetValidate";
        public const string InsertNotInTable = "InsertNotInTable";
        public const string DeleteNotInTable = "DeleteNotInTable";
    }
}