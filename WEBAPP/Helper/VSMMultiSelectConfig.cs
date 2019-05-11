using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class VSMMultiSelectConfig
    {
        public string KeySource { get; set; }
        public List<GridColumn> ColumnsShow { get; set; }
        public List<GridColumn> Columns { get; set; }
        public List<GridColumn> ColumnKey { get; set; }
        public List<VSMParameter> Parameters { get; set; }
        public DefaultConfig DefaultShowConfig { get; set; }
        public string CustomsDataOnSave { get; set; }

        private bool _Searching = false;
        public bool Searching
        {
            get { return _Searching; }
            set { _Searching = value; }
        }
    }
}
