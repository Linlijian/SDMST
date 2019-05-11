using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DataAccess.MIS
{
    [Validator(typeof(MISS01P001Validator))]
    [Serializable]
    public class MISS01P001Model : StandardModel
    {
        [Display(Name = "MODULE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string MODULE { get; set; }
        [Display(Name = "ASSIGN_USER", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ASSIGN_USER { get; set; }
        [Display(Name = "DETAIL", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string DETAIL { get; set; }
        [Display(Name = "ROOT_CAUSE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ROOT_CAUSE { get; set; }
        [Display(Name = "SOLUTION", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string SOLUTION { get; set; }
        [Display(Name = "EFFECTS", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string EFFECTS { get; set; }
        [Display(Name = "ISSUE_BY", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ISSUE_BY { get; set; }
        [Display(Name = "RESPONSE_BY", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string RESPONSE_BY { get; set; }
        [Display(Name = "FILE_ID", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string FILE_ID { get; set; }
        [Display(Name = "PROGRAM_NAME", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string PROGRAM_NAME { get; set; }
        [Display(Name = "FILE_NAME", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ISSUE_IMG { get; set; }
        //[Display(Name = "FILE_NAME", ResourceType = typeof(Translation.MIS.MISS01P001))]
        //public HttpPostedFileBase ISSUE_IMG { get; set; }
        [Display(Name = "COMMENT", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string REMARK { get; set; }
        [Display(Name = "RESOLUTION_TARGET", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string RESOLUTION_TARGET { get; set; }
        [Display(Name = "RESPONSE_TARGET", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string RESPONSE_TARGET { get; set; }
        [Display(Name = "ISSUE_DATE_PERIOD", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ISSUE_DATE_PERIOD { get; set; }
        [Display(Name = "MENU", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string MENU { get; set; }
        [Display(Name = "PRG_NAME", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string PRG_NAME { get; set; }

        [Display(Name = "PRIORITY", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string PRIORITY { get; set; }
        public IEnumerable<DDLCenterModel> PRIORITY_MODEL { get; set; }
        [Display(Name = "ISSUE_TYPE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string ISSUE_TYPE { get; set; }
        public IEnumerable<DDLCenterModel> ISSUE_TYPE_MODEL { get; set; }
        [Display(Name = "DEFECT", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string DEFECT { get; set; }
        public IEnumerable<DDLCenterModel> DEFECT_MODEL { get; set; }
        [Display(Name = "STATUS", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string STATUS { get; set; }
        public IEnumerable<DDLCenterModel> STATUS_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> RESPONSE_BY_MODEL { get; set; }

        public string ACTIVE_STEP { get; set; }
        public string STR_ISSUE_DATE { get; set; }
        public string STR_RESPONSE_DATE { get; set; }
        public string STR_TARGET_DATE { get; set; }
        public string STR_CLOSE_DATE { get; set; }
        public string STR_RDEPLOY_PD { get; set; }
        public string STR_DEPLOY_QA { get; set; }
        public string USER_ID { get; set; }
        public string CLIENT_ID { get; set; }
        public string FILE_EXCEL { get; set; }
        public string ERROR_CODE { get; set; }
        public string FLAG { get; set; }
        public string PRG_CODE { get; set; }
        public string ISS_YEAR { get; set; }
        public string ISS_TYPE { get; set; }

        public System.Data.DataSet ds { get; set; }


        [Display(Name = "NO", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? NO { get; set; }
        [Display(Name = "REF_NO", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? REF_NO { get; set; }
        [Display(Name = "ESSR_NO", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? ESSR_NO { get; set; }
        [Display(Name = "MAN_PLM_SA", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? MAN_PLM_SA { get; set; }
        [Display(Name = "MAN_PLM_QA", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? MAN_PLM_QA { get; set; }
        [Display(Name = "MAN_PLM_PRG", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? MAN_PLM_PRG { get; set; }
        [Display(Name = "MAN_PLM_PL", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? MAN_PLM_PL { get; set; }
        [Display(Name = "MAN_PLM_DBA", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public decimal? MAN_PLM_DBA { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ISSUE_DATE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ISSUE_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "TARGET_DATE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> TARGET_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "RESPONSE_DATE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> RESPONSE_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "CLOSE_DATE", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> CLOSE_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DEPLOY_QA", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> DEPLOY_QA { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DEPLOY_PD", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> DEPLOY_PD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ACTUAL_RESTIME", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> ACTUAL_RESTIME { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ACTUAL_RESTIME_QA", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> ACTUAL_RESTIME_QA { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ACTUAL_RESTIME_PD", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> ACTUAL_RESTIME_PD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ACTUAL_RESTIME_CL", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public Nullable<System.DateTime> ACTUAL_RESTIME_CL { get; set; }

        [Display(Name = "MENU_R", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string MENU_R { get; set; }
        [Display(Name = "PRG_NAME_R", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string PRG_NAME_R { get; set; }
        [Display(Name = "MODULE_R", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string MODULE_R { get; set; }
        [Display(Name = "PROGRAM_NAME_R", ResourceType = typeof(Translation.MIS.MISS01P001))]
        public string PROGRAM_NAME_R { get; set; }
    }
    public class MISS01P001Validator : AbstractValidator<MISS01P001Model>
    {
        public MISS01P001Validator()
        {
            RuleSet("Add", () =>
            {
                NewValid();
            });
            RuleSet("AddRefNo", () =>
            {
                NewValid();
            });
            RuleSet("Edit", () =>
            {
                Valid();
            });
            RuleSet("Upload", () =>
            {
                RuleFor(t => t.APP_CODE).NotEmpty();
            });
        }
        private void NewValid()
        {
            RuleFor(t => t.ISSUE_BY).NotEmpty();
            RuleFor(t => t.STR_ISSUE_DATE).NotEmpty().WithMessage(Translation.CenterLang.Validate.Issue_date);
            RuleFor(t => t.MODULE).NotEmpty();
            RuleFor(t => t.NO).NotEmpty();
            RuleFor(t => t.APP_CODE).NotEmpty();
            RuleFor(t => t.DETAIL).NotEmpty();
            RuleFor(t => t.RESPONSE_BY).NotEmpty();
        }
        private void Valid()
        {
            RuleFor(t => t.ISSUE_BY).NotEmpty();
            RuleFor(t => t.STR_ISSUE_DATE).NotEmpty().WithMessage(Translation.CenterLang.Validate.Issue_date);
            RuleFor(t => t.MODULE).NotEmpty();
            RuleFor(t => t.STR_RESPONSE_DATE).NotEmpty().WithMessage(Translation.CenterLang.Validate.Response_date);
            RuleFor(t => t.PRIORITY).NotEmpty();
            RuleFor(t => t.ISSUE_TYPE).NotEmpty();
            RuleFor(t => t.DEFECT).NotEmpty();
            RuleFor(t => t.STR_TARGET_DATE).NotEmpty().WithMessage(Translation.CenterLang.Validate.Traget_date);

            RuleFor(t => t.NO).NotEmpty();
            RuleFor(t => t.APP_CODE).NotEmpty();
            RuleFor(t => t.DETAIL).NotEmpty();
            RuleFor(t => t.RESPONSE_BY).NotEmpty();

            RuleFor(t => t.MAN_PLM_PL).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_DBA).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_PRG).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_QA).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_SA).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
        }
    }
}