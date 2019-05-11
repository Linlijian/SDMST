using DataAccess.SEC;
using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;

namespace DataAccess.Users
{
    [Serializable]
    public partial class ModuleModel : StandardModel
    {
        public decimal ID { get; set; }
        public string NAME { get; set; }
        public Nullable<int> SEQUENCE { get; set; }
        public string SYS_CODE { get; set; }
        public string IMG { get; set; }
        public string IMG_COLOR { get; set; }
    }
    
}
