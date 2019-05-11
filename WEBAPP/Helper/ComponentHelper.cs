using System.Collections.Generic;
using System.ComponentModel;

namespace WEBAPP.Helper
{
    public class StandardButton
    {

        public const string TempDataKey = "TempDataStandardButton";
        public StandardIconPosition IconPosition { get; set; }
        private StandButtonType _Type = StandButtonType.Button;
        public StandButtonType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        public string Name { get; set; }
        public string CssClass { get; set; }
        public string IconCssClass { get; set; }
        public string IconColor { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string FormName { get; set; }
        public string CallbackName { get; set; }
        public bool IsValidate { get; set; }
        public bool RequiredCer { get; set; }
        public bool OverrideSubmit { get; set; }
        public string ToolTipText { get; set; }
        public IDictionary<string, object> HtmlAttribute { get; set; }
    }

    public class StandardButtonName
    {
        public const string Search = "Search";
        public const string Add = "Add";
        public const string Edit = "Edit";
        public const string DeleteSearch = "DeleteSearch";
        public const string Delete = "Delete";
        public const string Report = "Report";
        public const string Print = "Print";
        public const string Clear = "Clear";
        public const string Save = "Save";
        public const string SaveCreate = "SaveCreate";
        public const string SaveModify = "SaveModify";
        public const string Process = "Process";
        public const string Import = "Import";
        public const string Export = "Export";
        public const string Upload = "Upload";
        public const string Download = "Download";
        public const string ExportExcel = "ExportExcel";
        public const string ExportPDF = "ExportPDF";
        public const string ConfSys = "Confige System";
        public const string Info = "Info";
        public const string ElectronicSign = "ElectronicSign";
        public const string LoadFile = "LoadFile";
        public const string DownloadTemplate = "DownloadTemplate";
    }
    public class StandardButtonCss
    {
        public const string Search = "std-btn-search";
        public const string Add = "btn-add";
        public const string Edit = "btn-edit";
        public const string DeleteSearch = "std-btn-delete";
        public const string Delete = "std-btn-delete";
        public const string Clear = "std-btn-clear";
        public const string Save = "std-btn-confirm";
        public const string SaveCreate = "std-btn-confirm";
        public const string SaveModify = "std-btn-confirm";
        public const string Info = "btn-info";
        public const string SendData = "btn-senddata";
    }
    public class SetStandardButton
    {
        public SetStandardButton(string name = "", bool isVisible = true)
        {
            Name = name;
            IsVisible = isVisible;
        }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }

    public class StandardWizardButton
    {
        public StandardWizardButton(StandardStepWizardMode mode, StandardStepWizard step, bool isAjax = true, bool existDatatables = false, bool requiredCer = false, StandardWizardToorbarPosition toorbarPosition = StandardWizardToorbarPosition.Bottom)
        {
            ToolBarPosition = toorbarPosition;
            IsAjax = isAjax;
            ExistDatatables = existDatatables;
            RequiredCer = requiredCer;
            SetStandardWizard(mode, step);
        }
        public StandardWizardButton(StandardStepWizardMode mode, bool isAjax = true, bool existDatatables = false, bool requiredCer = false, StandardWizardToorbarPosition toorbarPosition = StandardWizardToorbarPosition.Bottom)
        {
            ToolBarPosition = toorbarPosition;
            IsAjax = isAjax;
            ExistDatatables = existDatatables;
            RequiredCer = requiredCer;
            SetStandardWizard(mode);
        }

        public StandardWizardButton(StandardStepWizardMode mode, StandardWizardToorbarPosition toorbarPosition = StandardWizardToorbarPosition.Bottom)
        {
            ToolBarPosition = toorbarPosition;
            SetStandardWizard(mode);
        }

        public StandardWizardButton()
        {
        }

        public bool IsAjax { get; set; }
        public bool ExistDatatables { get; set; }
        public bool RequiredCer { get; set; }

