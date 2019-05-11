

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Ionic.Zip;
using System.Configuration;

namespace UtilityLib
{
    public static class CompressHelper
    {
        public static string BytesToString(this long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public static Dictionary<string, MemoryStream> ToVerifySined(this Stream signStream)
        {
            var result = new Dictionary<string, MemoryStream>();
            var pws = ConfigurationManager.AppSettings["PwsUnzip"];

            using (var zip = ZipFile.Read(signStream))
            {
                // zip.Password = "zaq1234";
                zip.Password = pws.IsNullOrEmpty() ? "vsm1234" : pws;
                foreach (var e in zip)
                {
                    var data = new MemoryStream();
                    e.Extract(data);
                    result.Add(e.FileName, data);
                }
            }

            return result;
        }

        //public static string BytesToString(long byteCount)
        //{
        //    string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        //    if (byteCount == 0)
        //        return "0" + suf[0];
        //    long bytes = Math.Abs(byteCount);
        //    int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        //    double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        //    return (Math.Sign(byteCount) * num).ToString() + suf[place];
        //}

        public static byte[] StreamtoByteArray(this Stream InputStream)
        {
            // Get the input value from the file/other source
            byte[] result;
            using (var streamReader = new MemoryStream())
            {
                InputStream.CopyTo(streamReader);
                result = streamReader.ToArray();
            }

            return result;
        }

        public static string ByteArrayToString(this byte[] bytes)
        {
            System.Text.StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();

        }

        public static byte[] StringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }


        //internal static void ToZipSigned(string file, string signature)
        //{
        //    FileInfo fi = new FileInfo(file);
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.Password = "zaq1234";
        //        zip.Encryption = EncryptionAlgorithm.WinZipAes256;
        //        zip.AddFile(fi.FullName, ""); //ตั้งค่าไฟล์แรก
        //        zip.AddEntry("Signed.txt", signature);
        //        //  zip.Save("Backup-AES-Encrypted.crypto");
        //        zip.Save(string.Format("{0}.signed", fi.Name));

        //    }
        //}

        //internal static void ToZipSigned(string filePath, string signature, string selectedPath)
        //{
        //    FileInfo fi = new FileInfo(filePath);
        //    using (ZipFile zip = new ZipFile())
        //    {
        //        {
        //            zip.Password = "zaq1234";
        //            zip.Encryption = EncryptionAlgorithm.WinZipAes256;
        //            zip.AddFile(fi.FullName, ""); //ตั้งค่าไฟล์แรก
        //            zip.AddEntry("Signed.txt", signature);
        //            //  zip.Save("Backup-AES-Encrypted.crypto");
        //            var savePath = Path.Combine(selectedPath, string.Format("{0}.signed", fi.Name));
        //            zip.Save(savePath);

        //        }
        //    }
        //}







    }

    public enum CertType
    {
        PrivateKey,
        PublicKey,
        TokenKey
    }

    public enum PKIErrorType
    {
        Certificate = 0,
        DataFile = 1
    }

    public class PKIResult
    {
        public string CerNumber { get; set; }
        public bool Success { get; set; }
        public PKIErrorType ErrorType { get; set; }
        public string ErrorMessage { get; set; }

        //เดี๋ยวลบออก ถ้าแก้เสร็จหมดแล้ว
        private Dictionary<string, object> _FileData;
        public Dictionary<string, object> FileData
        {
            get
            {
                if (_FileData == null)
                {
                    _FileData = new Dictionary<string, object>();
                }
                return _FileData;
            }
            set
            {
                _FileData = value;
            }
        }

    }
    public class FileData
    {
        public decimal? ID { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
        public byte[] DataBytes { get; set; }
        public string DataHash { get; set; }
        public string Signature { get; set; }
        public string CertNumber { get; set; }
        public bool Success { get; set; }
        public string ErrorMSG { get; set; }

    }

    public class FileUpload
    {
        public string PRG_CODE { get; set; }
        public decimal? DOCUMENT_TYPE_ID { get; set; }
        public decimal? SECTION_GROUP_ID { get; set; }
        public decimal? COVER_SHEET_SEND_ID { get; set; }
        public decimal? HEADER_INPUT_ID { get; set; }
        public decimal? DETAIL_INPUT_ID { get; set; }

