namespace WEBAPP.Helper
{
    public class GridEditableConfig : VSMDetailConfig
    {
        public string CustomInitAfterClick { get; set; }
        public string OnAfterBinding { get; set; }
        public string OnDrawCallback { get; set; }

        private string _PRG_CODE = SessionHelper.SYS_CurrentPRG_CODE;
        public string PRG_CODE
        {
            get { return _PRG_CODE; }
            set { _PRG_CODE = value; }
        }

    }
}
