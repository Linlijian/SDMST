using System;
using WEBAPP.Helper;

namespace WEBAPP.Reports.MasterPage
{
    public partial class ReportMasterLayout : System.Web.UI.MasterPage
    {
        private void SetCulture()
        {
            string UIculture = "th-TH";
            if (this.Session == null || this.Session[SessionSystemName.SYS_CurrentCulture] == null)
            {

                var httpSessionStateBase = this.Session;
                if (httpSessionStateBase != null) httpSessionStateBase[SessionSystemName.SYS_CurrentCulture] = UIculture;
            }
            else
            {
                UIculture = (string)this.Session[SessionSystemName.SYS_CurrentCulture];
            }

            CultureHelper.CurrentCulture = UIculture;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetCulture();

            if (SessionHelper.SYS_CurrentCulture == "th-TH")
            {
                Lang.InnerText = Translation.CenterLang.Center.English;
                userTH.HRef = "/Users/Account/LangUi?lang=en-US";
            }
            else
            {
                Lang.InnerText = Translation.CenterLang.Center.Thai;
                userTH.HRef = "/Users/Account/LangUi?lang=th-TH";
            }
        }
    }
}