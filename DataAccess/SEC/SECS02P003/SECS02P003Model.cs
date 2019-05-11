using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SECS02P003Validator))]
    [Serializable]
    public class SECS02P003Model : StandardModel
    {
        public decimal? TITLE_ID { get; set; }
        [Display(Name = "TITLE_NAME_TH", ResourceType = typeof(Translation.SEC.SECS02P003))]
        public string TITLE_NAME_TH { get; set; }
        [Display(Name = "TITLE_NAME_EN", ResourceType = typeof(Translation.SEC.SECS02P003))]
        public string TITLE_NAME_EN { get; set; }
        [Display(Name = "TITLE_NAME", ResourceType = typeof(Translation.SEC.SECS02P003))]
        public string TITLE_NAME { get; set; }
    }

    public class SECS02P003Validator : AbstractValidator<SECS02P003Model>
    {
        public SECS02P003Validator()
        {
            //If Not Empty Set Start 1,...
            RuleSet("Add", () =>
            {
                RuleFor(m => m.TITLE_NAME_TH).NotEmpty().Store("CD_TITLE_001");
            });

            RuleSet("Edit", () =>
            {
                RuleFor(m => m.TITLE_NAME_TH).NotEmpty().Store("CD_TITLE_002", m => m.TITLE_ID);
            });
        }
    }
}