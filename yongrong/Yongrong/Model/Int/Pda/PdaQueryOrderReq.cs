using System;
using System.Collections.Generic;
using System.Text;
using BaseS.String;

namespace Yongrong.Model.Int.Pda
{
    public class PdaQueryOrderReq
    {
        /// <summary>
        /// 时间戳参数
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 预约号
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string Tractor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Check()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                return "Token为空";
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
