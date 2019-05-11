using System;
using UtilityLib;

namespace WEBAPP.Helper
{
    public class GridColumn
    {
        public GridColumn()
        {

        }
        public GridColumn(string data)
        {
            this.data = data;
        }
        public GridColumn(string data, ColumnsType type = ColumnsType.None)
        {
            this.data = data;
            this.type = type;
            var align = ColumnsTextAlign.None;
            string vRender = null;
            if ((type == ColumnsType.Date || type == ColumnsType.DateTime) && string.IsNullOrEmpty(vRender))
            {
                vRender = "function (data, type, full, meta) {return $.jsonDateToFormat(data";
                if (type == ColumnsType.DateTime)
                {
                    render += ",'DD/MM/YYYY HH:mm:ss'";
                }
                vRender += ");}";
                align = ColumnsTextAlign.Center;
            }

            if ((type == ColumnsType.Number ||
                type == ColumnsType.NumberFormat ||
                type == ColumnsType.NumberFormat2 ||
                type == ColumnsType.NumberFormat3 ||
                type == ColumnsType.NumberFormat4 ||
                type == ColumnsType.NumberFormat6 ||
                type == ColumnsType.NumberFormat8 ||
                type == ColumnsType.NumberFormat10
                ) && string.IsNullOrEmpty(vRender))
            {
                align = ColumnsTextAlign.Right;
                if (type != ColumnsType.Number)
                {
                    var digitLength = 0;
                    if (type == ColumnsType.NumberFormat2)
                    {
                        digitLength = 2;
                    }
                    else if (type == ColumnsType.NumberFormat3)
                    {
                        digitLength = 3;
                    }
                    else if (type == ColumnsType.NumberFormat4)
                    {
                        digitLength = 4;
                    }
                    else if (type == ColumnsType.NumberFormat6)
                    {
                        digitLength = 6;
                    }
                    else if (type == ColumnsType.NumberFormat8)
                    {
                        digitLength = 8;
                    }
                    else if (type == ColumnsType.NumberFormat10)
                    {
                        digitLength = 10;
                    }
                    if (digitLength > 0)
                    {
                        vRender = "function (data, type, full, meta) {return digitsFormat(toNumberFormat(data)," + digitLength + ");}";
                    }
                    else
                    {
                        vRender = "function (data, type, full, meta) {return toNumberFormat(data);}";
                    }
                }
            }

            if (type == ColumnsType.File ||
                type == ColumnsType.FileMultiple)
            {
                vRender = "function (data, type, full, meta) {return '<table id=\"Grid|tplName|";
                vRender += data;
                vRender += "\" class=\"table table-striped table-bordered table-hover gridInGrid\"></table>';}";
            }
            this.render = vRender;
            this.className = align != ColumnsTextAlign.None ? align.GetDescription() : null;
        }
        public GridColumn(string data, string title, int? width)
        {
            this.data = data;
            this.title = title;
            this.width = width != null ? Convert.ToString(width) : null;
        }
        public GridColumn(string data, string title, int? width, ColumnsTextAlign align = ColumnsTextAlign.None)
        {
            this.data = data;
            this.title = title;
            this.width = width != null ? Convert.ToString(width) : null;
            this.className = align != ColumnsTextAlign.None ? align.GetDescription() : null;
        }
        public GridColumn(string data, string title, int? width, ColumnsType type = ColumnsType.None, ColumnsTextAlign align = ColumnsTextAlign.None, string render = null)
        {
            this.data = data;
            this.title = title;
            this.width = width != null ? Convert.ToString(width) : null;
            this.type = type;
            if ((type == ColumnsType.Date ||
                type == ColumnsType.DateTime) && string.IsNullOrEmpty(render))
            {
                render = "function (data, type, full, meta) {return $.jsonDateToFormat(data";
                if (type == ColumnsType.DateTime)
                {
                    render += ",'DD/MM/YYYY HH:mm:ss'";
                }
                render += ");}";
                if (align == ColumnsTextAlign.None)
                {
                    align = ColumnsTextAlign.Center;
                }
            }

            if (type == ColumnsType.Time && string.IsNullOrEmpty(render))
            {
                render = "function (data, type, full, meta) {return $.jsonDateToFormat(data";
                if (type == ColumnsType.Time)
                {
                    render += ",'HH:mm:ss'";
                }
                render += ");}";
                if (align == ColumnsTextAlign.None)
                {
                    align = ColumnsTextAlign.Center;
                }
            }

            if ((type == ColumnsType.NumberFormat ||
                type == ColumnsType.NumberFormat2 ||
                type == ColumnsType.NumberFormat3 ||
                type == ColumnsType.NumberFormat4 ||
                type == ColumnsType.NumberFormat6 ||
                type == ColumnsType.NumberFormat8 ||
                type == ColumnsType.NumberFormat10
                ) && string.IsNullOrEmpty(render))
            {
                if (align == ColumnsTextAlign.None)
                {
                    align = ColumnsTextAlign.Right;
                }
                var digitLength = 0;
                if (type == ColumnsType.NumberFormat2)
                {
                    digitLength = 2;
                }
                else if (type == ColumnsType.NumberFormat3)
                {
                    digitLength = 3;
                }
                else if (type == ColumnsType.NumberFormat4)
                {
                    digitLength = 4;
                }
                else if (type == ColumnsType.NumberFormat6)
                {
                    digitLength = 6;
                }
                else if (type == ColumnsType.NumberFormat8)
                {
                    digitLength = 8;
                }
                else if (type == ColumnsType.NumberFormat10)
                {
                    digitLength = 10;
                }

                if (digitLength > 0)
                {
                    render = "function (data, type, full, meta) {return digitsFormat(toNumberFormat(data)," + digitLength + ");}";
                }
                else
                {
                    render = "function (data, type, full, meta) {return toNumberFormat(data);}";
                }
            }

            if (type == ColumnsType.Checkbox)
            {
                if (align == ColumnsTextAlign.None)
                {
                    align = ColumnsTextAlign.Center;
                }
                render = "function (data, type, full, meta) {";
                render += "var tag = '<i class=\"ace-icon fa fa-square-o\"></i>';";
                render += "if(data == 'Y'){";
                render += "tag = '<i class=\"ace-icon fa fa-check-square-o\"></i>';}";
                render += "return tag;}";
            }
            this.render = render;
            this.className = align != ColumnsTextAlign.None ? align.GetDescription() : null;
        }
        public string cellType { get; set; }
        private string _className = "dt-head-center";
        public string className
        {
            get
            {
                if (_IsHeadNoWrap)
                {
                    _className += " dt-head-nowrap";
                }
                return _className;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    _className = "dt-head-center " + value;
                }
                else
                {
                    _className = "dt-head-center";
                }
            }
        }
        public string contentPadding { get; set; }
        public string createdCell { get; set; }
        public string data { get; set; }
        public string defaultContent { get; set; }
        public string name { get; set; }
        private bool _orderable = true;
        public bool orderable
        {
            get
            {
                return _orderable;
            }
            set
            {
                _orderable = value;
            }
        }
        public string orderData { get; set; }
        public string orderDataType { get; set; }
        public string orderSequence { get; set; }
        public string render { get; set; }
        public string searchable { get; set; }
        public string title { get; set; }
        public ColumnsType type { get; set; }
        public string visible { get; set; }
        public bool visiblerender { get; set; }
        public string width { get; set; }
        private bool _IsKey = false;
        public bool IsKey
        {
            get
            {
                return _IsKey;
            }
            set
            {
                _IsKey = value;
                if (value)
                {
                    _IsRequired = value;
                }
            }
        }
        private bool _CaseSensitive = true;
        public bool CaseSensitive
        {
            get
            {
                return _CaseSensitive;
            }
            set
            {
                _CaseSensitive = value;
            }
        }
        private bool _IsButtonColumn = false;
        public bool IsButtonColumn
        {
            get
            {
                return _IsButtonColumn;
            }
            set
            {
                _IsButtonColumn = value;
            }
        }
        private bool _IsOrderColumn = false;
        public bool IsOrderColumn
        {
            get
            {
                return _IsOrderColumn;
            }
            set
            {
                _IsOrderColumn = value;
            }
        }

