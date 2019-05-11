using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class ButtonConfig
    {
        public ButtonConfig(string name)
        {
            Name = name;
        }
        private string _Url = "javascript:void(0)";
        //private string _Url = "href=\"#\" data-dismiss=\"modal\"";
        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }
        public string CssClass { get; set; }
        public string IconCssClass { get; set; }
        private TextColor _IconColor = TextColor.None;
        public TextColor IconColor
        {
            get
            {
                return _IconColor;
            }
            set
            {
                _IconColor = value;
            }
        }
        private StandardIconPosition _IconPosition = StandardIconPosition.BeforeText;
        public StandardIconPosition IconPosition
        {
            get
            {
                return _IconPosition;
            }
            set
            {
                _IconPosition = value;
            }
        }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        private int _Index = -1;
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value;
            }
        }
        private bool _IsNewWindow = false;
        public bool IsNewWindow
        {
            get
            {
                return _IsNewWindow;
            }
            set
            {
                _IsNewWindow = value;
            }
        }

        private bool _Tooltip = false;

        public bool Tooltip
        {
            get { return _Tooltip; }
            set { _Tooltip = value; }
        }


        private List<VSMParameter> _Parameters = new List<VSMParameter>();
        public List<VSMParameter> Parameters
        {
            get
            {
                return _Parameters;
            }
            private set
            {
                _Parameters = value;
            }
        }
        private Dictionary<string, object> _HtmlAttribute = new Dictionary<string, object>();
        public Dictionary<string, object> HtmlAttribute
        {
            get
            {
                return _HtmlAttribute;
            }
            private set
            {
                _HtmlAttribute = value;
            }
        }

    }
}