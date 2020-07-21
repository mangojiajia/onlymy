using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseS.Net
{
    public static class BIP
    {
        /// <summary>
        /// IP地址规则
        /// </summary>
        private static Regex ipReg = new Regex(@"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");

        /// <summary>
        /// 是否是有效的IP4地址
        /// </summary>
        /// <returns></returns>
        public static bool IsIPAddr(this string ipStr)
        {
            if (string.IsNullOrWhiteSpace(ipStr))
            {
                return false;
            }

            if (ipReg.IsMatch(ipStr))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 字符串IP地址转成IPEndPoint
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static IPEndPoint String2IP(string ipStr, int port)
        {
            IPEndPoint ep = null;

            if (string.IsNullOrWhiteSpace(ipStr))
            {
                ep = new IPEndPoint(IPAddress.Any, 0);
            }
            else if (0 > port)
            {
                if (ipStr.Contains(':'))
                {
                    ep = new IPEndPoint(IPAddress.Parse(ipStr.Substring(0, ipStr.IndexOf(":"))), int.Parse(ipStr.Substring(ipStr.IndexOf(":") + 1)));
                }
                else
                {
                    ep = new IPEndPoint(IPAddress.Parse(ipStr), 0);
                }
            }
            else
            {
                if (ipStr.Contains(':'))
                {
                    ep = new IPEndPoint(IPAddress.Parse(ipStr.Substring(0, ipStr.IndexOf(":"))), port);
                }
                else
                {
                    ep = new IPEndPoint(IPAddress.Parse(ipStr), port);
                }
            }

            return ep;
        }
    }
}
