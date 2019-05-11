using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DataAccess.MIS
{
    [Validator(typeof(MISS01P003Validator))]
    [Serializable]
    public class MISS01P003Model : StandardModel
    {
        public IEnumerable<DDLCenterModel> APP_CODE_MODEL { get; set; }
        public IEnumerable<DDLCenterModel> STATUS_MODEL { get; set; }

        [Display(Name = "STATUS", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string STATUS { get; set; }
        [Display(Name = "CANCEL", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string CANCEL { get; set; }
        [Display(Name = "AGREED", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string AGREED { get; set; }
        [Display(Name = "STATUS", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string ASSIGN_STATUS { get; set; }
        [Display(Name = "RESPONSE_BY", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string RESPONSE_BY { get; set; }
        [Display(Name = "COM_NAME_E", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string COM_NAME_E { get; set; }
        [Display(Name = "ISE_NO", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public string ISE_NO { get; set; }

        public string ISE_KEY { get; set; }
        public string FALG { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "ISE_DATE_OPENING", ResourceType = typeof(Translation.MIS.MISS01P003))]
        public Nullable<System.DateTime> ISE_DATE_OPENING { get; set; }
        

        

    }
    public class MISS01P003Validator : AbstractValidator<MISS01P003Model>
    {
        public MISS01P003Validator()
        {
            RuleSet("Add", () =>
            {
                Valid();
            });
            
        }

        private void Valid()
        {

        }
    }
}