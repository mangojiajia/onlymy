using BaseS.String;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.Phone
{
    public class PhoneQueryOrderReq : BasePageReq
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 时间戳参数time 
        /// 时间戳是指UTC时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
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

            return Success;
        }
    }
}
