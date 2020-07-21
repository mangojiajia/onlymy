using System;
using System.Collections.Generic;
using System.Text;
using BaseS.String;

namespace Yongrong.Model.Int.Pda
{
    public class PdaSubmitCheckReq
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
        /// 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CheckResult CheckData { get; set; }

        public string Check()
        {
            if (null == CheckData)
            {
                return "提交审核结果为空";
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

    public class CheckResult
    {
        public int Id { get; set; }

        public string Uid { get; set; }

        public string Expburn { get; set; }

        public string Helmet { get; set; }

        public string Workshoe { get; set; }

        public string Smock { get; set; }

        public string Remark { get; set; }

        public string Tire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllPass
        {
            get
            {
                if ("pass".Equals(Expburn) 
                    && "pass".Equals(Helmet) 
                    && "pass".Equals(Workshoe)
                    && "pass".Equals(Smock)
                    && "pass".Equals(Tire))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
