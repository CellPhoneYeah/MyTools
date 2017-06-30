using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;

namespace ChaYeFeng
{
    public static class CYFEncryptUtil
    {
        #region 私有方法
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

        /// <summary>
        /// 对字符串进行Base64加密取前16位后再转换大写，最后转换成制定编码的byte[]类型
        /// </summary>
        /// <param name="strVal"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static byte[] Format16Byte(this string strVal, Encoding encoding)
        {
            if (string.IsNullOrEmpty(strVal))
                throw new ArgumentNullException("FormatByte()不允许对空字符串操作");
            if (strVal.Length < 16)
                throw new Exception("FormatByte()不允许对长度小于16的字符串操作");
            string base64Str = strVal.Base64();
            string Str16 = base64Str.Substring(0, 16);
            string upStr = Str16.ToUpper();
            return encoding.GetBytes(upStr);
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
        #endregion

        #region AES string类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">明文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns>密文</returns>
        public static string AesStr(this string value, string keyVal, string ivVal)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("AES不允许对空字符串加密");
            byte[] bKey;
            byte[] bVector;
            Encoding encoding = Encoding.UTF8;
            bKey = keyVal.Format16Byte(encoding);//通过base64进行加密之后再使用,增加复杂度
            bVector = ivVal.Format16Byte(encoding);
            byte[] originalByte = encoding.GetBytes(value);
            string result = string.Empty;
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream
                    , aes.CreateEncryptor(bKey, bVector)
                    , CryptoStreamMode.Write))
                {
                    cStream.Write(originalByte, 0, originalByte.Length);
                    cStream.FlushFinalBlock();
                    result = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return result;
        }

