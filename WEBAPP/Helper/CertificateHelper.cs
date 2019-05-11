using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UtilityLib;

namespace WEBAPP.Helper
{
    public class CertificateHelper
    {
        internal class TOTCheckResult
        {
            public string FieldName { get; set; }
            public string FieldText { get; set; }
            public int RowIndex { get; set; }
            public string SN { get; set; }

        }


        public bool VerifyFile(byte[] data, string signature, byte[] certPath)
        {

            bool Verify = false;

            try
            {

                // Load the certificate we'll use to verify the signature from a file 
                X509Certificate2 uidCert = new X509Certificate2(certPath);
                // Note: 
                // If we want to use the client cert in an ASP.NET app, we may use something like this instead:
                // X509Certificate2 cert = new X509Certificate2(Request.ClientCertificate.Certificate);

                // Get its associated CSP and public key
                RSACryptoServiceProvider csp = (RSACryptoServiceProvider)uidCert.PublicKey.Key;
                if (csp != null)
                {
                    byte[] bsignature = StringToByteArray(signature);
                    // csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA256"), bsignature);

                    if (uidCert.SignatureAlgorithm.FriendlyName.ToLower() == "sha1rsa")
                    {
                        var sha = new SHA1Managed();
                        byte[] hash = sha.ComputeHash(data);
                        // Sign the hash
                        Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);

                    }
                    //Fix SHA512RSA with SHA1
                    else if ((uidCert.PublicKey.Key).SignatureAlgorithm.ToString().Split('#')[1] == "rsa-sha1")
                    {
                        var sha = new SHA1Managed();
                        byte[] hash = sha.ComputeHash(data);
                        // Sign the hash
                        string alg = CryptoConfig.MapNameToOID("SHA1");
                        Verify = csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), bsignature);
                    }
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


