using System;
using System.Collections.Generic;
using System.Text;
using BaseS.String;
using BaseS.Security;

namespace Yongrong.Model.Int.Pda
{
    /// <summary>
    /// pda登录请求
    /// </summary>
    public class PdaLoginReq
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 时间戳参数time
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Check()
        {
            if (string.IsNullOrWhiteSpace(Uid))
            {
                return "账号为空";
            }

            if (string.IsNullOrWhiteSpace(Time))
            {
                return "时间戳为空";
            }

            if (string.IsNullOrWhiteSpace(Sign))
            {
                return "签名为空";
            }

            var t = BString.Get1970ToNowMilliseconds(DateTime.Now);

            long.TryParse(Time, out var src);

            if (Yongrong.Conf.BaseConf.Sys.SysTimeSpan < Math.Abs(t - src))
            {
                return "时间戳差距过大";
            }

            return "0";
        }

    }
}
