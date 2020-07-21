﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BaseS.String
{
    /// <summary>
    /// 字符串相关工具
    /// </summary>
    public static class BString
    {
        /// <summary>
        /// 
        /// </summary>
        private const string numPattern = @"^\d*$";

        /// <summary>
        /// 判断是否包含汉字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsContainChinese(this string s)
        {
            return Regex.IsMatch(s, @"[\4e00-\u9fa5]+");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumberStr(this string s)
        {
            return Regex.IsMatch(s, numPattern);
        }

        /// <summary>
        /// 是否是数字字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumberString(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            char[] chars = s.ToCharArray();
            int i = 0;

            if (chars.Length > 1 && chars[0] == '-')
            {
                i = 1;
            }

            for (; i < chars.Length; i++)
            {
                if (!Char.IsDigit(chars[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 判断邮箱是否正确
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmail(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }
            return Regex.IsMatch(source, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 字节码转字符串
        /// </summary>
        /// <param name="barr">字节码数组</param>
        /// <param name="coding">字符编码</param>
        /// <param name="pos">需要转换的字节码起始位置</param>
        /// <param name="length">需要转换的字节码的长度</param>
        /// <returns></returns>
        public static string B2String(this byte[] barr, Encoding coding = null, int pos = 0, int length = -1)
        {
            if (null == barr)
            {
                return string.Empty;
            }

            // 计算需要转换的字节长度
            if (-1 == length)
            {
                length = barr.Length - pos;
            }

            // 转换字节长度不足
            if (0 >= length)
            {
                return string.Empty;
            }

            if (pos + length > barr.Length)
            {
                return string.Empty;
            }

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            return coding.GetString(barr, pos, length);
        }

        /// <summary>
        /// 字符串转成字节码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static byte[] String2B(this string content, Encoding coding = null)
        {
            if (null == content)
            {
                return null;
            }

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            return coding.GetBytes(content);
        }

        /// <summary>
        /// 字节码转base64字符串
        /// </summary>
        /// <param name="barr"></param>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string B2Base64(this byte[] barr, int pos = 0, int length = -1)
        {
            if (null == barr)
            {
                return string.Empty;
            }

            // 计算需要转换的字节长度
            if (-1 == length)
            {
                length = barr.Length - pos;
            }

            // 转换字节长度不足
            if (0 >= length)
            {
                return string.Empty;
            }

            if (pos + length > barr.Length)
            {
                return string.Empty;
            }

            return Convert.ToBase64String(barr, pos, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string Str2Base64(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }

            byte[] utf8bytes = Encoding.UTF8.GetBytes(src);

            return B2Base64(utf8bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static byte[] Base642Bytes(this string dst)
        {
            if (string.IsNullOrWhiteSpace(dst))
            {
                return null;
            }

            return Convert.FromBase64String(dst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static string Base642Str(this string dst)
        {
            if (string.IsNullOrWhiteSpace(dst))
            {
                return string.Empty;
            }

            return B2String(Base642Bytes(dst));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        public static bool Str2Time(this string timeStr, out DateTime dt, string dtformat = "yyyy-MM-dd HH:mm:ss", DateTime? defTime = null)
        {
            dt = defTime ?? DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(timeStr))
            {
                if (null != defTime)
                {
                    dt = defTime ?? DateTime.MinValue;
                    return true;
                }
                return false;
            }

            if (!DateTime.TryParseExact(timeStr, dtformat, null, System.Globalization.DateTimeStyles.None, out dt))
            {
                if (null != defTime)
                {
                    dt = defTime ?? DateTime.MinValue;
                    return true;
                }

                return false;
            }


            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DateTimeStr"></param>
        /// <param name="dtformat"></param>
        /// <param name="defTime"></param>
        /// <returns></returns>
        public static DateTime StringToDateTime(this string DateTimeStr, string dtformat = "yyyy-MM-dd HH:mm:ss", DateTime? defTime = null)
        {
            DateTime dateTime = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(DateTimeStr))
            {
                if (null != defTime)
                {
                    return defTime ?? DateTime.MinValue;
                }

                return dateTime;
            }

            if (!DateTime.TryParseExact(DateTimeStr, dtformat, null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                if (null != defTime)
                {
                    return defTime ?? DateTime.MinValue;
                }
            }

            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Str2Url(this string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pArr"></param>
        /// <returns></returns>
        public static List<string> Str2List(bool filter, params string[] pArr)
        {
            List<string> pList = new List<string>();

            if (null == pArr)
            {
                return pList;
            }

            foreach (var p in pArr)
            {
                if (filter)
                {
                    if (string.IsNullOrWhiteSpace(p))
                    {
                        continue;
                    }

                    if (pList.Contains(p))
                    {
                        continue;
                    }
                }

                pList.Add(p);
            }

            return pList;
        }

        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(this string str, bool Contains = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            try
            {
                string urlReg = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

                if (!Contains)
                {
                    urlReg = "^" + urlReg + "$";
                }

                return Regex.IsMatch(str, urlReg);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBlockStr(this string str, bool blockUrl = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            if (blockUrl && IsUrl(str, true))
            {
                return true;
            }

            if (str.ToLower().Contains("<script")
                || str.ToLower().Contains("<html"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取 utc 1970-1-1到现在的秒数
        /// </summary>
        /// <returns></returns>
        public static long Get1970ToNowSeconds(DateTime? dt = null)
        {
            return ((dt ?? DateTime.Now).ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 获取 utc 1970-1-1到现在的毫秒数
        /// </summary>
        /// <returns></returns>
        public static long Get1970ToNowMilliseconds(DateTime? dt = null)
        {
            return ((dt ?? DateTime.Now).ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "0";
            }
            return str;
        }

    }
}
