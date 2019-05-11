using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBAPP.Helper
{
    public class ResultOptions
    {
        public string Mode { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string GridName { get; set; }
        private bool _ExistsChioce = false;
        public bool ExistsChioce
        {
            get
            {
                return _ExistsChioce;
            }
            set
            {
                _ExistsChioce = value;
            }
        }

    }
}