        private List<WizardButton> _Buttons = null;
        public List<WizardButton> Buttons
        {
            get
            {
                return _Buttons;
            }
        }
        public void SetDisableButton(params string[] disableButtons)
        {
            if (disableButtons != null)
            {
                foreach (var item in disableButtons)
                {
                    Buttons.RemoveAll(b => b.Name == item);
                }
            }
        }
        private void SetStandardWizard(StandardStepWizardMode mode, StandardStepWizard step)
        {
            _Buttons = new List<WizardButton>();
            if (step == StandardStepWizard.First)
            {
                if (mode == StandardStepWizardMode.SaveDraft)
                {
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Reset, StandardWizardButtonName.Reset, Translation.CenterLang.Center.Reset, false, FaIcons.FaRefresh));
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Save, StandardWizardButtonName.Save, Translation.CenterLang.Center.Save, true, FaIcons.FaSave, requiredCer: RequiredCer));
                }
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Next, StandardWizardButtonName.Next, Translation.CenterLang.Center.SaveNext, true, FaIcons.FaForward, StandardIconPosition.AfterText, requiredCer: RequiredCer, type: StandButtonType.ButtonComfirmAjax, isValidate: true));

            }
            else if (step == StandardStepWizard.Last)
            {

                _Buttons.Add(new WizardButton(StandardWizardButtonName.Back, StandardWizardButtonName.Back, Translation.CenterLang.Center.Back, true, FaIcons.FaBackward));
                if (mode == StandardStepWizardMode.SaveDraft)
                {
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Reset, StandardWizardButtonName.Reset, Translation.CenterLang.Center.Reset, true, FaIcons.FaRefresh));
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Save, StandardWizardButtonName.Save, Translation.CenterLang.Center.Save, true, FaIcons.FaSave, requiredCer: RequiredCer));
                }
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Last, StandardWizardButtonName.Last, Translation.CenterLang.Center.Finished, true, FaIcons.FaCheck, requiredCer: RequiredCer));
            }
            else
            {
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Back, StandardWizardButtonName.Back, Translation.CenterLang.Center.Back, true, FaIcons.FaBackward));
                if (mode == StandardStepWizardMode.SaveDraft)
                {
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Reset, StandardWizardButtonName.Reset, Translation.CenterLang.Center.Reset, true, FaIcons.FaRefresh));
                    _Buttons.Add(new WizardButton(StandardWizardButtonName.Save, StandardWizardButtonName.Save, Translation.CenterLang.Center.Save, true, FaIcons.FaSave, requiredCer: RequiredCer));
                }
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Next, StandardWizardButtonName.Next, Translation.CenterLang.Center.SaveNext, true, FaIcons.FaForward, StandardIconPosition.AfterText, requiredCer: RequiredCer));

                if (mode == StandardStepWizardMode.NonHaveNextButton)
                {
                    _Buttons.RemoveAll(b => b.Name == StandardWizardButtonName.Next);
                }
            }
        }
        private void SetStandardWizard(StandardStepWizardMode mode)
        {
            _Buttons = new List<WizardButton>();

            _Buttons.Add(new WizardButton(StandardWizardButtonName.Back, StandardWizardButtonName.Back, Translation.CenterLang.Center.Previous, true, FaIcons.FaBackward));
            if (mode == StandardStepWizardMode.SaveDraft)
            {
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Reset, StandardWizardButtonName.Reset, Translation.CenterLang.Center.Reset, false, FaIcons.FaRefresh));
                _Buttons.Add(new WizardButton(StandardWizardButtonName.Save, StandardWizardButtonName.Save, Translation.CenterLang.Center.Save, true, FaIcons.FaSave, requiredCer: RequiredCer));
            }
            _Buttons.Add(new WizardButton(StandardWizardButtonName.Next, StandardWizardButtonName.Next, Translation.CenterLang.Center.SaveNext, true, FaIcons.FaForward, requiredCer: RequiredCer));
            _Buttons.Add(new WizardButton(StandardWizardButtonName.Last, StandardWizardButtonName.Last, Translation.CenterLang.Center.Finished, true, FaIcons.FaCheck, requiredCer: RequiredCer));
        }

        public void AddWizardButton(params WizardButton[] buttons)
        {
            if (buttons != null)
            {
                if (_Buttons == null)
                {
                    _Buttons = new List<WizardButton>();
                }
                foreach (var item in buttons)
                {
                    if (item.Index < 0)
                    {
                        _Buttons.Add(item);
                    }
                    else
                    {
                        _Buttons.Insert(item.Index, item);
                    }
                }
            }
        }

        public StandardWizardToorbarPosition ToolBarPosition { get; set; }
    }
    
    public class WizardButton
    {
        public WizardButton(
            string name,
            string value = null,
            string text = "",
            bool isConfirm = false,
            string icon = null,
            StandardIconPosition iconPosition = StandardIconPosition.BeforeText,
            int index = -1,
            string action = "",
            bool requiredCer = false,
            StandButtonType type = StandButtonType.Button,
            string formName = "form1",
            bool isValidate = false,
            bool overrideSubmit = false,
            string url = "")
        {
            IsComfirm = isConfirm;
            Name = name;
            Value = value;
            Text = text;
            Icon = icon;
            Index = index;
            Action = action;
            IconPosition = iconPosition;
            RequiredCer = requiredCer;
            _Type = type;
            FormName = formName;
            IsValidate = isValidate;
            OverrideSubmit = overrideSubmit;
            Url = url;
        }

        private StandButtonType _Type = StandButtonType.Button;
        public StandButtonType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public StandardIconPosition IconPosition { get; set; }
        public string Text { get; set; }
        [DefaultValue(-1)]
        public int Index { get; set; }
        public string Action { get; set; }
        public bool IsComfirm { get; set; }
        public bool RequiredCer { get; set; }
        public string FormName { get; set; }
        public string CallbackName { get; set; }
        public bool IsValidate { get; set; }
        public bool OverrideSubmit { get; set; }
        public string Url { get; set; }
    }

    public class StandardWizardButtonName
    {
        public const string Save = "Save";
        public const string SaveNext = "SaveNext";
        public const string SaveConfirm = "SaveConfirm";
        public const string Reset = "Reset";
        public const string Back = "Back";
        public const string Next = "Next";
        public const string Last = "Last";
    }
    
    public class StandardActionName
    {
        public const string Index = "Index";
        public const string Search = "Search";
        public const string Add = "Add";
        public const string AddDetail = "AddDetail";
        public const string Edit = "Edit";
        public const string EditDetail = "EditDetail";
        public const string Detail = "Detail";
        public const string Delete = "Delete";
        public const string Info = "Info";
        public const string Print = "Print";
        public const string Clear = "Clear";
        public const string Back = "Back";
        public const string SaveModify = "SaveModify";
        public const string SaveCreate = "SaveCreate";
        public const string Process = "Process";
        public const string Import = "Import";
        public const string Export = "Export";
        public const string Upload = "Upload";
        public const string ExportExcel = "ExportExcel";
        public const string ExportPDF = "ExportPDF";
        public const string Register = "Register";
        public const string Report = "Report";
        public const string Download = "Download";
        public const string SendData = "SendData";
    }

    public class StandardTempKey
    {
        public const string OperateData = "TempStandardOperateData";
    }
}