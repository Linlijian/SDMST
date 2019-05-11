using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class DefaultConfig
    {
        public DefaultConfig()
        {

        }
        
        public DefaultConfig(string url, params VSMParameter[] param)
        {
            Url = url;
            if (param != null)
            {
                Parameters = new List<VSMParameter>();
                foreach (var item in param)
                {
                    Parameters.Add(item);
                }
            }
        }
        public string Url { get; set; }
        public List<VSMParameter> Parameters { get; set; }
    }
}
