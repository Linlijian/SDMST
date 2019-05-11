using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class VSMDetailConfig
    {
        //public List<GridColumn> Columns { get; set; }
        public List<VSMParameter> Parameters { get; set; }
        public DefaultConfig DefaultConfig { get; set; }

        private bool _VisibleCheckBox = true;
        public bool VisibleCheckBox
        {
            get { return _VisibleCheckBox; }
            set { _VisibleCheckBox = value; }
        }

        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set { _IsReadOnly = value; }
        }

        private bool _IsRequired = false;
        public bool IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }

        private bool _VisibleEditColumn = true;
        public bool VisibleEditColumn
        {
            get { return _VisibleEditColumn; }
            set { _VisibleEditColumn = value; }
        }

        private bool _CustomSave = false;
        public bool CustomSave
        {
            get { return _CustomSave; }
            set { _CustomSave = value; }
        }

        private bool _CustomEdit = false;
        public bool CustomEdit
        {
            get { return _CustomEdit; }
            set { _CustomEdit = value; }
        }

        private bool _VisibleAdd = true;
        public bool VisibleAdd
        {
            get { return _VisibleAdd; }
            set { _VisibleAdd = value; }
        }

        private bool _VisibleDelete = true;
        public bool VisibleDelete
        {
            get { return _VisibleDelete; }
            set { _VisibleDelete = value; }
        }

        private ColumnsWidthType _WidthType = ColumnsWidthType.Pixel;
        public ColumnsWidthType WidthType
        {
            get { return _WidthType; }
            set { _WidthType = value; }
        }

        private bool _ScrollX = true;
        public bool ScrollX
        {
            get { return _ScrollX; }
            set { _ScrollX = value; }
        }

        private bool _Searching = false;
        public bool Searching
        {
            get { return _Searching; }
            set { _Searching = value; }
        }

        private bool _VisibleExportButton = false;
        public bool VisibleExportButton
        {
            get { return _VisibleExportButton; }
            set { _VisibleExportButton = value; }
        }

        public string OnBeforeAdd { get; set; }
        public string OnBeforeSave { get; set; }
        public string OnAfterSave { get; set; }
        public string OnSave { get; set; }
        public string OnBeforeEdit { get; set; }
        public string OnDelete { get; set; }
        public string OnDrawCallback { get; set; }
        public string OnAfterModalShow { get; set; }
        public string OnRowCallback { get; set; }

        public string ConditionShowEdit { get; set; }

        private bool _IsSingleRowSelect = false;
        public bool IsSingleRowSelect
        {
            get { return _IsSingleRowSelect; }
            set { _IsSingleRowSelect = value; }
        }
    }
}
