using System.ComponentModel;

namespace WEBAPP.Helper
{
    public enum StandardWizardToorbarPosition
    {
        Top,
        Bottom
    }
    public enum StandardStepWizardMode
    {
        SaveDraft,
        Other,
        NonHaveNextButton
    }
    public enum StandardStepWizard
    {
        First,
        Last,
        Other
    }
    public enum StandardButtonMode
    {
        Index,
        Create,
        Modify,
        View,
        Other
    }

    public enum StandButtonType
    {
        [Description("Submit")]
        Submit,
        [Description("Button")]
        Button,
        [Description("ButtonNewWindows")]
        ButtonNewWindows,
        [Description("ButtonAjax")]
        ButtonAjax,
        [Description("ButtonComfirm")]
        ButtonComfirm,
        [Description("ButtonComfirmAjax")]
        ButtonComfirmAjax,
        [Description("ButtonComfirmSubmit")]
        ButtonComfirmSubmit,
        [Description("ButtonComfirmNewWindows")]
        ButtonComfirmNewWindows
    }

    public enum StandardIconPosition
    {
        AfterText,
        BeforeText
    }

    public enum ColumnsType
    {
        [Description("")]
        None,
        [Description("date")]
        Date,
        [Description("date")]
        DateTime,
        [Description("time")]
        Time,
        [Description("num-fmt")]
        Number,
        [Description("num-fmt")]
        NumberFormat,
        [Description("num-fmt")]
        NumberFormat2,
        [Description("num-fmt")]
        NumberFormat3,
        [Description("num-fmt")]
        NumberFormat4,
        [Description("num-fmt")]
        NumberFormat6,
        [Description("html-num")]
        NumberFormat8,
        [Description("html-num")]
        NumberFormat10,
        [Description("html-num")]
        html_num,
        [Description("html-num-fmt")]
        html_num_fmt,
        [Description("html")]
        html,
        [Description("string")]
        String,
        [Description("")]
        File,
        [Description("")]
        FileMultiple,
        [Description("")]
        Select,
        [Description("")]
        Checkbox,
        [Description("")]
        Autocomlete
    }
    public enum ColumnsTextAlign
    {
        [Description("")]
        None,
        [Description("dt-body-left")]
        Left,
        [Description("dt-body-center")]
        Center,
        [Description("dt-body-right")]
        Right,
        [Description("dt-body-justify")]
        Justify,
        [Description("dt-body-nowrap")]
        Nowrap,
        [Description("dt-body-wordbreak")]
        Wordbreak
    }
    public enum ColumnsWidthType
    {
        [Description("")]
        Pixel,
        [Description("%")]
        Percentage
    }
    public enum ModalSize
    {
        [Description("")]
        Default,
        [Description("modal-sm")]
        Sm,
        [Description("modal-lg")]
        Lg,
        [Description("modal-xl")]
        Xl,
        [Description("modal-fullscreen")]
        Fullscreen
    }
    public enum WidgetColor
    {
        [Description("")]
        None,
        [Description("widget-color-blue")]
        Blue,
        [Description("widget-color-blue2")]
        Blue2,
        [Description("widget-color-blue3")]
        Blue3,
        [Description("widget-color-green")]
        Green,
        [Description("widget-color-green2")]
        Green2,
        [Description("widget-color-green3")]
        Green3,
        [Description("widget-color-red")]
        Red,
        [Description("widget-color-red2")]
        Red2,
        [Description("widget-color-red3")]
        Red3,
        [Description("widget-color-purple")]
        Purple,
        [Description("widget-color-pink")]
        Pink,
        [Description("widget-color-orange")]
        Orange,
        [Description("widget-color-dark")]
        Dark,
        [Description("widget-color-grey")]
        Grey
    }
    public enum TextColor
    {
        [Description("")]
        None,
        [Description("dark")]
        dark,
        [Description("white")]
        white,
        [Description("red")]
        red,
        [Description("red2")]
        red2,
        [Description("light-red")]
        light_red,
        [Description("blue")]
        blue,
        [Description("light-blue")]
        light_blue,
        [Description("green")]
        green,
        [Description("light-green")]
        light_green,
        [Description("orange")]
        orange,
        [Description("orange2")]
        orange2,
        [Description("light-orange")]
        light_orange,
        [Description("purple")]
        purple,
        [Description("pink")]
        pink,
        [Description("pink2")]
        pink2,
        [Description("brown")]
        brown,
        [Description("grey")]
        grey,
        [Description("light-grey")]
        light_grey
    }

    public enum StandardIconProcess
    {
        [Description("WAITING")]
        WAITING,
        [Description("PROCESSING")]
        PROCESSING,
        [Description("COMPLETE")]
        COMPLETE,
        [Description("INCOMPLETE")]
        INCOMPLETE,
        [Description("WARNING")]
        WARNING,
    }

    public enum LogInResult
    {
        Success = 0,
        WrongUserNameOrPassword = 1,
        UserIsLocked = 2,
        LoginFailOver = 3,
        UserNotActive = 4,
        UserGroupNotAuthorized = 5,
        UserChangePassword = 6,
        ComCodeNotAuthorized = 7,
        UserGroupNotActive = 8

    }
}