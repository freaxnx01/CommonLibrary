using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Library
{
    public static class Md5Helper
    {
        #region GetMd5Sum
        public static string GetMd5Sum(string data)
        {
            Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

            byte[] text = new byte[data.Length * 2];
            enc.GetBytes(data.ToCharArray(), 0, data.Length, text, 0, true);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] checksum = md5.ComputeHash(text);

            //return GetMd5OutOfByteArray(result);
            return BitConverter.ToString(checksum).Replace("-", "").ToLower();
        }
        #endregion

        #region GetMd5SumOfFile
        public static string GetMd5SumOfFile(string file)
        {
            byte[] checksum = null;

            using (Stream stream = File.OpenRead(file))
            {
                using (MD5 md5 = MD5.Create())
                {
                    checksum = md5.ComputeHash(stream);
                }
            }

            return BitConverter.ToString(checksum).Replace("-", "").ToLower();
        }

        /*
        public static string GetMd5SumOfFile(string file)
        {
            using (Stream stream = File.OpenRead(file))
            {
                using (MD5 md5 = MD5.Create())
                {
                    return GetMd5OutOfByteArray(md5.ComputeHash(stream));
                }
            }
        }

        private static string GetMd5OutOfByteArray(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
        */
        #endregion
    }
}
