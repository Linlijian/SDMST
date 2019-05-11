using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.MIS
{
    [Validator(typeof(MISS02P001Validator))]
    [Serializable]
    public class MISS02P001Model : StandardModel
    {
        [Display(Name = "YEAR", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string YEAR { get; set; }
        [Display(Name = "MONTH", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string MONTH { get; set; }
        [Display(Name = "DAY", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DAY { get; set; }

        [Display(Name = "DEPLOYMENT_DATE", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOYMENT_DATE { get; set; }
        [Display(Name = "DEPLOY_PRG", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOY_PRG { get; set; }
        [Display(Name = "DEPLOY_USER", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOY_USER { get; set; }

        [Display(Name = "TYPE_DAY", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string TYPE_DAY { get; set; }
        public IEnumerable<DDLCenterModel> TYPE_DAY_MODEL { get; set; }

        public string ALL_SATURDAY_W { get; set; } //W
        public string ALL_HOLIDAY { get; set; } //H
        public string ALL_SPP { get; set; } //S
        public string ALL_DEPLOYMENT_IT { get; set; } //I
        public string ALL_DEPLOYMENT_DATE { get; set; } //D
        public string CLIENT_ID { get; set; }
        public string YYYY { get; set; }
        public string MM { get; set; }
        public string DD { get; set; }
        public string FILE_EXCEL { get; set; }
        public string FLAG { get; set; }
        public string REMARK { get; set; }

        public string ERROR_CODE { get; set; }
        public string ERROR_MSG { get; set; }
        public string APP_CODE { get; set; }

        public List<MISS02P001DetailPModel> Details { get; set; }
        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }

        public System.Data.DataSet ds { get; set; }
    }

    public class MISS02P001DetailPModel
    {
        [Display(Name = "TYPE_DAY", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string TYPE_DAY { get; set; }
        public IEnumerable<DDLCenterModel> TYPE_DAY_MODEL { get; set; }

        [Display(Name = "YEAR", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string YEAR { get; set; }
        [Display(Name = "MONTH", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string MONTH { get; set; }
        [Display(Name = "DAY", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DAY { get; set; }

        [Display(Name = "DEPLOYMENT_DATE", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOYMENT_DATE { get; set; }

        [Display(Name = "DEPLOY_PRG", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOY_PRG { get; set; }
        [Display(Name = "DEPLOY_USER", ResourceType = typeof(Translation.MIS.MISS02P001))]
        public string DEPLOY_USER { get; set; }

        public string COM_CODE { get; set; }
    }
    public class MISS02P001Validator : AbstractValidator<MISS02P001Model>
    {
        public MISS02P001Validator()
        {
            RuleSet("Add", () =>
            {
                RuleFor(t => t.TYPE_DAY).NotEmpty();
                Valid();
            });
            RuleSet("Edit", () =>
            {
                RuleFor(t => t.TYPE_DAY).NotEmpty();
                Valid();
            });
            RuleSet("Upload", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(t => t.YEAR).NotEmpty();
            RuleFor(t => t.APP_CODE).NotEmpty();
        }
        private void CD_OLD()
        {
            RuleFor(m => m.YEAR).Store("CD_MISS02P001_001", m => m.APP_CODE, m => m.MONTH, m => m.DAY).NotEmpty();
            RuleFor(m => m.APP_CODE).Store("CD_MISS02P001_001", m => m.YEAR, m => m.MONTH, m => m.DAY).NotEmpty();
            RuleFor(m => m.MONTH).Store("CD_MISS02P001_001", m => m.APP_CODE, m => m.YEAR, m => m.DAY).NotEmpty();
            RuleFor(m => m.DAY).Store("CD_MISS02P001_001", m => m.APP_CODE, m => m.MONTH, m => m.YEAR).NotEmpty();
        }
    }
}