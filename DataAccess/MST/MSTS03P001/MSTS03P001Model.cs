using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.MST
{
    [Validator(typeof(MSTS03P001Validator))]
    [Serializable]
    public class MSTS03P001Model : StandardModel
    {
        [Display(Name = "PRIORITY_NAME", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string PRIORITY_NAME { get; set; }
        [Display(Name = "ISSUE_TYPE", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string ISSUE_TYPE { get; set; }
        [Display(Name = "RES_TIME", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public decimal? RES_TIME { get; set; }
        [Display(Name = "T_RES_TIME", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public decimal? T_RES_TIME { get; set; }
        [Display(Name = "REMASK", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string REMASK { get; set; }
        [Display(Name = "IS_FREE", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string IS_FREE { get; set; }

        [Display(Name = "KEY_ID", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string KEY_ID { get; set; }
        public IEnumerable<DDLCenterModel> KEY_ID_MODEL { get; set; }
        [Display(Name = "T_RES_TYPE", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string T_RES_TYPE { get; set; }
        public IEnumerable<DDLCenterModel> T_RES_TYPE_MODEL { get; set; }
        [Display(Name = "RES_TYPE", ResourceType = typeof(Translation.MST.MSTS03P001))]
        public string RES_TYPE { get; set; }
        public IEnumerable<DDLCenterModel> RES_TYPE_MODEL { get; set; }
        
        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }

        public decimal? PIT_ID { get; set; }
        public bool IS_USED { get; set; }
    }
    public class MSTS03P001Validator : AbstractValidator<MSTS03P001Model>
    {
        public MSTS03P001Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
                RuleFor(t => t.KEY_ID).NotEmpty();
            });
            RuleSet("Edit", () =>
            {
                Valid();
            });
        }
        
        private void Valid()
        {
            RuleFor(m => m.APP_CODE).Store("CD_MSTS03P001_001", m => m.PRIORITY_NAME).NotEmpty();
            RuleFor(m => m.PRIORITY_NAME).Store("CD_MSTS03P001_001", m => m.APP_CODE).NotEmpty();
            RuleFor(t => t.RES_TIME).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.T_RES_TIME).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.T_RES_TYPE).NotEmpty();
            RuleFor(t => t.RES_TYPE).NotEmpty();
        }
    }
}