using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilityLib;

namespace DataAccess
{
    public class StandardModel
    {
        [Display(Name = "APP_CODE", ResourceType = typeof(Translation.CenterLang.Center))]
        public string APP_CODE { get; set; }
        [Display(Name = "COM_CODE", ResourceType = typeof(Translation.CenterLang.Center))]
        public string COM_CODE { get; set; }

        [Display(Name = "CRET_BY", ResourceType = typeof(Translation.CenterLang.Center))]
        public string CRET_BY { get; set; }
        [Display(Name = "CRET_DATE", ResourceType = typeof(Translation.CenterLang.Center))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> CRET_DATE { get; set; }
        [Display(Name = "MNT_BY", ResourceType = typeof(Translation.CenterLang.Center))]
        public string MNT_BY { get; set; }
        [Display(Name = "MNT_DATE", ResourceType = typeof(Translation.CenterLang.Center))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> MNT_DATE { get; set; }


        public bool IsDefaultSearch { get; set; }

        public bool IsIdNotFound { get; set; }

        private bool _IsReadOnly = true;

        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set { _IsReadOnly = value; }
        }

        [Display(Name = "EXCEL_UPLOAD", ResourceType = typeof(Translation.CenterLang.Center))]
        public List<FileUpload> EXCEL_UPLOAD { get; set; }
    }
}