        private bool _IsEditable = false;
        public bool IsEditable
        {
            get
            {
                return _IsEditable;
            }
            set
            {
                _IsEditable = value;
            }
        }
        public object SelectOptions { get; set; }
        public string FileType { get; set; }

        private bool _IsRequired = false;
        public bool IsRequired
        {
            get
            {
                return _IsRequired;
            }
            set
            {
                _IsRequired = value;
            }
        }
        private bool _CustomTrigger = false;
        public bool CustomTrigger
        {
            get
            {
                return _CustomTrigger;
            }
            set
            {
                _CustomTrigger = value;
            }
        }

        private bool _EditableReadOnly = false;
        public bool EditableReadOnly
        {
            get
            {
                return _EditableReadOnly;
            }
            set
            {
                _EditableReadOnly = value;
            }
        }
        public GridColumnAutocompleteConfig AutocompleteConfig { get; set; }
        public int? MaxLength { get; set; }

        private bool _IsIdCard = false;
        public bool IsIdCard
        {
            get
            {
                return _IsIdCard;
            }
            set
            {
                _IsIdCard = value;
            }
        }
        public string CustomOptionEditable { get; set; }

        private bool _IsHeadNoWrap = true;
        public bool IsHeadNoWrap
        {
            get
            {
                return _IsHeadNoWrap;
            }
            set
            {
                _IsHeadNoWrap = value;
            }
        }
    }
}
