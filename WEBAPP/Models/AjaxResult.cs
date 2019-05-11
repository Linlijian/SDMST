using System.Collections.Generic;

namespace WEBAPP.Models
{
    public class AjaxResult : BaseAjaxResult
    {
        public AjaxResult()
        {

        }
        public AjaxResult(string mode, bool status = false, string style = null, string message = null, List<string> redirectToUrl = null, string titleMsg = null, bool isReturnValue = false, object returnValues = null, string cancelToUrl = null, List<string> ignorebtn = null)
        {
            Mode = mode;
            Status = status;
            RedirectToUrl = redirectToUrl;
            Style = style;
            Message = message;
            TitleMassage = titleMsg;
            IsReturnValue = isReturnValue;
            ReturnValues = returnValues;
            CancelToUrl = cancelToUrl;
            IgnoreBtn = ignorebtn;
        }
        public AjaxResult(string mode, bool status = false, string style = null, string message = null, params string[] redirectToUrl)
        {
            Mode = mode;
            Status = status;
            Style = style;
            Message = message;
            if (redirectToUrl!=null)
            {
                RedirectToUrl = new List<string>();
                RedirectToUrl.AddRange(redirectToUrl);
            }
        }
        public List<string> RedirectToUrl { get; set; }
        public string CancelToUrl { get; set; }
        public bool IsReturnValue { get; set; }
        public object ReturnValues { get; set; }

        private bool _ExistsChioce = false;
        public bool ExistsChioce
        {
            get { return _ExistsChioce; }
            set { _ExistsChioce = value; }
        }

        public List<string> IgnoreBtn { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; }

    }
}