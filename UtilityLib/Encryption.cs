using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UtilityLib
{
    public static class Encryption
    {
        private static string Key
        {
            get { return "ABCDEFGH"; }
        }

        // Create an md5 sum string of this string
        public static string GetMd5Sum(this string data)
        {
            // First we need to convert the string into bytes, which
            // means using a text encoder.
            var enc = Encoding.Unicode.GetEncoder();

            // Create a buffer large enough to hold the string
            var unicodeText = new byte[data.Length * 2];
            enc.GetBytes(data.ToCharArray(), 0, data.Length, unicodeText, 0, true);

            // Now that we have a byte array we can ask the CSP to hash it
            MD5 md5 = new MD5CryptoServiceProvider();
            var result = md5.ComputeHash(unicodeText);

            // Build the final string by converting each byte
            // into hex and appending it to a StringBuilder
            var sb = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
                sb.Append(i.ToString(Key));

            // And return it
            return sb.ToString();
        }

        public static byte[] Encrypt(this string data)
        {
            // Create a memory stream.
            var key = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(Key),
                IV = Encoding.ASCII.GetBytes(Key)
            };
            var ms = new MemoryStream();

            // Create a CryptoStream using the memory stream and the
            // CSP(cryptoserviceprovider) DES key.
            var crypstream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

            // Create a StreamWriter to write a string to the stream.
            var sw = new StreamWriter(crypstream);

            // Write the strText to the stream.
            sw.WriteLine(data);

            // Close the StreamWriter and CryptoStream.
            sw.Close();
            crypstream.Close();

            // Get an array of bytes that represents the memory stream.
            var buffer = ms.ToArray();

            // Close the memory stream.
            ms.Close();

            // Return the encrypted byte array.
            return buffer;
        }

        public static string Decrypt(this byte[] data)
        {
            // Create a memory stream to the passed buffer.
            var key = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(Key),
                IV = Encoding.ASCII.GetBytes(Key)
            };
            var ms = new MemoryStream(data);

            // Create a CryptoStream using  memory stream and CSP DES key.
            var crypstream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader for reading the stream.
            var sr = new StreamReader(crypstream);

            // Read the stream as a string.
            var val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            crypstream.Close();
            ms.Close();

            return val;
        }

        public static byte[] GetBytes(this string data)
        {
            var bytes = new byte[data.Length * sizeof(char)];
            System.Buffer.BlockCopy(data.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        public static string GetString(this byte[] data)
        {
            var chars = new char[data.Length / sizeof(char)];
            System.Buffer.BlockCopy(data, 0, chars, 0, data.Length);
            return new string(chars);
        }

        public static byte[] FromBase64String(this string data)
        {
            return Convert.FromBase64String(data);
        }

        public static string ToBase64String(this byte[] data)
        {
            return Convert.ToBase64String(data);
        }
        public static string ToAsciiString(this string data)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(data));
        }
        public static string ToAsciiBase64String(this string data)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(data));
        }
    }
}