        private byte[] ToByteArray(Stream stream)
        {
            byte[] result = new byte[stream.Length];
            using (stream)
            {
                using (MemoryStream memStream = new MemoryStream())
                {

                    stream.CopyTo(memStream);
                    result = memStream.ToArray();

                }
            }
            return result;
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

            var htmlContent = new HtmlDocument();
            htmlContent.LoadHtml(htmlString);

            var root = htmlContent.DocumentNode;
            var nodes = root.Descendants();
            var query = from table in root.SelectNodes("//table").Cast<HtmlNode>()
                        from row in table.SelectNodes("tr").Cast<HtmlNode>()
                            //from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
                        select new { Row = row, RowIndex = row.Line };

            if (query != null)
            {
                foreach (var node in query)
                {
                    if (node.Row.SelectNodes("td").Count == 2)
                    {
                        lsResult.Add(new TOTCheckResult
                        {
                            FieldName = node.Row.SelectNodes("td")[0].InnerText,
                            FieldText = node.Row.SelectNodes("td")[1].InnerText,
                            RowIndex = node.RowIndex
                        });
                    }

                }
            }

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
                Stream os = req.GetRequestStream();
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
            catch (WebException WebEx)
            {
                RestCert = "Error:" + WebEx.Message;
            }
            catch (Exception ex)
            {

                RestCert = "Error:" + ex.Message;
            }
            return RestCert;

        }
        public VerifyCertificateResult VerifyCertificate(byte[] rawData)
        {
            X509Certificate2 certificate = new X509Certificate2(rawData, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.UserKeySet);

            X509Chain chain = new X509Chain();
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain; // X509RevocationFlag.EntireChain
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online; //  X509RevocationMode.Offline;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 30);
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
            chain.Build(certificate);

            // List<Dictionary<string, string>> lisDic = new List<Dictionary<string, string>>();

            //Dictionary<string, object> dic = new Dictionary<string, object>();

            VerifyCertificateResult result = new VerifyCertificateResult();

            result.STATUS = null;
            result.REVOKED = false;
            result.REVOKED_MSG = "";
            result.UNTRUSTED = false;
            result.EXPIRY = certificate.NotAfter.Date;
            result.VERIFY = certificate.Verify();

            result.SERIAL_NO = certificate.SerialNumber;
            result.SIGN_ALGOR = certificate.SignatureAlgorithm.FriendlyName;
            if (certificate.SignatureAlgorithm.FriendlyName.ToLower() == "sha1rsa")
            {
                result.SIGN_HASH_ALGOR = "sha1";
            }
            else if (certificate.SignatureAlgorithm.FriendlyName.ToLower() == "sha256rsa")
            {  //SignatureAlgorithm SHA256RSA
                result.SIGN_HASH_ALGOR = "sha256";
            }
            else if (certificate.SignatureAlgorithm.FriendlyName.ToLower() == "sha512rsa")
            {  //SignatureAlgorithm SHA512RSA
                result.SIGN_HASH_ALGOR = "sha512";
            }
            ////Fix SHA512RSA with SHA1
            //else if ((certificate.PublicKey.Key).SignatureAlgorithm.ToString().Split('#')[1] == "rsa-sha1")
            //{
            //    result.SIGN_HASH_ALGOR = "sha1";
            //}
            result.ISSUER = certificate.Issuer;
            var jsonSubject = StringToList(certificate.Subject.AsString());
            var jsonSubject2 = StringToList(certificate.Issuer.AsString());
            if (jsonSubject.Count > 0)
            {
                result.ISSUED_TO = jsonSubject[0];
            }
            if (jsonSubject2.Count > 0)
            {
                result.ISSUED_BY = jsonSubject2[0];
            }
            result.VALID_FROM = certificate.NotBefore.Date;
            result.VALID_TO = certificate.NotAfter.Date;
            result.SUBJECT = certificate.Subject;
            result.PUBLIC_KEY = string.Format("{0}({1}bits)", certificate.GetRSAPublicKey().KeyExchangeAlgorithm, certificate.GetRSAPublicKey().KeySize);

            foreach (var item in certificate.Extensions)
            {
                //Authority Key Identifier 2.5.29.35
                //CRL Distribution Points 2.5.29.31
                if (item.Oid.Value.Equals("2.5.29.31") || item.Oid.Value.Equals("2.5.29.35"))
                {
                    AsnEncodedData asndata = new AsnEncodedData(item.Oid, item.RawData);
                    if (item.Oid.Value.Equals("2.5.29.31"))
                    {
                        result.CRL_DISTRIB_POINT = asndata.Format(true);
                    }
                    else if (item.Oid.Value.Equals("2.5.29.35"))
                    {
                        result.AUTHOR_KEY_IDENTIFIER = asndata.Format(true);
                    }
                }
            }
            string status = "";

            string[] status_arr = new string[chain.ChainStatus.Length];
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                status += string.Format("Certificate CRL Status: {0}{1}", chain.ChainStatus[i].Status, Environment.NewLine);
                //result.status_" + i, chain.ChainStatus[i].Status.ToString());
                status_arr[i] = chain.ChainStatus[i].Status.ToString();
                // lisDic.Add("stat"+i,"")
            }
            if (status != "")
            {
                result.STATUS = status_arr;
                if (status.ToUpper().Contains("UNTRUSTED"))
                {
                    result.UNTRUSTED = true;
                }
                if (status.ToUpper().Contains("REVOKED"))
                {
                    result.REVOKED = true;
                }
            }
            else
            {
                result.UNTRUSTED = false;
                result.REVOKED = false;
            }

            string TOTResult = this.CheckTOTValidCRL(certificate.SerialNumber);
            if (string.IsNullOrEmpty(TOTResult) == false)
            {

                result.REVOKED = true;
                result.REVOKED_MSG = TOTResult;
            }
            //dic["STATUS"] = status;

            return result;
        }
        private List<string> StringToList(string Data)
        {
            var result = new List<string>();

            string[] Datas = Data.Split(new string[] { ", " }, StringSplitOptions.None);
            if (Datas.Length > 0)
            {
                result.AddRange(Datas);
            }
            return result;
        }
        private string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }

    public class VerifyCertificateResult
    {
        public string[] STATUS { get; set; }
        public bool REVOKED { get; set; }
        public string REVOKED_MSG { get; set; }
        public bool UNTRUSTED { get; set; }
        public DateTime EXPIRY { get; set; }
        public bool VERIFY { get; set; }
        public string SUBJECT_NAME { get; set; }
        public string ISSUERNAME { get; set; }
        public string CERT_NUMBER { get; set; }

        public string SERIAL_NO { get; set; }
        public string SIGN_ALGOR { get; set; }
        public string SIGN_HASH_ALGOR { get; set; }
        public string ISSUER { get; set; }
        public DateTime VALID_FROM { get; set; }
        public DateTime VALID_TO { get; set; }
        public string SUBJECT { get; set; }
        public string PUBLIC_KEY { get; set; }
        public string AUTHOR_KEY_IDENTIFIER { get; set; }
        public string CRL_DISTRIB_POINT { get; set; }
        public string ISSUED_TO { get; set; }
        public string ISSUED_BY { get; set; }
    }
}