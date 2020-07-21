using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BaseS.Security
{
    /// <summary>
    /// aes加密
    /// </summary>
    public static class BAES
    {
        //默认密钥向量 
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// aes加密  明文字节流-->密文字节流
        /// </summary>
        /// <param name="srcBytes">原始字节流</param>
        /// <param name="destBytes">加密后字节流</param>
        /// <param name="key">密钥</param>
        /// <param name="coding">编码</param>
        /// <returns></returns>
        public static bool EncryptAES(this byte[] srcBytes, ref byte[] destBytes, string key, Encoding coding = null)
        {
            coding = coding ?? Encoding.UTF8;

            try
            {
                //分组加密算法
                SymmetricAlgorithm aes = Rijndael.Create();

                //设置密钥及密钥向量
                aes.Key = coding.GetBytes(key);
                aes.IV = _key1;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(srcBytes, 0, srcBytes.Length);
                cs.FlushFinalBlock();
                destBytes = ms.ToArray();//得到加密后的字节数组
                cs.Close();
                ms.Close();
            }
            catch (Exception ex)
            {
                ("aes加密失败，Exception:" + ex.Message + ex.StackTrace + " Key:" + key).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// aes加密 明文字符串-->base64转码
        /// </summary>
        /// <param name="srcString">原始字符串</param>
        /// <param name="destString">加密后字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="coding">编码</param>
        /// <returns></returns>
        public static bool EncryptAES(this string srcString, ref string destString, string key, Encoding coding = null)
        {
            byte[] destBytes = null;
            coding = coding ?? Encoding.UTF8;

            byte[] inputByteArray = coding.GetBytes(srcString);
            bool ret = EncryptAES(inputByteArray, ref destBytes, key, coding);

            if (ret)
            {
                destString = Convert.ToBase64String(destBytes);
            }

            return ret;
        }

        /// <summary>
        /// aes解密  密文字节流-->明文字节流
        /// </summary>
        /// <param name="srcBytes">密文字节流</param>
        /// <param name="destBytes">明文字节流</param>
        /// <param name="key">密钥</param>
        /// <param name="coding">编码</param>
        /// <returns></returns>
        public static bool DecryptAES(this byte[] srcBytes, ref byte[] destBytes, string key, Encoding coding = null)
        {
            coding = coding ?? Encoding.UTF8;

            try
            {
                SymmetricAlgorithm aes = Rijndael.Create();
                aes.Key = coding.GetBytes(key);
                aes.IV = _key1;
                destBytes = new byte[srcBytes.Length];
                MemoryStream ms = new MemoryStream(srcBytes);
                CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                cs.Read(destBytes, 0, destBytes.Length);
                cs.Close();
                ms.Close();
            }
            catch (Exception ex)
            {
                (" aes解密失败，Exception:" + ex.Message + ex.StackTrace + " Key:" + key).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// aes解密  密文字符串-->明文字符串
        /// </summary>
        /// <param name="srcString">密文字符串</param>
        /// <param name="destString">明文字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="coding">编码</param>
        /// <returns></returns>
        public static bool DecryptAES(this string srcString, ref string destString, string key, Encoding coding = null)
        {
            byte[] destBytes = null;

            coding = coding ?? Encoding.UTF8;

            byte[] inputByteArray = Convert.FromBase64String(srcString);

            bool ret = DecryptAES(inputByteArray, ref destBytes, key, coding);

            if (ret)
            {
                destString = coding.GetString(destBytes).Replace("\0", "");
            }

            return true;
        }
    }
}
