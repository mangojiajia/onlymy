﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseS.String
{
    public static class BMobile
    {
        /// <summary>
        /// 移动
        /// </summary>
        public const int MOBILE = 1;

        /// <summary>
        /// 联通
        /// </summary>
        public const int UNION = 2;

        /// <summary>
        /// 电信
        /// </summary>
        public const int TELCOM = 3;

        /// <summary>
        /// 广电
        /// </summary>
        public const int GUANGDIAN = 4;

        /// <summary>
        /// 未知
        /// </summary>
        public const int UNKNOWN = -1;

        /// <summary>
        /// 移动号码正则表达式
        /// </summary>
        private static readonly Regex mobileRole = new Regex("^1((34[0-8]{1})|(703|705|706)[0-9]{7})|((35|36|37|38|39|47|48|50|51|52|54|57|58|59|72|78|82|83|84|87|88|95|97|98)[0-9]{8})$");

        /// <summary>
        /// 联通号码正则表达式
        /// </summary>
        private static readonly Regex unionRole = new Regex("^1(704|707|708|709[0-9]{7})|((30|31|32|45|46|55|56|66|71|75|76|85|86|96)[0-9]{8})$");

        /// <summary>
        /// 电信号码正则表达式
        /// </summary>
        private static readonly Regex telecomRole = new Regex("^1(700|701|702|349[0-9]{7})|((33|53|73|77|80|81|89|90|91|93|99)[0-9]{8})$");

        /// <summary>
        /// 广电号码
        /// </summary>
        private static readonly Regex guangdianRole = new Regex("^1((92)[0-9]{8})$");

        /// <summary>
        /// 固定电话号码正则表达式
        /// </summary>
        private static readonly Regex phoneRole = new Regex(@"^(0[0-9]{2,3})([2-9][0-9]{6,7})$");

        /// <summary>
        /// 所有手机号码规则
        /// </summary>
        private static readonly Regex allPhoneRole = new Regex(@"^1(3[0-9]|4[579]|5[0-3,5-9]|6[6]|7[0135678]|8[0-9]|9[89])\d{8}$");

        /// <summary>
        /// 区分手机号码运营商
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns>1移动 2联通 3电信</returns>
        public static int GetMobileType(this string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile) || 11 != mobile.Length)
            {
                return BMobile.UNKNOWN;
            }

            // 移动
            if (mobileRole.IsMatch(mobile))
            {
                return BMobile.MOBILE;
            }

            // 联通
            if (unionRole.IsMatch(mobile))
            {
                return BMobile.UNION;
            }

            // 电信
            if (telecomRole.IsMatch(mobile))
            {
                return BMobile.TELCOM;
            }

            // 未知类型
            return BMobile.UNKNOWN;
        }

        /// <summary>
        /// 是否是固话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsFixedPhone(this string phone)
        {
            return phoneRole.IsMatch(phone);
        }

        /// <summary>
        /// 是否匹配所有手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsAllPhone(this string phone)
        {
            return allPhoneRole.IsMatch(phone);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string CheckPhone(this string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return string.Empty;
            }

            phone = phone.Replace(",", "").Replace("，", "")
                .Replace("'", "").Replace("＇", "").Replace("‘", "").Replace("’", "")
                .Replace("“", "").Replace("”", "").Replace("\"", "")
                .Replace(":", "").Replace("：", "")
                .Replace(".", "").Replace("。", "")
                .Trim();

            return phone;
        }

        /// <summary>
        /// 移动联通电信号码(非固定电话)规则化 如果不是有效号码则返回空字符串
        /// </summary>
        /// <param name="numStr"></param>
        /// <returns></returns>
        public static string ToMobile(this string numStr)
        {
            if (string.IsNullOrWhiteSpace(numStr))
            {
                return string.Empty;
            }

            // 截取有效部分号码
            if (0 < numStr.IndexOf(','))
            {
                numStr = numStr.Substring(0, numStr.IndexOf(','));
            }

            numStr = numStr.Replace("-", "").Trim();

            // 国家前缀
            if (numStr.StartsWith("86"))
            {
                numStr = numStr.Substring(2);
            }

            // 地区号
            if (!numStr.StartsWith("010") &&
                 (numStr.StartsWith("01") || numStr.StartsWith("10")))
            {
                numStr = numStr.Substring(2);
            }
            
            //是否为固定电话
            if (IsFixedPhone(numStr))
            {
                return string.Empty;
            }

            if (mobileRole.IsMatch(numStr)
                || unionRole.IsMatch(numStr)
                || telecomRole.IsMatch(numStr))
            {
                return numStr;
            }

            return string.Empty;
        }
    }
}
