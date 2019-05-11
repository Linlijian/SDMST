using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;

namespace DataAccess.SEC
{
    [Serializable]
    public partial class MenuModel : StandardModel
    {
        public string USER_ID { get; set; }
        public string USG_LEVEL { get; set; }
        public Nullable<decimal> USG_ID { get; set; }
        public string SYS_GROUP_NAME { get; set; }
        public string SYS_CODE { get; set; }
        public string SYS_NAME_TH { get; set; }
        public string SYS_NAME_EN { get; set; }
        public string PRG_CODE { get; set; }
        public string PRG_NAME_TH { get; set; }
        public string PRG_NAME_EN { get; set; }
        public Nullable<decimal> SYS_SEQ { get; set; }
        public Nullable<decimal> PRG_SEQ { get; set; }
        public string ROLE_SEARCH { get; set; }
        public string ROLE_ADD { get; set; }
        public string ROLE_EDIT { get; set; }
        public string ROLE_DEL { get; set; }
        public string ROLE_PRINT { get; set; }

        public bool IsROLE_SEARCH { get; set; }
        public bool IsROLE_ADD { get; set; }
        public bool IsROLE_EDIT { get; set; }
        public bool IsROLE_DEL { get; set; }
        public bool IsROLE_PRINT { get; set; }

        public string PRG_AREA { get; set; }
        public string PRG_CONTROLLER { get; set; }
        public string PRG_ACTION { get; set; }
        public string PRG_IMG { get; set; }
        public string PRG_PARAMETER { get; set; }
    }
    
}
