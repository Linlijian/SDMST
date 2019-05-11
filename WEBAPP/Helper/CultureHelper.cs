using System.Globalization;
using System.Threading;
using System.Web.SessionState;

namespace WEBAPP.Helper
{
    public class CultureHelper
    {
        protected HttpSessionState session;

        //constructor   
        public CultureHelper(HttpSessionState httpSessionState)
        {
            session = httpSessionState;
        }
        // Properties  
        public static string CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentUICulture.Name; }
            set
            {
                var cInfo = new CultureInfo(value)
                {
                    DateTimeFormat =
                    {
                        ShortDatePattern = "dd/MM/yyyy",
                        DateSeparator = "/"
                    }
                };

                Thread.CurrentThread.CurrentUICulture = cInfo;

                //ไม่เซ็ตให้เครื่อง
                //Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;

            }
        }
    }
}
