using System.Collections.Generic;

namespace DataAccess
{
    public class TreeModel : StandardModel
    {
        public decimal? id { get; set; }
        public decimal? parent_id { get; set; }
        public string text { get; set; }
        public string tbl_name { get; set; }
        //private string _icon = "glyphicon glyphicon-stop";
        //public string icon
        //{
        //    get
        //    {
        //        return _icon;
        //    }
        //    set
        //    {
        //        _icon = value;
        //    }
        //}
        //private string _selectedIcon = "glyphicon glyphicon-stop";
        //public string selectedIcon
        //{
        //    get
        //    {
        //        return _selectedIcon;
        //    }
        //    set
        //    {
        //        _selectedIcon = value;
        //    }
        //}
        private string _color = "#000000";
        public string color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        private string _backColor = "#FFFFFF";
        public string backColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }
        private string _href = "#";
        public string href
        {
            get
            {
                return _href;
            }
            set
            {
                _href = value;
            }
        }
        private bool _selectable = true;
        public bool selectable
        {
            get
            {
                return _selectable;
            }
            set
            {
                _selectable = value;
            }
        }
        private bool _selected = false;
        public bool selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }
        private TreeStateModel _state = new TreeStateModel();
        public TreeStateModel state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }
        //private List<string> _tags = new List<string> { "available" };
        //public List<string> tags
        //{
        //    get
        //    {
        //        return _tags;
        //    }
        //    set
        //    {
        //        _tags = value;
        //    }
        //}
        public List<TreeModel> nodes { get; set; }
        public decimal? level { get; set; }
    }

    public class TreeStateModel
    {
        private bool _checked = false;
        public bool @checked
        {
            get { return _checked; }
            set { _checked = value; }
        }
        private bool _disabled = false;
        public bool disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }
        private bool _expanded = false;
        public bool expanded
        {
            get { return _expanded; }
            set { _expanded = value; }
        }
        private bool _selected = false;
        public bool selected
        {
            get { return _selected; }
            set { _selected = value; }
        }
    }
}
