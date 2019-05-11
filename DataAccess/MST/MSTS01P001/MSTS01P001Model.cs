using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.MST
{
    [Validator(typeof(MSTS01P001Validator))]
    [Serializable]
    public class MSTS01P001Model : StandardModel
    {

        [Display(Name = "ISSUE_TYPE", ResourceType = typeof(Translation.MST.MSTS01P001))]
        public string ISSUE_TYPE { get; set; }
        public IEnumerable<DDLCenterModel> ISSUE_TYPE_MODEL { get; set; }
        [Display(Name = "TYPE_RATE", ResourceType = typeof(Translation.MST.MSTS01P001))]
        public string TYPE_RATE { get; set; }
        public IEnumerable<DDLCenterModel> TYPE_RATE_MODEL { get; set; }

        [Display(Name = "MAN_PLM_SA", ResourceType = typeof(Translation.MST.MSTS01P001))]
        public decimal? MAN_PLM_SA { get; set; }
        [Display(Name = "MAN_PLM_QA", ResourceType = typeof(Translation.MST.MSTS01P001))]
        public decimal? MAN_PLM_QA { get; set; }
        [Display(Name = "MAN_PLM_PRG", ResourceType = typeof(Translation.MST.MSTS01P001))]
        public decimal? MAN_PLM_PRG { get; set; }

        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }

    }
    public class MSTS01P001Validator : AbstractValidator<MSTS01P001Model>
    {
        public MSTS01P001Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
                RuleFor(m => m.APP_CODE).Store("CD_MSTS01P001_001", m => m.ISSUE_TYPE, m => m.TYPE_RATE).NotEmpty();
                RuleFor(m => m.ISSUE_TYPE).Store("CD_MSTS01P001_001", m => m.APP_CODE, m => m.TYPE_RATE).NotEmpty();
                RuleFor(m => m.TYPE_RATE).Store("CD_MSTS01P001_001", m => m.APP_CODE, m => m.ISSUE_TYPE).NotEmpty();
            });
            RuleSet("Edit", () =>
            {
                Valid();
                RuleFor(m => m.APP_CODE).Store("CD_MSTS01P001_001", m => m.ISSUE_TYPE, m => m.TYPE_RATE).NotEmpty();
                RuleFor(m => m.ISSUE_TYPE).Store("CD_MSTS01P001_001", m => m.APP_CODE, m => m.TYPE_RATE).NotEmpty();
                RuleFor(m => m.TYPE_RATE).Store("CD_MSTS01P001_001", m => m.APP_CODE, m => m.ISSUE_TYPE).NotEmpty();
            });
        }

        private void Valid()
        {
            RuleFor(t => t.MAN_PLM_SA).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_QA).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
            RuleFor(t => t.MAN_PLM_PRG).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(Convert.ToDecimal(99.9)).WithMessage(Translation.CenterLang.Validate.OneNumber2Digit1);
        }
    }
}