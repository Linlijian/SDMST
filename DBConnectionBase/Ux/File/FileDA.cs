using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.Ux
{
    public class FileDA : BaseDA
    {
        public FileDTO DTO
        {
            get;
            set;
        }

        public FileDA()
        {
            DTO = new FileDTO();
        }

        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (FileDTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case FileExecuteType.GetBlobList: return GetBlobList(dto);
                case FileExecuteType.GetBlobFileList: return GetBlobFileList(dto);
                case FileExecuteType.GetBlobFileByID: return GetBlobFileByID(dto);
                case FileExecuteType.GetElectronicSign: return GetElectronicSign(dto);
            }
            return dto;
        }

        private FileDTO GetBlobFileList(FileDTO dto)
        {
            var parameters = CreateParameterWithRefCursor("RecordSet");
            parameters.AddParameter("pAPP_CODE", dto.ExecuteFile.COM_CODE);
            parameters.AddParameter("pPRG_CODE", dto.ExecuteFile.PRG_CODE);
            parameters.AddParameter("pDOCUMENT_TYPE_ID", dto.ExecuteFile.DOCUMENT_TYPE_ID);
            parameters.AddParameter("pSECTION_GROUP_ID", dto.ExecuteFile.SECTION_GROUP_ID);
            parameters.AddParameter("pCOVER_SHEET_SEND_ID", dto.ExecuteFile.COVER_SHEET_SEND_ID);
            parameters.AddParameter("pHEADER_INPUT_ID", dto.ExecuteFile.HEADER_INPUT_ID);
            parameters.AddParameter("pDETAIL_INPUT_ID", dto.ExecuteFile.DETAIL_INPUT_ID);
            parameters.AddParameter("pFILE_NAME", dto.ExecuteFile.FILE_NAME);
            parameters.AddParameter("pBLOB_LIST_ID", dto.ExecuteFile.ID);

            var result = _DBManger.ExecuteDataSet("PKG_OIC_COM_BLOB_CTRL.SP_QUERY_BLOB_FILE_LIST", parameters);
            if (result.Success(dto))
            {
                if (result.OutputData != null && result.OutputData.Tables.Count > 0)
                {
                    dto.ExecuteFile.Files = result.OutputData.Tables[0].ToList<FileUpload>();
                }
            }
            return dto;
        }

        private FileDTO GetBlobList(FileDTO dto)
        {
            var parameters = CreateParameterWithRefCursor("RecordSet");
            parameters.AddParameter("pAPP_CODE", dto.ExecuteFile.COM_CODE);
            parameters.AddParameter("pPRG_CODE", dto.ExecuteFile.PRG_CODE);
            parameters.AddParameter("pDOCUMENT_TYPE_ID", dto.ExecuteFile.DOCUMENT_TYPE_ID);
            parameters.AddParameter("pSECTION_GROUP_ID", dto.ExecuteFile.SECTION_GROUP_ID);
            parameters.AddParameter("pCOVER_SHEET_SEND_ID", dto.ExecuteFile.COVER_SHEET_SEND_ID);
            parameters.AddParameter("pHEADER_INPUT_ID", dto.ExecuteFile.HEADER_INPUT_ID);
            parameters.AddParameter("pDETAIL_INPUT_ID", dto.ExecuteFile.DETAIL_INPUT_ID);
            parameters.AddParameter("pFILE_NAME", dto.ExecuteFile.FILE_NAME);
            parameters.AddParameter("pBLOB_LIST_ID", dto.ExecuteFile.ID);

            var result = _DBManger.ExecuteDataSet("PKG_OIC_COM_BLOB_CTRL.SP_QUERY_BLOB_LIST", parameters);
            if (result.Success(dto))
            {
                if (result.OutputData != null && result.OutputData.Tables.Count > 0)
                {
                    dto.ExecuteFile.Files = result.OutputData.Tables[0].ToList<FileUpload>();
                }
            }
            return dto;
        }
        private BaseDTO GetBlobFileByID(BaseDTO dto)
        {
            var parameters = CreateParameterWithRefCursor("RecordSet");
            parameters.AddParameter("pAPP_CODE", dto.ExecuteFile.COM_CODE);
            parameters.AddParameter("pBLOB_LIST_ID", dto.ExecuteFile.ID);

            var result = _DBManger.ExecuteDataSet("PKG_OIC_COM_BLOB_CTRL.SP_QUERY_BLOB_FILE", parameters);
            if (result.Success(dto))
            {
                if (result.OutputData != null && result.OutputData.Tables.Count > 0)
                {
                    dto.ExecuteFile.File = result.OutputData.Tables[0].ToObject<FileUpload>();
                }
            }
            return dto;
        }

        private FileDTO GetElectronicSign(FileDTO dto)
        {
            var parameters = CreateParameterWithRefCursor("RecordSet");
            parameters.AddParameter("pAPP_CODE", dto.ExecuteFile.COM_CODE);
            parameters.AddParameter("pPRG_CODE", dto.ExecuteFile.PRG_CODE);
            parameters.AddParameter("pDOCUMENT_TYPE_ID", dto.ExecuteFile.DOCUMENT_TYPE_ID);
            parameters.AddParameter("pSECTION_GROUP_ID", dto.ExecuteFile.SECTION_GROUP_ID);
            parameters.AddParameter("pCOVER_SHEET_SEND_ID", dto.ExecuteFile.COVER_SHEET_SEND_ID);
            parameters.AddParameter("pHEADER_INPUT_ID", dto.ExecuteFile.HEADER_INPUT_ID);
            parameters.AddParameter("pDETAIL_INPUT_ID", dto.ExecuteFile.DETAIL_INPUT_ID);
            parameters.AddParameter("pFILE_NAME", dto.ExecuteFile.FILE_NAME);
            parameters.AddParameter("pBLOB_LIST_ID", dto.ExecuteFile.ID);

            var result = _DBManger.ExecuteDataSet("PKG_OIC_COM_BLOB_CTRL.SP_QUERY_BLOB_FILE_LIST_H", parameters);
            if (result.Success(dto))
            {
                if (result.OutputData != null)
                {
                    dto.ESignModel = result.OutputData.Tables[0].ToObject<ElectronicSignModel>();
                }
            }
            return dto;
        }
    }
}
