using System.Collections.Generic;
using UtilityLib;

namespace DataAccess
{
    public class FileModel : StandardModel
    {
        public decimal? ROW_ID { get; set; }
        public decimal? ID { get; set; }
        public string Name { get; set; }
        public string PRG_CODE { get; set; }
        public decimal? DOCUMENT_TYPE_ID { get; set; }
        public decimal? SECTION_GROUP_ID { get; set; }
        public decimal? COVER_SHEET_SEND_ID { get; set; }
        public decimal? HEADER_INPUT_ID { get; set; }
        public decimal? DETAIL_INPUT_ID { get; set; }
        public string PATH_FILE_UPLOAD { get; set; }
        public string FILE_NAME { get; set; }
        private FileUpload _File;
        public FileUpload File
        {
            get
            {
                if (_File == null)
                {
                    _File = new FileUpload();
                }
                return _File;
            }
            set
            {
                _File = value;
            }
        }
        public List<FileUpload> Files { get; set; }

        private bool _IsCopy = false;
        public bool IsCopy
        {
            get { return _IsCopy; }
            set { _IsCopy = value; }
        }

        private bool _IsDeleteBeforeSave = true;
        public bool IsDeleteBeforeSave
        {
            get { return _IsDeleteBeforeSave; }
            set { _IsDeleteBeforeSave = value; }
        }
        public decimal? OLD_DOCUMENT_TYPE_ID { get; set; }
        public decimal? OLD_SECTION_GROUP_ID { get; set; }
        public decimal? OLD_COVER_SHEET_SEND_ID { get; set; }
        public decimal? OLD_HEADER_INPUT_ID { get; set; }
        public decimal? OLD_DETAIL_INPUT_ID { get; set; }
    }
}
