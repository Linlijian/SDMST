using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Admin.Dashboard
{
    [Validator(typeof(DashboardAEValidator))]
    [Serializable]
    public class DashboardModel : StandardModel
    {
        public List<TreeModel> TreeView01admModel { get; set; }

        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        [Display(Name = "ACT_DATE", ResourceType = typeof(Translation.Admin.Dashboard))]
        public DateTime? ACT_DATE { get; set; }

        [Display(Name = "TODO_TYPE", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string TODO_TYPE { get; set; }

        [Display(Name = "MS_CORP_GROUP", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string MS_CORP_GROUP { get; set; }

        [Display(Name = "MS_CORP_GROUP", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string MS_CORP_GROUP_TH { get; set; }

        public string MS_CORP_ID { get; set; }

        [Display(Name = "MS_CORP_NAME_TH", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string MS_CORP_NAME_TH { get; set; }

        [Display(Name = "USER_ID", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string USER_ID { get; set; }

        [Display(Name = "USER_FNAME_TH", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string USER_FNAME_TH { get; set; }

        [Display(Name = "USER_LNAME_TH", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string USER_LNAME_TH { get; set; }

        [Display(Name = "USER_FULLNAME_TH", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string USER_FULLNAME_TH { get; set; }

        [Display(Name = "POSITION", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string POSITION { get; set; }

        public string FIX_DETAIL_NAME_TH { get; set; }
        public string PRG_CODE { get; set; }
        public string PRG_NAME_TH { get; set; }
        public string PRG_CONTROLLERNAME { get; set; }
        public string PRG_AREA { get; set; }
        public string PRG_ACTIONNAME { get; set; }
        public string TODO_ACTION { get; set; }

        public decimal? SEC_CERTIFICATE_ID { get; set; }
        public string CER_DESC { get; set; }

        [Display(Name = "VALID_FROM", ResourceType = typeof(Translation.Admin.Dashboard))]
        public DateTime? VALID_FROM { get; set; }

        [Display(Name = "VALID_TO", ResourceType = typeof(Translation.Admin.Dashboard))]
        public DateTime? VALID_TO { get; set; }

        [Display(Name = "ISSUER", ResourceType = typeof(Translation.Admin.Dashboard))]
        public string ISSUER { get; set; }
    }
    public class DashboardAEValidator : AbstractValidator<DashboardModel>
    {
        public DashboardAEValidator()
        {
            ////If Not Empty Set Start 1,...
            //RuleSet("Add", () =>
            //{
            //    RuleFor(x => x.COM_CODE).NotEmpty().Length(1, 25).Store("ChkDupApplication");
            //    Valid();
            //});

            //RuleSet("Edit", () =>
            //{
            //    Valid();
            //});
        }

        private void Valid()
        {
            //RuleFor(x => x.APP_NAME_TH).NotEmpty().Length(1, 250);
            //RuleFor(x => x.APP_OBJECTIVE).NotEmpty().Length(1, 250);
            //RuleFor(x => x.APP_GROUP_CODE).NotEmpty();
            //RuleFor(x => x.STATUS).NotEmpty().ActiveAndStatusMatch(m => m.ACTIVE);
            //RuleFor(x => x.ACTIVE).NotEmpty().ActiveAndStatusMatch(m => m.STATUS);

            ////Else Not Empty Set Start 0,...
            //RuleFor(x => x.APP_NAME_EN).Length(0, 250);
            //RuleFor(x => x.APP_CONTACT_PERSON).Length(0, 250);
            //RuleFor(x => x.APP_CONTACT_TEL).Length(0, 250);
            //RuleFor(x => x.APP_CONTACT_EMAIL).Length(0, 250);
            //RuleFor(x => x.APP_URL).Length(0, 150);

            //RuleFor(m => m.APP_GOLIFE_DT).NotEmpty().GreaterThanOrEqualTo(m => m.APP_EXPIRE_DT);
            //RuleFor(m => m.APP_EXPIRE_DT).NotEmpty().LessThanOrEqualTo(m => m.APP_GOLIFE_DT);
            ////RuleFor(m => m.APP_EXPIRE_DT).NotEmpty().LessThan(m => m.APP_GOLIFE_DT);
        }
    }
}