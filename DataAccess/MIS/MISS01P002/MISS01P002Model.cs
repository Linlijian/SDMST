using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DataAccess.MIS
{
    [Validator(typeof(MISS01P002Validator))]
    [Serializable]
    public class MISS01P002Model : StandardModel
    {
        [Display(Name = "NO", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public decimal? NO { get; set; }
        [Display(Name = "NO", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public decimal? ISE_NO { get; set; }

        [Display(Name = "COMPLETE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public int Complete { get; set; }
        [Display(Name = "INCOMPLETE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public int Incomplete { get; set; }
        [Display(Name = "TOTAL", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public int Total { get; set; }

        [Display(Name = "RESPONSE_BY", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string RESPONSE_BY { get; set; }
        [Display(Name = "ASSIGN_USER", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string ASSIGN_USER { get; set; }
        [Display(Name = "ASSIGN_STATUS", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string ASSIGN_STATUS { get; set; }
        [Display(Name = "ISE_STATUS", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string ISE_STATUS { get; set; }
        [Display(Name = "USER_ID", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string USER_ID { get; set; }
        [Display(Name = "FILE_ID", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string FILE_ID { get; set; }
        [Display(Name = "ISSUE_BY", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string ISSUE_BY { get; set; }
        [Display(Name = "TIMEOUT", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string TIMEOUT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISSUE_DATE_T", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISSUE_DATE_T { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISSUE_DATE_F", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISSUE_DATE_F { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISSUE_DATE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISSUE_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "TARGET_DATE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> TARGET_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_OPENING", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISE_DATE_OPENING { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_ONPROCESS", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISE_DATE_ONPROCESS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_FOLLOWUP", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISE_DATE_FOLLOWUP { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DEPLOY_QA", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> DEPLOY_QA { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DEPLOY_PD", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> DEPLOY_PD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_GOLIVE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISE_DATE_GOLIVE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_CLOSE", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public Nullable<System.DateTime> ISE_DATE_CLOSE { get; set; }

        [Display(Name = "STATUS", ResourceType = typeof(Translation.MIS.MISS01P002))]
        public string STATUS { get; set; }
        public IEnumerable<DDLCenterModel> STATUS_MODEL { get; set; }
        
        //===================================STATUS OPENING===========================================
        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> ASSIGN_USER_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> TIMEOUT_MODEL { get; set; }

        public string ACTIVE_STEP { get; set; }
        public string FLAG { get; set; }
        public string ISE_KEY { get; set; }
    }
    public class MISS01P002Validator : AbstractValidator<MISS01P002Model>
    {
        public MISS01P002Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
            });
            RuleSet("FilePacket", () =>
            {
                RuleFor(t => t.FILE_ID).NotEmpty();
            });
            RuleSet("Status3Followup", () =>
            {
               // RuleFor(t => t.APP_CODE).NotEmpty();
            });
            RuleSet("Assignment", () =>
            {
                RuleFor(t => t.ASSIGN_USER).NotEmpty();
            });
        }

        private void Valid()
        {

        }
    }
}