        /// <summary>
        /// AES string类型 解密
        /// </summary>
        /// <param name="value">密文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns>明文</returns>
        public static string UnAesStr(this string value, string keyVal, string ivVal)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("AES不允许对空字符串加密");
            byte[] bKey;
            byte[] bVector;
            Encoding encoding = Encoding.UTF8;
            bKey = keyVal.Format16Byte(encoding);
            bVector = ivVal.Format16Byte(encoding);
            string result = string.Empty;
            byte[] originalByte = Convert.FromBase64String(value);
            Rijndael aes = Rijndael.Create();
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream
                    , aes.CreateDecryptor(bKey, bVector)
                    , CryptoStreamMode.Write))
                {
                    cStream.Write(originalByte, 0, originalByte.Length);
                    cStream.FlushFinalBlock();
                    result = encoding.GetString(mStream.ToArray());
                }
            }
            return result;
        }
        #endregion

        #region AES Byte类型
        /// <summary>
        /// AES Byte类型 加密
        /// </summary>
        /// <param name="data">明文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns>密文byte类型</returns>
        public static byte[] AesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length))
                , bKey
                , bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length))
                , bVector
                , bVector.Length);
            byte[] cryptOgraph;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream
                        , aes.CreateEncryptor(bKey, bVector)
                        , CryptoStreamMode.Write))
                    {
                        cStream.Write(data, 0, data.Length);
                        cStream.FlushFinalBlock();
                        cryptOgraph = mStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cryptOgraph;
        }

        /// <summary>
        /// AES Byte类型 解密
        /// </summary>
        /// <param name="data">密文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns>明文</returns>
        public static byte[] UnAesByte(this byte[] data, string keyVal, string ivVal)
        {
            byte[] bKey = new byte[32];
            byte[] bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(keyVal.Length))
                , bKey
                , bKey.Length);
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length))
                , bVector
                , bVector.Length);
            byte[] original;
            Rijndael aes = Rijndael.Create();
            try
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream
                        , aes.CreateDecryptor(bKey, bVector)
                        , CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[1024];
                        int readBytes = 0;
                        while ((readBytes = cStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            mStream.Write(buffer, 0, readBytes);
                        }
                        original = mStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return original;
        }
        #endregion

        #region BASE64加密解密
        /// <summary>
        /// BASE64加密 可逆
        /// </summary>
        /// <param name="value">待加密原文</param>
        /// <returns>密文</returns>
        public static string Base64Encrypt(string value)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="ciphervalue">密文</param>
        /// <returns>密文</returns>
        public static string Base64Decrypt(string ciphervalue)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(ciphervalue));
        }
        #endregion

        #region BASE64加密解密(string扩展方法)
        /// <summary>
        /// BASE64加密
        /// </summary>
        /// <param name="value">待加密字段</param>
        /// <returns>原文</returns>
        public static string Base64(this string value)
        {
            try
            {
                byte[] btArray = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(btArray, 0, btArray.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// BASE64解密
        /// </summary>
        /// <param name="value">待解密字段</param>
        /// <returns>原文</returns>
        public static string UnBase64(this string value)
        {
            try
            {
                byte[] btArray = Convert.FromBase64String(value);
                return Encoding.UTF8.GetString(btArray);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DES加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">原文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">算法向量</param>
        /// <returns></returns>
        public static string Des(this string value, string keyVal, string ivVal)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal);
                des.IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal);
                var encrypt = des.CreateEncryptor();
                byte[] result = encrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">密文</param>
        /// <param name="keyVal">秘钥</param>
        /// <param name="ivVal">算法向量</param>
        /// <returns></returns>
        public static string UnDes(this string value, string keyVal, string ivVal)
        {
            try
            {
                string[] sInput = value.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                des.Key = Encoding.UTF8.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal);
                des.IV = Encoding.UTF8.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal);
                var desencrypt = des.CreateDecryptor();
                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 不可逆的加密方法
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

        /// <summary>
        /// 加权MD5
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Md532(this string value, string salt)
        {
            return salt == null ? value.Md532() : (value + "『" + salt + "』").Md532();
        }
        #endregion

        #region SHA加密
        public static string Sha1(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行Sha1加密");
            Encoding encoding = Encoding.UTF8;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return HashAlgorithmBase(sha1, value, encoding);
        }
        #endregion

        #region SHA256加密
        public static string Sha526(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行Sha256加密");
            Encoding encoding = Encoding.UTF8;
            SHA256 sha256 = new SHA256Managed();
            return HashAlgorithmBase(sha256, value, encoding);
        }
        #endregion

        #region SHA512加密
        public static string Sha512(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行Sha512加密");
            Encoding encoding = Encoding.UTF8;
            SHA512 sha512 = new SHA512Managed();
            return HashAlgorithmBase(sha512, value, encoding);
        }
        #endregion

        #region HMAC加密
        public static string HmacSha1(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacSha1加密");
            Encoding encoding = Encoding.UTF8;
            HMACSHA1 hmac1 = new HMACSHA1(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmac1, value, encoding);
        }
        #endregion

        #region HmacSha256加密
        public static string HmacSha256(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacSha256加密");
            Encoding encoding = Encoding.UTF8;
            HMACSHA256 hmac256 = new HMACSHA256(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmac256, value, encoding);
        }
        #endregion

        #region HmacSha384加密
        public static string HmacSha384(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacSha384加密");
            Encoding encoding = Encoding.UTF8;
            HMACSHA384 hmac384 = new HMACSHA384(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmac384, value, encoding);
        }
        #endregion

        #region HmacSha512加密
        public static string HmacSha512(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacSha512加密");
            Encoding encoding = Encoding.UTF8;
            HMACSHA512 hmac512 = new HMACSHA512(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmac512, value, encoding);
        }
        #endregion

        #region HmacMd5加密
        public static string HmacMd5(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacMd5加密");
            Encoding encoding = Encoding.UTF8;
            HMACMD5 hmacmd5 = new HMACMD5(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmacmd5, value, encoding);
        }
        #endregion

        #region HmacRipeMd160加密
        public static string HmacRipeMd160(this string value, string keyVal)
        {
            if (value == null)
                throw new ArgumentNullException("不能对空字符串进行HmacRipeMd160加密");
            Encoding encoding = Encoding.UTF8;
            HMACRIPEMD160 hmacripemd160 = new HMACRIPEMD160(encoding.GetBytes(keyVal));
            return HashAlgorithmBase(hmacripemd160, value, encoding);
        }
        #endregion 
        #endregion

    }
}
