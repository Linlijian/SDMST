using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilityLib;

namespace DataAccess.Ux
{
    public class ElectronicSignModel : StandardModel
    {
        public string PRG_CODE { get; set; }
        public decimal? DOCUMENT_TYPE_ID { get; set; }
        public decimal? SECTION_GROUP_ID { get; set; }
        public decimal? COVER_SHEET_SEND_ID { get; set; }
        public decimal? HEADER_INPUT_ID { get; set; }
        public decimal? DETAIL_INPUT_ID { get; set; }

        [Display(Name = "RECEIVE_DT", ResourceType = typeof(Translation.Ux.File))]
        public DateTime? RECEIVE_DT { get; set; }
        [Display(Name = "MS_CORP_NAME_TH", ResourceType = typeof(Translation.Ux.File))]
        public string MS_CORP_NAME_TH { get; set; }
        [Display(Name = "MS_CORP_ADDR1", ResourceType = typeof(Translation.Ux.File))]
        public string MS_CORP_ADDR1 { get; set; }
        [Display(Name = "DOCUMENT_TYPE_NAME_TH", ResourceType = typeof(Translation.Ux.File))]
        public string DOCUMENT_TYPE_NAME_TH { get; set; }
        [Display(Name = "PERIOD_NAME", ResourceType = typeof(Translation.Ux.File))]
        public string PERIOD_NAME { get; set; }
        [Display(Name = "SEND_BY_TH", ResourceType = typeof(Translation.Ux.File))]
        public string SEND_BY_TH { get; set; }
        [Display(Name = "REFERENCE_DOC", ResourceType = typeof(Translation.Ux.File))]
        public string REFERENCE_DOC { get; set; }
        [Display(Name = "CERTIFICATE_NUMBER", ResourceType = typeof(Translation.Ux.File))]
        public string CERTIFICATE_NUMBER { get; set; }
        [Display(Name = "RECEIVE_DOC", ResourceType = typeof(Translation.Ux.File))]
        public string RECEIVE_DOC { get; set; }
        [Display(Name = "SEND_NO", ResourceType = typeof(Translation.Ux.File))]
        public string SEND_NO { get; set; }
        [Display(Name = "RECEIVE_BY_NAME_TH", ResourceType = typeof(Translation.Ux.File))]
        public string RECEIVE_BY_NAME_TH { get; set; }
        [Display(Name = "COM_ADDR1_T", ResourceType = typeof(Translation.Ux.File))]
        public string COM_ADDR1_T { get; set; }
        [Display(Name = "APP_VERSION", ResourceType = typeof(Translation.Ux.File))]
        public string APP_VERSION { get; set; }
        [Display(Name = "DATA_FORMAT", ResourceType = typeof(Translation.Ux.File))]
        public string DATA_FORMAT { get; set; }
        [Display(Name = "SIGNATURE_SIGN", ResourceType = typeof(Translation.Ux.File))]
        public string SIGNATURE_SIGN { get; set; }

        [Display(Name = "DOC_TYPE_NAME_TH", ResourceType = typeof(Translation.Ux.File))]
        public string DOC_TYPE_NAME_TH { get; set; }

        public List<FileUpload> Files { get; set; }
    }
}
