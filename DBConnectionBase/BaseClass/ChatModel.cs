using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class ChatModel : StandardModel
    {
        public string WF_STS { get; set; }

        public string MS_CORP_GROUP { get; set; }

        public string MS_CORP_NAME_TH { get; set; }

        public string USER_NAME_TH { get; set; }

        public string USER_ID { get; set; }

        public string REFERENCE_DOC { get; set; }

        public string APPROVED_NO { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SEND_DT { get; set; }

        public string SEND_NO { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? APPROVED_DT { get; set; }

        public string ROW1ST { get; set; }

        public string ROW2ND { get; set; }
    }
}