        public string COM_CODE { get; set; }
        public System.Web.HttpPostedFileBase File { get; set; }
        public decimal? ROW_ID { get; set; }
        public decimal? ID { get; set; }
        public string FILE_TYPE { get; set; }
        public string FILE_NAME { get; set; }
        public decimal? FILE_SIZE { get; set; }
        public DateTime? FILE_DATE { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public byte[] BLOB_FILE { get; set; }
        public string BLOB_FILE_HASH { get; set; }
        public string CERTIFICATE_NUMBER { get; set; }
        public string SIGNATURE_SIGN { get; set; }

        public string DOCUMENT_TYPE_NAME_TH { get; set; }
        public string SECTION_GROUP_NAME_TH { get; set; }

        public bool Success { get; set; }
        public string ErrorMSG { get; set; }
    }

    public class SystemConfigFile
    {
        public string COM_CODE { get; set; }
        public int? NO_OF_FILE { get; set; }
        public int? FILE_SIZE { get; set; }
        public List<SystemConfigFileDT> Details { get; set; }
    }

    public class SystemConfigFileDT
    {
        public string PRG_CODE { get; set; }
        public string FILE_TYPE { get; set; }
        public int? FILE_SIZE { get; set; }
    }
    public class PKIHelper
    {
        internal class TOTCheckResult
        {
            public string FieldName { get; set; }
            public string FieldText { get; set; }
            public int RowIndex { get; set; }
            public string SN { get; set; }

        }

        private decimal ret = 1048576;
        public CertType CertType { get; set; }
        private byte[] RawCert { get; set; }
        public byte[] RawDataDocument { get; set; }
        public string Password { get; set; }
        public string PRG_CODE { get; set; }
        public SystemConfigFile ConfigFile { get; set; }
        private Dictionary<string, object> _FileData;
        public Dictionary<string, object> FileData
        {
            get
            {
                if (_FileData == null)
                {
                    _FileData = new Dictionary<string, object>();
                }
                return _FileData;
            }

            set
            {
                _FileData = value;
            }
        }

        private List<SystemConfigFileDT> FileConfigDT
        {
            get
            {
                var confDT = ConfigFile.Details;
                if (!PRG_CODE.IsNullOrEmpty())
                {
                    var confByPrg = confDT.Where(m => m.PRG_CODE == PRG_CODE).ToList();
                    if (confByPrg != null && confByPrg.Count > 0)
                    {
                        confDT = confByPrg;
                    }
                }
                return confDT;
            }
        }

