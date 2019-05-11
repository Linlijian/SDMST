using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SECS01P001Validator))]
    [Serializable]
    public class SECS01P001Model : StandardModel
    {
        [Display(Name = "COM_CODE", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_CODE { get; set; }

        [Display(Name = "BOND_NO", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string BOND_NO { get; set; }

        [Display(Name = "COM_BRANCH", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_BRANCH { get; set; }

        [Display(Name = "COM_NAME_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_NAME_T { get; set; }

        [Display(Name = "COM_FAC_NAME_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_NAME_T { get; set; }

        [Display(Name = "COM_NAME_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_NAME_E { get; set; }

        [Display(Name = "COM_FAC_NAME_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_NAME_E { get; set; }

        [Display(Name = "COM_BRANCH_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_BRANCH_T { get; set; }

        [Display(Name = "COM_BRANCH_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_BRANCH_E { get; set; }

        [Display(Name = "COM_ADDR1_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ADDR1_T { get; set; }

        [Display(Name = "COM_ADDR2_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ADDR2_T { get; set; }

        [Display(Name = "COM_SOI_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_SOI_T { get; set; }

        [Display(Name = "COM_ROAD_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ROAD_T { get; set; }

        [Display(Name = "COM_DISTRICT_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_DISTRICT_T { get; set; }

        [Display(Name = "COM_AMPHUR_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_AMPHUR_T { get; set; }

        [Display(Name = "COM_PROVINCE_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_PROVINCE_T { get; set; }

        [Display(Name = "COM_ADDR1_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ADDR1_E { get; set; }

        [Display(Name = "COM_ADDR2_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ADDR2_E { get; set; }

        [Display(Name = "COM_SOI_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_SOI_E { get; set; }

        [Display(Name = "COM_ROAD_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_ROAD_E { get; set; }

        [Display(Name = "COM_DISTRICT_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_DISTRICT_E { get; set; }

        [Display(Name = "COM_AMPHUR_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_AMPHUR_E { get; set; }

        [Display(Name = "COM_PROVINCE_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_PROVINCE_E { get; set; }

        [Display(Name = "COM_POST_CODE_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_POST_CODE_T { get; set; }

        [Display(Name = "COM_POST_CODE_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_POST_CODE_E { get; set; }
        public string COM_POST_CODE { get; set; }




        [Display(Name = "COM_FAC_ADDR1_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ADDR1_T { get; set; }

        [Display(Name = "COM_FAC_ADDR2_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ADDR2_T { get; set; }

        [Display(Name = "COM_FAC_SOI_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_SOI_T { get; set; }

        [Display(Name = "COM_FAC_ROAD_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ROAD_T { get; set; }

        [Display(Name = "COM_FAC_DISTRICT_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_DISTRICT_T { get; set; }

        [Display(Name = "COM_FAC_AMP_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_AMP_T { get; set; }

        [Display(Name = "COM_FAC_PRV_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_PRV_T { get; set; }

        [Display(Name = "COM_FAC_ADDR1_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ADDR1_E { get; set; }

        [Display(Name = "COM_FAC_ADDR2_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ADDR2_E { get; set; }

        [Display(Name = "COM_FAC_SOI_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_SOI_E { get; set; }

        [Display(Name = "COM_FAC_ROAD_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_ROAD_E { get; set; }

        [Display(Name = "COM_FAC_DISTRICT_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_DISTRICT_E { get; set; }

        [Display(Name = "COM_FAC_AMP_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_AMP_E { get; set; }

        [Display(Name = "COM_FAC_PRV_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_PRV_E { get; set; }

        [Display(Name = "COM_FAC_POST_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_POST_T { get; set; }

        [Display(Name = "COM_FAC_POST_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAC_POST_E { get; set; }
        public string COM_FAC_POST { get; set; }


        [Display(Name = "COM_TEL_NO", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_TEL_NO { get; set; }

        [Display(Name = "COM_FAX_NO", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_FAX_NO { get; set; }

        [Display(Name = "COM_E_MAIL", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_E_MAIL { get; set; }

        [Display(Name = "COM_NAME_SHORT_T", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_NAME_SHORT_T { get; set; }

        [Display(Name = "COM_NAME_SHORT_E", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_NAME_SHORT_E { get; set; }

        [Display(Name = "COM_LICENSE_ID", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_LICENSE_ID { get; set; }

        [Display(Name = "COM_TAX_ID", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_TAX_ID { get; set; }

        [Display(Name = "COM_USE_LANGUAGE", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string COM_USE_LANGUAGE { get; set; }

        [Display(Name = "USER_ID", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string USER_ID { get; set; }

        [Display(Name = "MODULE", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string MODULE { get; set; }

        //------DDL----//
        public IEnumerable<DDLCenterModel> COM_USE_LANGUAGE_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> COM_PROVINCE_T_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> COM_PROVINCE_E_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> COM_FAC_PRV_T_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> COM_FAC_PRV_E_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> USER_ID_MODEL { get; set; }

        public List<SECS01P001DetailPModel> Details { get; set; }

    }

    public class SECS01P001DetailPModel
    {
        [Display(Name = "MODULE", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string MODULE { get; set; }
        [Display(Name = "USER_ID", ResourceType = typeof(Translation.SEC.SECS01P001))]
        public string USER_ID { get; set; }

        public string COM_CODE { get; set; }

    }

    public class SECS01P001Validator : AbstractValidator<SECS01P001Model>
    {
        public SECS01P001Validator()
        {
            //If Not Empty Set Start 1,...
            RuleSet("Add", () =>
            {
                Valid();
                RuleFor(m => m.COM_CODE).Store("CD_SECS01P001_001").NotEmpty();
            });

            RuleSet("Edit", () =>
            {
                Valid();
            });
        }

        private void Valid()
        {
            RuleFor(m => m.COM_BRANCH).NotEmpty().Length(1, 5);
            RuleFor(m => m.COM_NAME_T).NotEmpty();
            RuleFor(m => m.COM_NAME_E).NotEmpty();
            RuleFor(m => m.COM_BRANCH_T).NotEmpty();
            RuleFor(m => m.COM_BRANCH_E).NotEmpty();
            RuleFor(m => m.COM_FAC_NAME_T).NotEmpty();
            RuleFor(m => m.COM_FAC_NAME_E).NotEmpty();

            //RuleFor(m => m.COM_BRANCH).NotEmpty().Length(1, 16);
        }
    }
}