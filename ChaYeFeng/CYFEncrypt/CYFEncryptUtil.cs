using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ChaYeFeng
{
    public static class CYFEncryptUtil
    {
        private static readonly string EncryptKey;

        static CYFEncryptUtil(string key)
        {
            EncryptKey = key;
        }

        public static ICryptoTransform GetCryptTranform(CipherMode mode, PaddingMode paddingMode)
        {
            byte[] encryptKey = UTF8Encoding.UTF8.GetBytes(EncryptKey);
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = encryptKey;
            rijndael.Mode = mode;
            rijndael.Padding = paddingMode;
            return rijndael.CreateEncryptor();
        }

        #region MD5加密
        public static string Md532(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Md532()的调用对象未空");
            }
            Encoding encoding = Encoding.UTF8;
            MD5 md5 = MD5.Create();
            return HashAlgorithmBase(md5, value, encoding);
        }
        #endregion
        /// <summary>
        /// 将byte[]转换成字符串（16进制大写，域宽两位）
        /// </summary>
        /// <param name="source"></param>
        /// <param name="formatStr"></param>
        /// <returns></returns>
        private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            StringBuilder pwd = new StringBuilder();
            foreach (byte btStr in source)
            {
                pwd.AppendFormat(formatStr, btStr);
            }
            return pwd.ToString();
        }

        /// <summary>
        /// 转换成byte[]
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static byte[] Str2Bytes(this string source)
        {
            source = source.Replace(" ", "");
            byte[] buffer = new byte[source.Length / 2];
            for (int i = 0; i < source.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
            }
            return buffer;
        }

        private static byte[] FormatByte(this string strVal, Encoding encoding)
        { 
            return encoding.GetBytes(strVal
        }

        /// <summary>
        /// 统一调用加密方法
        /// </summary>
        /// <param name="hab">算法类型</param>
        /// <param name="value">原始字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        private static string HashAlgorithmBase(HashAlgorithm hab, string value, Encoding encoding)
        {
            byte[] btStr = encoding.GetBytes(value);
            byte[] hashStr = hab.ComputeHash(btStr);
            return hashStr.Bytes2Str();
        }
    }
}