        private PKIResult _pkiResul;
        public PKIHelper(CertType type, byte[] rawCert, string password = "")
        {
            this.RawCert = rawCert;
            this.Password = password;
            this.CertType = type;
            _pkiResul = new PKIResult();
        }
        public PKIResult Verify(byte[] dataBytes, byte[] signatureBytes)
        {
            var result = new PKIResult();
            try
            {
                var signature = Encoding.UTF8.GetString(signatureBytes);


                // Load the certificate we'll use to verify the signature from a file 
                X509Certificate2 uidCert = new X509Certificate2(RawCert);
                // Note: 
                // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
                // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);

                // Get its associated CSP and public key
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)uidCert.PublicKey.Key;
                if (csp != null)
                {
                    byte[] bsignature = signature.StringToByteArray();
                    // csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), bsignature);

                    if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha1rsa")
                    {
                        var sha = new SHA1Managed();
                        byte[] hash = sha.ComputeHash(dataBytes);
                        // Sign the hash
                        result.Success = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);

                    }
                    //Fix SHA512RSA with SHA1
                    else if ((uidCert.PublicKey.Key).SignatureAlgorithm.ToString().Split('#')[1] == "rsa-sha1")
                    {
                        var sha = new SHA1Managed();
                        byte[] hash = sha.ComputeHash(dataBytes);
                        // Sign the hash
                        string alg = CryptoConfig.MapNameToOID("SHA1");
                        result.Success = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);
                    }
                    else if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha256rsa")
                    {  //SignatureAlgorithm SHA256RSA

                        var sha = new SHA256Managed();
                        byte[] hash = sha.ComputeHash(dataBytes);
                        // Sign the hash
                        result.Success = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), bsignature);
                    }
                    else if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha512rsa")
                    {  //SignatureAlgorithm SHA512RSA

                        var sha = new SHA512Managed();
                        byte[] hash = sha.ComputeHash(dataBytes);
                        result.Success = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA512"), bsignature);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return result;
        }
        public PKIResult Verify()
        {
            PKIResult result = new PKIResult();
            result.Success = true;
            try
            {
                if (CertType == CertType.PublicKey)
                {
                    if (this.RawCert != null)
                    {
                        var json = this.VerifyCertificate();
                        Dictionary<string, object> jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        if ((DateTime)jsonObj["EXPIRY"] <= DateTime.Now.Date)
                        {
                            result.ErrorType = PKIErrorType.Certificate;
                            result.ErrorMessage = " : ใบรับรองหมดอายุ. ";
                            result.Success = false;
                        }
                        else if ((Boolean)jsonObj["REVOKED"])
                        {
                            result.ErrorType = PKIErrorType.Certificate;
                            result.ErrorMessage = " : ใบรับรองถูกยกเลิก. ";
                            result.Success = false;
                        }
                        else if ((Boolean)jsonObj["UNTRUSTED"])
                        {
                            result.ErrorType = PKIErrorType.Certificate;
                            result.ErrorMessage = " : ใบรับรองความปลอดภัยไม่น่าเชื่อถือ. ";
                            result.Success = false;
                        }
                        else
                        {
                            foreach (var file in FileData)
                            {
                                if (file.Value.GetType() == typeof(FileUpload))
                                {
                                    var dataFile = (FileUpload)file.Value;
                                    if (dataFile.BLOB_FILE != null && !dataFile.SIGNATURE_SIGN.IsNullOrEmpty())
                                    {
                                        dataFile.Success = this.VerifyFile(dataFile.BLOB_FILE, dataFile.SIGNATURE_SIGN.Trim());
                                        if (!dataFile.Success)
                                        {
                                            dataFile.ErrorMSG = "ใบรับรองหรือข้อมูลไม่ถูกต้อง";
                                            result.ErrorType = PKIErrorType.DataFile;
                                            result.Success = false;
                                            result.ErrorMessage = "ใบรับรองหรือข้อมูลไม่ถูกต้อง";
                                        }
                                    }
                                    else
                                    {
                                        dataFile.ErrorMSG = "ไม่มีไฟล์";
                                        result.ErrorType = PKIErrorType.DataFile;
                                        result.Success = false;
                                        result.ErrorMessage = "ไม่มีไฟล์";
                                    }
                                    result.FileData.Add(file.Key, dataFile);
                                }
                                else if (file.Value.GetType() == typeof(List<FileUpload>))
                                {
                                    var dataFile = (List<FileUpload>)file.Value;
                                    foreach (var item in dataFile)
                                    {
                                        if (item.BLOB_FILE != null && !item.SIGNATURE_SIGN.IsNullOrEmpty())
                                        {
                                            item.Success = this.VerifyFile(item.BLOB_FILE, item.SIGNATURE_SIGN.Trim());
                                            if (!item.Success)
                                            {
                                                item.ErrorMSG = item.FILE_NAME + " : ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                                                result.ErrorType = PKIErrorType.DataFile;
                                                result.Success = false;
                                                result.ErrorMessage = "ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                                            }
                                        }
                                        else
                                        {
                                            item.ErrorMSG = item.FILE_NAME + " : ไม่มีไฟล์";
                                            result.ErrorType = PKIErrorType.DataFile;
                                            result.Success = false;
                                            result.ErrorMessage = "ไม่มีไฟล์";
                                        }
                                    }
                                    result.FileData.Add(file.Key, dataFile);
                                }
                            }
                        }
                    }
                    else
                    {
                        result.ErrorMessage = "ไม่มีใบรับรอง";
                        result.Success = false;
                    }
                }
                else if (CertType == CertType.TokenKey)
                {
                    foreach (var file in FileData)
                    {
                        if (file.Value.GetType() == typeof(FileUpload))
                        {
                            var dataFile = (FileUpload)file.Value;
                            if (dataFile.File != null)
                            {
                                dataFile.FILE_NAME = dataFile.File.FileName;
                                dataFile.FILE_SIZE = ((decimal)dataFile.File.ContentLength) / ret;
                                dataFile.BLOB_FILE = dataFile.File.ToArrayByte();
                                dataFile.BLOB_FILE_HASH = GetFileHash(dataFile.BLOB_FILE);
                                //fileData.Signature = dataFile.Signature;
                                //fileData.CertNumber = jsonObj["CERT_NUMBER"].AsString();
                                dataFile.Success = true;
                                var ext = Path.GetExtension(dataFile.FILE_NAME);
                                var config = FileConfigDT.Where(m => m.FILE_TYPE.Replace(".", "") == ext.Replace(".", "")).FirstOrDefault();
                                if (config == null)
                                {
                                    dataFile.Success = false;
                                    dataFile.ErrorMSG = "ไฟล์ไม่ถูกประเภท";
                                    result.ErrorType = PKIErrorType.DataFile;
                                    result.Success = false;
                                }
                                else if (dataFile.FILE_SIZE > config.FILE_SIZE)
                                {
                                    dataFile.Success = false;
                                    dataFile.ErrorMSG = "ขนาดไฟล์เกิน " + config.FILE_SIZE + "MB";
                                    result.ErrorType = PKIErrorType.DataFile;
                                    result.Success = false;
                                }
                            }

                            //if (fileData.Success)
                            //{
                            //    fileData.Success = this.VerifyFile(fileData.DataBytes, fileData.Signature.Trim());
                            //    if (!fileData.Success)
                            //    {
                            //        fileData.ErrorMSG = " : ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                            //        result.ErrorType = PKIErrorType.DataFile;
                            //        result.Success = false;
                            //        result.ErrorMessage = " : ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                            //    }
                            //}

                        }
                        else if (file.Value.GetType() == typeof(List<FileUpload>))
                        {
                            var dataFile = (List<FileUpload>)file.Value;
                            foreach (var item in dataFile.Where(m => m.File != null))
                            {
                                item.FILE_NAME = item.File.FileName;
                                item.FILE_SIZE = ((decimal)item.File.ContentLength) / ret;
                                item.BLOB_FILE = item.File.ToArrayByte();
                                item.BLOB_FILE_HASH = GetFileHash(item.BLOB_FILE);
                                //fileData.Signature = item.Signature;
                                //fileData.CertNumber = jsonObj["CERT_NUMBER"].AsString();
                                item.Success = true;
                                var ext = Path.GetExtension(item.FILE_NAME);
                                var config = FileConfigDT.Where(m => m.FILE_TYPE == ext.Replace(".", "")).FirstOrDefault();
                                if (config == null)
                                {
                                    item.Success = false;
                                    item.ErrorMSG = "ไฟล์ไม่ถูกประเภท";
                                }
                                else if (item.FILE_SIZE > config.FILE_SIZE)
                                {
                                    item.Success = false;
                                    item.ErrorMSG = "ขนาดไฟล์เกิน " + config.FILE_SIZE + "MB";
                                }

                                //if (item.Success)
                                //{
                                //    item.Success = this.VerifyFile(item.DATA_BYTES, item.Signature.Trim());
                                //    if (!item.Success)
                                //    {
                                //        item.ErrorMSG = " : ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                                //        result.ErrorType = PKIErrorType.DataFile;
                                //        result.Success = false;
                                //        result.ErrorMessage = " : ใบรับรอง หรือ ข้อมูล ไม่ถูกต้อง.";
                                //    }
                                //}

                            }
                            result.FileData.Add(file.Key, dataFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return result;
        }
        private bool VerifyFile(byte[] data, string signature)
        {

            bool Verify = false;

            try
            {

                // Load the certificate we'll use to verify the signature from a file 
                X509Certificate2 uidCert = new X509Certificate2(this.RawCert);
                // Note: 
                // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
                // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);

                // Get its associated CSP and public key
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)uidCert.PublicKey.Key;
                if (csp != null)
                {
                    byte[] bsignature = signature.StringToByteArray();
                    // csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), bsignature);

                    if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha1rsa")
                    {
                        var sha = new SHA1Managed();
                        byte[] hash = sha.ComputeHash(data);
                        // Sign the hash
                        Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);

                    }
                    //Fix SHA512RSA with SHA1
                    //else if ((uidCert.PublicKey.Key).SignatureAlgorithm.ToString().Split('#')[1] == "rsa-sha1")
                    //{
                    //    var sha = new SHA1Managed();
                    //    byte[] hash = sha.ComputeHash(data);
                    //    // Sign the hash
                    //    string alg = CryptoConfig.MapNameToOID("SHA1");
                    //    Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);
                    //}
                    else if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha256rsa")
                    {  //SignatureAlgorithm SHA256RSA

                        var sha = new SHA256Managed();
                        byte[] hash = sha.ComputeHash(data);
                        // Sign the hash
                        Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), bsignature);
                    }
                    else if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha512rsa")
                    {  //SignatureAlgorithm SHA512RSA

                        var sha = new SHA512Managed();
                        byte[] hash = sha.ComputeHash(data);
                        Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA512"), bsignature);
                    }
                }
            }
            catch (Exception exx)
            {
                throw exx;

            }

            return Verify;
        }
        private string VerifyCertificate()
        {
            X509Certificate2 certificate = new X509Certificate2(this.RawCert, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserKeySet);

            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain; // X509RevocationFlag.EntireChain
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online; //  X509RevocationMode.Offline;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 30);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
            chain.Build(certificate);

            // List<Dictionary<string, string>> lisDic = new List<Dictionary<string, string>>();

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("STATUS", null);
            dic.Add("REVOKED", false);
            dic.Add("REVOKED_MSG", "");
            dic.Add("UNTRUSTED", false);
            dic.Add("EXPIRY", certificate.NotAfter.Date);
            dic.Add("VERIFY", certificate.Verify());

            dic.Add("SUBJECT_NAME", certificate.SubjectName.Name);
            dic.Add("ISSUERNAME", certificate.IssuerName.Name);
            dic.Add("VALID_FROM", certificate.NotBefore.Date);
            dic.Add("VALID_TO", certificate.NotAfter.Date);
            dic.Add("CERT_NUMBER", certificate.SerialNumber);
            //  PrintOutput(string.Format("Chain revocation flag: {0}{1}", chain.ChainPolicy.RevocationFlag, Environment.NewLine));
            //  PrintOutput(string.Format("Chain revocation mode: {0}{1}", chain.ChainPolicy.RevocationMode, Environment.NewLine));
            //  PrintOutput(string.Format("Chain verification flag: {0}{1}", chain.ChainPolicy.VerificationFlags, Environment.NewLine));
            //  PrintOutput(string.Format("Chain verification time: {0}{1}", chain.ChainPolicy.VerificationTime, Environment.NewLine));
            //  PrintOutput(string.Format("Chain status length: {0}{1}", chain.ChainStatus.Length, Environment.NewLine));
            //  PrintOutput(string.Format("Chain application policy count: {0}{1}", chain.ChainPolicy.ApplicationPolicy.Count, Environment.NewLine));
            //  PrintOutput(string.Format("Chain certificate policy count: {0} {1}", chain.ChainPolicy.CertificatePolicy.Count, Environment.NewLine));

            string status = "";
            string[] status_arr = new string[chain.ChainStatus.Length];
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                status += string.Format("Certificate CRL Status: {0}{1}", chain.ChainStatus[i].Status, Environment.NewLine);
                //dic.Add("status_" + i, chain.ChainStatus[i].Status.ToString());
                status_arr[i] = chain.ChainStatus[i].Status.ToString();
                // lisDic.Add("stat"+i,"")
            }
            if (status != "")
            {
                dic["STATUS"] = status_arr;
                if (status.ToUpper().Contains("UNTRUSTED"))
                {
                    dic["UNTRUSTED"] = true;
                }
                if (status.ToUpper().Contains("REVOKED"))
                {
                    dic["REVOKED"] = true;
                }
            }
            else
            {
                dic["UNTRUSTED"] = false;
                dic["REVOKED"] = false;
            }

            string TOTResult = this.CheckTOTValidCRL(certificate.SerialNumber);
            if (string.IsNullOrEmpty(TOTResult) == false)
            {

                dic["REVOKED"] = true;
                dic["REVOKED_MSG"] = TOTResult;
            }

            //dic["STATUS"] = status;

            return Newtonsoft.Json.JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// return ValidCRL true or false
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public string CheckTOTValidCRL(string serial)
        {

            string htmlResult = this.CheckCRL(serial);
            string TOTResult = string.Empty;
            try
            {
                if (htmlResult.Contains("Error") == false)
                {
                    List<TOTCheckResult> lsResult = this.parseHTML(htmlResult);
                    if (lsResult.Count > 0)
                    {
                        var Q1 = lsResult.Where(q => q.FieldName.Contains("สถานะของใบรับรอง")).SingleOrDefault();
                        var Q2 = lsResult.Where(q => q.FieldName.Contains("วันที่ใบรับรองถูกยกเลิก")).SingleOrDefault();
                        if (Q1.FieldText.Equals("Valid") == false || Q1.FieldText.Equals("Revoked"))
                        {
                            throw new Exception("ใบรับรองถูกยกเลิก วันที่ถูกยกเลิก :" + Q2.FieldText);
                        }
                    }

                }
                else
                {

                    //TODO LOGIC By Jubpas
                }
            }
            catch (Exception ex)
            {
                TOTResult = ex.Message;
            }

            return TOTResult;
        }

        private List<TOTCheckResult> parseHTML(string htmlString)
        {
            List<TOTCheckResult> lsResult = new List<TOTCheckResult>();

            // ไม่มี Libary ติดไว้ก่อน
            //var htmlContent = new  HtmlDocument();
            //htmlContent.LoadHtml(htmlString);

            //var root = htmlContent.DocumentNode;
            //var nodes = root.Descendants();
            //var query = from table in root.SelectNodes("//table").Cast<HtmlNode>()
            //            from row in table.SelectNodes("tr").Cast<HtmlNode>()
            //                //from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
            //            select new { Row = row, RowIndex = row.Line };

            //if (query != null)
            //{
            //    foreach (var node in query)
            //    {
            //        if (node.Row.SelectNodes("td").Count == 2)
            //        {
            //            lsResult.Add(new TOTCheckResult
            //            {
            //                FieldName = node.Row.SelectNodes("td")[0].InnerText,
            //                FieldText = node.Row.SelectNodes("td")[1].InnerText,
            //                RowIndex = node.RowIndex
            //            });
            //        }

            //    }
            //}

            return lsResult;

        }
        private string CheckCRL(string serial)
        {
            string RestCert = string.Empty;
            string Service_URL = "http://app.ca.tot.co.th/c_service/verify/product_status_checking_result.jsp";

            //ForTEST = 6737
            var Parameters = "issuer=null&searchType=serial&sn=" + serial;

            System.Net.WebRequest req = System.Net.WebRequest.Create(Service_URL);
            req.ContentType = "application/x-www-form-urlencoded";

            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;

            try
            {
                System.IO.Stream os = req.GetRequestStream();
                os.Write(bytes, 0, bytes.Length); //Push it out there
                os.Close();

                System.Net.WebResponse resp = req.GetResponse();

                if (resp != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), Encoding.Default);
                    Encoding end = sr.CurrentEncoding;
                    RestCert = sr.ReadToEnd().Trim();
                    //if (!string.IsNullOrEmpty(RestCert))
                    //{
                    //    parseHTML(RestCert);
                    //}
                }

            }
            catch (Exception ex)
            {

                RestCert = "Error:" + ex.Message;
            }
            return RestCert;

        }

        private string GetFileHash(byte[] bfile)
        {
            var sha = new SHA1Managed();
            byte[] hashbfile = sha.ComputeHash(bfile);
            return Convert.ToBase64String(hashbfile);
        }
    }


}
