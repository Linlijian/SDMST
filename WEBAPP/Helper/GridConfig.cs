namespace WEBAPP.Helper
{
    public class GridConfig
    {
        public DefaultConfig GetConfig { get; set; }
        public DefaultConfig DeleteConfig { get; set; }
        public DefaultConfig CustomsExportConfig { get; set; }

        private bool _VisibleCheckBox = false;
        public bool VisibleCheckBox
        {
            get { return _VisibleCheckBox; }
            set { _VisibleCheckBox = value; }
        }

        private bool _VisibleExportButton = true;
        public bool VisibleExportButton
        {
            get { return _VisibleExportButton; }
            set { _VisibleExportButton = value; }
        }

        private string _DataSrc = "";
        public string DataSrc
        {
            get { return _DataSrc; }
            set { _DataSrc = value; }
        }

        private bool _ScrollX = true;
        public bool ScrollX
        {
            get { return _ScrollX; }
            set { _ScrollX = value; }
        }
        private bool _DefaultBinding = false;
        public bool DefaultBinding
        {
            get { return _DefaultBinding; }
            set { _DefaultBinding = value; }
        }
        public string OnAfterBinding { get; set; }
        public string OnDrawCallback { get; set; }
        public string OnRowCallback { get; set; }
        public string OnInit { get; set; }
        public string OnSelect { get; set; }
        public string OnDeSelect { get; set; }
        public string OnUserSelect { get; set; }
        public string OnPreDrawCallback { get; set; }
        public string OnSelectAll { get; set; }
        public string OninitComplete { get; set; }

        private ColumnsWidthType _WidthType = ColumnsWidthType.Pixel;
        public ColumnsWidthType WidthType
        {
            get { return _WidthType; }
            set { _WidthType = value; }
        }

        private bool _DisableDefaultSorting = true;
        public bool DisableDefaultSorting
        {
            get { return _DisableDefaultSorting; }
            set { _DisableDefaultSorting = value; }
        }

        private bool _Paging = true;
        public bool Paging
        {
            get { return _Paging; }
            set { _Paging = value; }
        }
        private bool _Searching = false;
        public bool Searching
        {
            get { return _Searching; }
            set { _Searching = value; }
        }

        private bool _IsAjax = true;
        public bool IsAjax
        {
            get { return _IsAjax; }
            set { _IsAjax = value; }
        }
        public object Data { get; set; }

        private bool _IsCustomsTitle = false;
        public bool IsCustomsTitle
        {
            get { return _IsCustomsTitle; }
            set { _IsCustomsTitle = value; }
        }

        public int? ScrollY { get; set; }
        private bool _ScrollCollapse = false;
        public bool ScrollCollapse
        {
            get
            {
                return _ScrollCollapse;
            }
            set
            {
                _ScrollCollapse = value;
            }
        }

        private bool _TableResponsive = true;
        public bool TableResponsive
        {
            get
            {
                return _TableResponsive;
            }
            set
            {
                _TableResponsive = value;
            }
        }

        private bool _ServerSide = false;
        public bool ServerSide
        {
            get
            {
                return _ServerSide;
            }
            set
            {
                _ServerSide = value;
            }
        }

        private bool _IsSingleRowSelect = false;
        public bool IsSingleRowSelect
        {
            get { return _IsSingleRowSelect; }
            set { _IsSingleRowSelect = value; }
        }

        private int? _PageLength = null;
        public int? PageLength
        {
            get { return _PageLength; }
            set { _PageLength = value; }
        }
    }
}
