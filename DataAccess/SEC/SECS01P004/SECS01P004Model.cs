using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SECS01P004Validator))]
    [Serializable]
    public class SECS01P004Model : StandardModel
    {
        [Display(Name = "SYS_CODE", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public string SYS_CODE { get; set; }
        [Display(Name = "SYS_NO", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public int? SYS_NO { get; set; }
        [Display(Name = "SYS_SEQ", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public Nullable<decimal> SYS_SEQ { get; set; }
        [Display(Name = "SYS_NAME_TH", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public string SYS_NAME_TH { get; set; }
        [Display(Name = "SYS_NAME_EN", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public string SYS_NAME_EN { get; set; }
        [Display(Name = "SYS_STATUS", ResourceType = typeof(Translation.SEC.SECS01P004))]
        public string SYS_STATUS { get; set; }
        public IEnumerable<DDLCenterModel> SYS_STATUS_MODEL { get; set; }

    }

    public class SECS01P004Validator : AbstractValidator<SECS01P004Model>
    {
        public SECS01P004Validator()
        {
            //If Not Empty Set Start 1,...
            RuleSet("Add", () =>
            {
                Valid();
            });

            RuleSet("Edit", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(m => m.SYS_CODE).NotEmpty();
            RuleFor(m => m.SYS_NAME_TH).NotEmpty();
            RuleFor(m => m.SYS_NAME_EN).NotEmpty();
            RuleFor(m => m.SYS_STATUS).NotEmpty();
        }
    }
}