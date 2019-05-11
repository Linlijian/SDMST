using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SECS01P003Validator))]
    [Serializable]
    public class SECS01P003Model : StandardModel
    {
        [Display(Name = "PRG_CODE", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_CODE { get; set; }
        [Display(Name = "PRG_NAME_TH", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_NAME_TH { get; set; }
        [Display(Name = "PRG_NAME_EN", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_NAME_EN { get; set; }
        [Display(Name = "PRG_TYPE", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_TYPE { get; set; }
        public IEnumerable<DDLCenterModel> PRG_TYPE_MODEL { get; set; }
        [Display(Name = "PRG_LEVEL", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public decimal? PRG_LEVEL { get; set; }
        [Display(Name = "PRG_STATUS", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_STATUS { get; set; }
        public IEnumerable<DDLCenterModel> PRG_STATUS_MODEL { get; set; }
        [Display(Name = "PRG_URL", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_URL { get; set; }

        [Display(Name = "PRG_IMG", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_IMG { get; set; }

        [Display(Name = "PRG_AREA", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_AREA { get; set; }

        [Display(Name = "PRG_CONTROLLER", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_CONTROLLER { get; set; }

        [Display(Name = "PRG_ACTION", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_ACTION { get; set; }

        [Display(Name = "PRG_PARAMETER", ResourceType = typeof(Translation.SEC.SECS01P003))]
        public string PRG_PARAMETER { get; set; }

        public string PRG_NO { get; set; }

        public string SERVER_NAME { get; set; }
    }

    public class SECS01P003Validator : AbstractValidator<SECS01P003Model>
    {
        public SECS01P003Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
                RuleFor(m => m.PRG_CODE).NotEmpty().Length(1, 15);
            });

            RuleSet("Edit", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(m => m.PRG_NAME_TH).NotEmpty().Length(1, 255);
            RuleFor(m => m.PRG_NAME_EN).NotEmpty().Length(1, 255);
            RuleFor(m => m.PRG_AREA).NotEmpty().Length(1, 50);
            RuleFor(m => m.PRG_CONTROLLER).NotEmpty().Length(1, 50);
            RuleFor(m => m.PRG_ACTION).NotEmpty().Length(1, 50);
            RuleFor(m => m.PRG_PARAMETER).NotEmpty().Length(1, 500);
            RuleFor(m => m.PRG_TYPE).NotEmpty().Length(1, 255);
            RuleFor(m => m.PRG_STATUS).NotEmpty().Length(1, 255);
        }
    }
}
