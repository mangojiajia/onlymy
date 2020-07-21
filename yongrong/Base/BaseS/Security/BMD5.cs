using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BaseS.Security
{
    /// <summary>
    /// MD5 算法
    /// </summary>
    public static class BMD5
    {
        /// <summary>
        /// Md5默认的字符集
        /// </summary>
        public static Encoding Md5Coding = Encoding.UTF8;

        /// <summary>
        /// MD5算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5String(this byte[] input, int len = 16, bool upper = false)
        {
            byte[] hashbytes = ToMD5Bytes(input);

            System.Text.StringBuilder s = new System.Text.StringBuilder();

            foreach (byte b in hashbytes)
            {
                s.Append(b.ToString("x2"));
            }

            var s16 = s.ToString();

            if (16 == len)
            {
                s16 = s.ToString().Substring(8, 16);
            }

            if (!upper)
            {
                return s16;
            }
            else
            {
                return s16?.ToUpper();
            }
        }

        /// <summary>
        /// MD5 hash算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(this byte[] input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            return md5Hasher.ComputeHash(input);
        }

        /// <summary>
        /// MD5 hash算法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(this string input, Encoding encoder = null)
        {
            encoder = encoder ?? Md5Coding;

            return ToMD5Bytes(encoder.GetBytes(input));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ToMD5String(this string input, int len = 16, Encoding encoder = null, bool upper = false)
        {
            encoder = encoder ?? Md5Coding;

            return ToMD5String(encoder.GetBytes(input), len, upper);
        }
    }
}
