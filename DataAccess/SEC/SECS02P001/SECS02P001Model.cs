using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.SEC
{
    [Validator(typeof(SECS02P001Validator))]
    [Serializable]
    public class SECS02P001Model : StandardModel
    {
        public decimal? USG_ID { get; set; }
        [Display(Name = "USG_CODE", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string USG_CODE { get; set; }
        [Display(Name = "USG_NAME_TH", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string USG_NAME_TH { get; set; }
        [Display(Name = "USG_NAME_EN", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string USG_NAME_EN { get; set; }
        [Display(Name = "USG_STATUS", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string USG_STATUS { get; set; }
        public List<DDLCenterModel> USG_STATUS_MODEL { get; set; }
        [Display(Name = "USG_LEVEL", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string USG_LEVEL { get; set; }
        public List<DDLCenterModel> USG_LEVEL_MODEL { get; set; }

        [Display(Name = "SYS_NAME", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string SYS_CODE { get; set; }

        [Display(Name = "SYS_GROUP_NAME", ResourceType = typeof(Translation.SEC.SECS02P001))]
        public string SYS_GROUP_NAME { get; set; }
        public List<DDLCenterModel> SYS_GROUP_NAME_MODEL { get; set; }

        public List<DDLCenterModel> SYS_CODE_MODEL { get; set; }
        public List<SECS02P00101Model> PRIV_MODEL { get; set; }
    }

    public class SECS02P00101Model : StandardModel
    {
        public decimal? USRGRPPRIV_ID { get; set; }
        public decimal? USG_ID { get; set; }
        public string SYS_CODE { get; set; }
        public string PRG_CODE { get; set; }
        public string PRG_NAME { get; set; }
        public string PRG_NAME_TH { get; set; }
        public string PRG_NAME_EN { get; set; }
        public string PRG_TYPE { get; set; }
        public Nullable<decimal> SYS_SEQ { get; set; }
        public Nullable<decimal> PRG_SEQ { get; set; }
        public string ROLE_SEARCH { get; set; }
        public string ROLE_ADD { get; set; }
        public string ROLE_EDIT { get; set; }
        public string ROLE_DEL { get; set; }
        public string ROLE_PRINT { get; set; }
        public string SYS_STATUS { get; set; }
        public string PRG_STATUS { get; set; }
        public string SYS_NAME { get; set; }
        public string SYS_NAME_TH { get; set; }
        public string SYS_NAME_EN { get; set; }
    }

    public class SECS02P001Validator : AbstractValidator<SECS02P001Model>
    {

        public SECS02P001Validator()
        {
            RuleSet("Add", () =>
            {
                RuleFor(m => m.USG_CODE).Store("CD_USRGROUP_001", m => m.COM_CODE).NotEmpty();
                RuleFor(m => m.USG_NAME_TH).Store("CD_USRGROUP_003", m => m.COM_CODE).NotEmpty();
                RuleFor(m => m.USG_NAME_EN).Store("CD_USRGROUP_005", m => m.COM_CODE).NotEmpty();
                valid();
            });
            RuleSet("Edit", () =>
            {
                RuleFor(m => m.USG_CODE).Store("CD_USRGROUP_002", m => m.COM_CODE, m => m.USG_ID).NotEmpty();
                RuleFor(m => m.USG_NAME_TH).Store("CD_USRGROUP_004", m => m.COM_CODE, m => m.USG_ID).NotEmpty();
                RuleFor(m => m.USG_NAME_EN).Store("CD_USRGROUP_006", m => m.COM_CODE, m => m.USG_ID).NotEmpty();
                valid();
            });
        }

        private void valid()
        {
            RuleFor(m => m.USG_STATUS).NotEmpty();
            RuleFor(m => m.USG_LEVEL).NotEmpty();
        }
    }

}
