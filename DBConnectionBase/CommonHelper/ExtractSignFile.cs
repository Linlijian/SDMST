using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UtilityLib;

namespace DataAccess
{
    public static class ExtractSignFile
    {
        public static List<ExtractSignFileModel> GetFileDecrypt(string Type, System.Web.HttpPostedFileBase DataUpload, byte[] certBytes, string AttFileType, List<FILE_ATTACH_CONFIG> configModel)//A=Attract,R=Reportfile
        {
            List<ExtractSignFileModel> ListRII = new List<ExtractSignFileModel>();

            var dicEntry = new Dictionary<string, MemoryStream>();
            Stream unzippedEntryStream;

         
            ZipArchive archive = new ZipArchive(DataUpload.InputStream);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                dicEntry = null;

                #region :::::::::Extract File:::::::

                if (entry.FullName.EndsWith(".signed", StringComparison.Ordinal))
                {
                    unzippedEntryStream = entry.Open();

                    string sFileName = "";
                    string sSignature = "";
                    byte[] ByteSignFile = CompressHelper.StreamtoByteArray(unzippedEntryStream);

                    using (var stream = new MemoryStream(ByteSignFile))
                    {
                        dicEntry = CompressHelper.ToVerifySined(stream);

                        byte[] dataBuffer = new byte[] { };
                        byte[] signBuffer = new byte[] { };
                        //Step 2: Verify Data
                        if (dicEntry != null)
                        {
                            //var certBytes = File.ReadAllBytes(pubkey);  // มาจาก ที่เค้า Register ไว้ในระบบ

                            foreach (var dic in dicEntry)
                            {
                                if (dic.Key == "Signed.txt")
                                {
                                    signBuffer = dic.Value.ToArray();
                                    sSignature = Encoding.UTF8.GetString(signBuffer.ToArray());
                                    // signBuffer = CompressHelper.StreamtoByteArray(dic.Value);
                                }
                                else if (Type == "R" && dic.Key.EndsWith(".xlsx"))//report
                                {
                                    dataBuffer = dic.Value.ToArray();
                                    //dataBuffer = CompressHelper.StreamtoByteArray(dic.Value);

                                    sFileName = dic.Key;
                                }
                                else if (Type == "A")/// attach
                                {
                                    string strExtension = Path.GetExtension(dic.Key);

                                    //if (AttFileType.Contains(strExtension))
                                    //{
                                    decimal dm_result = configModel.Where(s => s.FILE_TYPE == strExtension)
                                        .Select(s => Convert.ToDecimal(s.FILE_SIZE)).Sum();

                                    if (!dm_result.IsNullOrEmpty() && dm_result > 0) //เช็ค size by file Extension
                                    {
                                        if (decimal.Divide(dataBuffer.Length, 1048576) <= dm_result)
                                        {
                                            dataBuffer = dic.Value.ToArray();
                                            sFileName = dic.Key;
                                        }
                                    }

                                    //}
                                }
                            }
                            if (dataBuffer.Length > 0)
                            {
                                var pki = new PKIHelper(CertType.PublicKey, certBytes);
                                //Output For Debug
                                PKIResult verPkiResult = pki.Verify(dataBuffer, signBuffer);

                                if (verPkiResult.Success)
                                {
                                    ExtractSignFileModel LIST_CONTRACT = new ExtractSignFileModel();

                                    LIST_CONTRACT.DOCUMENT_UPLOAD = DataUpload;
                                    LIST_CONTRACT.BLOB_FILE = dataBuffer;
                                    LIST_CONTRACT.FILE_SIZE = decimal.Divide(dataBuffer.Length, 1048576).ToString("N4");
                                    LIST_CONTRACT.FILE_NAME = sFileName;
                                    LIST_CONTRACT.DATA_TYPE = Type;
                                    LIST_CONTRACT.CERTIFICATE_NO = verPkiResult.CerNumber;
                                    LIST_CONTRACT.SIGNATURE_SIGN = sSignature;
                                    LIST_CONTRACT.BLOB_FILE_HASH = dataBuffer.GetHashCode().ToString();

                                    ListRII.Add(LIST_CONTRACT);
                                }
                            }
                        }
                    }
                }

                #endregion :::::::::Extract File:::::::
            }

            return ListRII;
        }
    }
}