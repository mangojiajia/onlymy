using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BaseS.Security
{
    /// <summary>
    /// SHA1Hash算法
    /// </summary>
    public static class BSHA1
    {
        /// <summary>
        /// 
        /// </summary>
        public static Encoding ShaCoding = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string msg)
        {
            return ToSHA1Bytes(ShaCoding.GetBytes(msg));
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="msgbytes"></param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this byte[] msgbytes)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            return sha.ComputeHash(msgbytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ToSHA1String(this string msg)
        {
            StringBuilder hashSB = new StringBuilder(333);

            foreach (byte bt in ToSHA1Bytes(msg))
            {
                hashSB.AppendFormat("{0:x2}", bt);
            }

            return hashSB.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgbytes"></param>
        /// <returns></returns>
        public static string ToSHA1String(this byte[] msgbytes)
        {
            StringBuilder hashSB = new StringBuilder(333);

            foreach (byte bt in ToSHA1Bytes(msgbytes))
            {
                hashSB.AppendFormat("{0:x2}", bt);
            }

            return hashSB.ToString();
        }
    }
}
