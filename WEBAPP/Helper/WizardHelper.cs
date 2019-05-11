using System.Collections.Generic;

namespace WEBAPP.Helper
{
    public class WizardHelper
    {
        public class WizardHeaderMode
        {
            public const string disabled = "disabled";
            public const string complete = "complete";
            public const string completeactive = "complete active";
            public const string active = "active";
            public const string activeend = "active-end";
            public const string completeend = "complete-end";
            public const string complete_dis = "complete-dis";
            public const string none = "";
            public const string current = "current";
        }

        public class WizardHeaderConfig
        {
            public WizardHeaderConfig()
            {
            }
            public WizardHeaderConfig(string currentWizard, string lastWizard, params WizardHeader[] wizardHeader)
            {
                if (!string.IsNullOrEmpty(currentWizard))
                {
                    _CurrentWizard = currentWizard.Split('.');
                }
                if (!string.IsNullOrEmpty(lastWizard))
                {
                    _LastWizard = lastWizard.Split('.');
                }

                WizardHeader = new List<WizardHeader>();
                foreach (var item in wizardHeader)
                {
                    WizardHeader.Add(item);
                }
            }
            public WizardHeaderConfig(string currentWizard, string lastWizard, string wizardStatus, params WizardHeader[] wizardHeader)
            {
                if (!string.IsNullOrEmpty(currentWizard))
                {
                    _CurrentWizard = currentWizard.Split('.');
                }
                if (!string.IsNullOrEmpty(lastWizard))
                {
                    _LastWizard = lastWizard.Split('.');
                }

                if (!string.IsNullOrEmpty(wizardStatus))
                {
                    _WizardStatus = wizardStatus;
                }
                WizardHeader = new List<WizardHeader>();
                foreach (var item in wizardHeader)
                {
                    WizardHeader.Add(item);
                }
            }

            public const string TempDataKey = "TempDataHeaderWizard";
            private string[] _CurrentWizard = { "0" };
            public string[] CurrentWizard
            {
                get
                {
                    return _CurrentWizard;
                }
            }
            private string[] _LastWizard = { "0" };
            public string[] LastWizard
            {
                get
                {
                    return _LastWizard;
                }
            }
            private string _WizardStatus = "P";
            public string WizardStatus
            {
                get
                {
                    return _WizardStatus;
                }
            }
            public List<WizardHeader> WizardHeader { get; set; }
        }

        public class WizardHeader
        {
            public WizardHeader(
                string text,
                string url,
                string iconCssClass = FaIcons.FaPencil,
                string iconColor = "",
                string textStep = "",
                bool skip = false,
                params SubWizardHeader[] subWizardHeader)
            {
                Text = text;
                Url = url;
                IconCssClass = iconCssClass;
                IconColor = iconColor;
                Skip = skip;
                TextStep = textStep;
                SubWizardHeader = new List<SubWizardHeader>();
                foreach (var item in subWizardHeader)
                {
                    SubWizardHeader.Add(item);
                }
            }

            public string IconCssClass { get; set; }
            public string IconColor { get; set; }
            public string Text { get; set; }
            public string TextStep { get; set; }
            public string Url { get; set; }
            public bool Skip { get; set; }
            public List<SubWizardHeader> SubWizardHeader { get; set; }
            public StandardWizardButton StandardWizardButton { get; set; }
        }

        public class SubWizardHeader
        {
            public SubWizardHeader(string text, string url, string wizardHeaderMode = WizardHelper.WizardHeaderMode.none, bool skip = false, string fixHeaderClass = "")
            {
                Text = text;
                Url = url;
                Skip = skip;
                WizardHeaderMode = wizardHeaderMode;
                FixHeaderClass = fixHeaderClass;
            }

            public string Text { get; set; }
            public string Url { get; set; }
            public bool Skip { get; set; }
            public string FixHeaderClass { get; set; }
            public string WizardHeaderMode { get; set; }
        }
    }
}