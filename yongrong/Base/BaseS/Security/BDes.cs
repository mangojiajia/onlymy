using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BaseS.String;
using System.IO;

namespace BaseS.Security
{
    /// <summary>
    /// 对称加密
    /// </summary>
    public static class BDes
    {
        /// <summary>
        /// 对称加密字符集
        /// </summary>
        public static Encoding DesCoding = Encoding.UTF8;

        /// <summary>
        /// 3Des加密
        /// </summary>
        /// <param name="srcBytes">明文-原始内容字节流</param>
        /// <param name="key">加密 密钥</param>
        /// <returns>密文</returns>
        public static bool EncryptTripleDES(this byte[] srcBytes, ref byte[] destBytes, string key, Encoding coding = null, CipherMode mode = CipherMode.ECB)
        {
            if (null == coding)
            {
                coding = DesCoding;
            }

            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
                {
                    Key = coding.GetBytes(key),
                    Mode = mode
                };

                ICryptoTransform desEncrypt = des.CreateEncryptor();
                destBytes = desEncrypt.TransformFinalBlock(srcBytes, 0, srcBytes.Length);
            }
            catch (Exception e)
            {
                ("Key:" + key + "Err:" + e.Message + e.StackTrace).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 3Des加密 base64转码
        /// </summary>
        /// <param name="srcString">明文-原始内容</param>
        /// <param name="key">加密 密钥</param>
        /// <returns>密文</returns>
        public static bool EncryptTripleDES(this string srcString, ref string destStr, string key, Encoding coding = null, CipherMode mode = CipherMode.ECB)
        {
            if (null == coding)
            {
                coding = DesCoding;
            }

            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
                {
                    Key = coding.GetBytes(key),
                    Mode = mode
                };

                ICryptoTransform desEncrypt = des.CreateEncryptor();

                byte[] srcBytes = coding.GetBytes(srcString);
                byte[] desBytes = desEncrypt.TransformFinalBlock(srcBytes, 0, srcBytes.Length);

                destStr = Convert.ToBase64String(desBytes);
            }
            catch (Exception e)
            {
                ("SrcString:" + srcString + " Key:" + key + " Err：" + e.Message + e.StackTrace).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 3Des解密
        /// </summary>
        /// <param name="destString">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>原文</returns>
        public static bool DecryptTripleDES(byte[] destBytes, ref byte[] srcBytes, string key, Encoding coding = null, CipherMode mode = CipherMode.ECB)
        {
            if (null == coding)
            {
                coding = DesCoding;
            }

            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
                {
                    Key = coding.GetBytes(key),
                    Mode = mode
                };

                ICryptoTransform descDecrypt = des.CreateDecryptor();
                srcBytes = descDecrypt.TransformFinalBlock(destBytes, 0, destBytes.Length);
            }
            catch (Exception e)
            {
                ("Key:" + key + " Err:" + e.Message + e.StackTrace + "destBytes:" + destBytes.B2Base64()).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="destString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DecryptTripleDES(this string destString, ref string srcString, string key, Encoding coding = null, CipherMode mode = CipherMode.ECB)
        {
            if (null == coding)
            {
                coding = DesCoding;
            }

            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider()
                {
                    Key = coding.GetBytes(key),
                    Mode = mode
                };

                ICryptoTransform descDecrypt = des.CreateDecryptor();
                byte[] destBytes = Convert.FromBase64String(destString);
                byte[] srcBytes = descDecrypt.TransformFinalBlock(destBytes, 0, destBytes.Length);
                srcString = srcBytes.B2String(coding);
            }
            catch (Exception e)
            {
                ("DestString：" + destString + " Key:" + key + " Err:" + e.Message + e.StackTrace).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool EncryptDES(this string src, ref string dest, string key, Encoding srccoding = null)
        {
            byte[] destBytes = null;

            srccoding = srccoding ?? DesCoding;

            bool ret = EncryptDES(srccoding.GetBytes(src), ref destBytes, key);

            if (ret)
            {
                dest = Convert.ToBase64String(destBytes);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="destBytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool EncryptDES(this byte[] srcBytes, ref byte[] destBytes, string key)
        {
            try
            {
                byte[] b8 = new byte[8];

                if (string.IsNullOrEmpty(key) || 7 >= key.Length)
                {
                    key += ".mAn1Man";
                }

                Buffer.BlockCopy(DesCoding.GetBytes(key), 0, b8, 0, 8);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider()
                {
                    Key = b8,
                    IV = b8
                };

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(srcBytes, 0, srcBytes.Length);
                cs.FlushFinalBlock();

                destBytes = ms.ToArray();
            }
            catch (Exception e)
            {
                ("Message:" + e.Message + e.StackTrace + " Key:" + key + " srcStr：" + srcBytes.B2String()).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcBytes"></param>
        /// <param name="destBytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool EncryptDES(this byte[] srcBytes, ref byte[] destBytes, byte[] key)
        {
            try
            {
                if (null == key || 8 != key.Length)
                {
                    return false;
                }

                DESCryptoServiceProvider des = new DESCryptoServiceProvider()
                {
                    Key = key,
                    //IV = key
                };

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(srcBytes, 0, srcBytes.Length);
                cs.FlushFinalBlock();

                destBytes = ms.ToArray();
            }
            catch (Exception e)
            {
                ("Message:" + e.Message + e.StackTrace + " Key:" + key + " srcStr：" + srcBytes.B2String()).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DecryptDES(this string dest, ref string src, string key)
        {
            byte[] srcBytes = null;
            bool ret = DecryptDES(Convert.FromBase64String(dest), ref srcBytes, key);

            if (ret)
            {
                src = srcBytes.B2String();
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="srcBytes"></param>
        /// <param name="key"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool DecryptDES(this byte[] destBytes, ref byte[] srcBytes, string key)
        {
            if (null == destBytes)
            {
                return false;
            }

            byte[] b8 = new byte[8];
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            if (string.IsNullOrEmpty(key) || 7 >= key.Length)
            {
                key += ".mAn1Man";
            }

            Buffer.BlockCopy(DesCoding.GetBytes(key), 0, b8, 0, 8);

            try
            {
                des.Key = b8;
                des.IV = b8;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(destBytes, 0, destBytes.Length);
                cs.FlushFinalBlock();

                srcBytes = ms.ToArray();
            }
            catch (Exception e)
            {
                ("Exception:" + e.Message + e.StackTrace + " Key:" + key + " dest：" + destBytes.B2Base64()).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="srcBytes"></param>
        /// <param name="key"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool DecryptDES(this byte[] destBytes, ref byte[] srcBytes, byte[] key)
        {
            if (null == destBytes)
            {
                return false;
            }

            if(null == key || 8 != key.Length)
            {
                return false;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            
            try
            {
                des.Key = key;
                des.IV = key;

                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

                cs.Write(destBytes, 0, destBytes.Length);
                cs.FlushFinalBlock();

                srcBytes = ms.ToArray();
            }
            catch (Exception e)
            {
                ("Exception:" + e.Message + e.StackTrace + " Key:" + key + " dest：" + destBytes.B2Base64()).Warn();
                return false;
            }

            return true;
        }

    }
}
