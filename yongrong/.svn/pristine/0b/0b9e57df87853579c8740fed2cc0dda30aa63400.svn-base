using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.Phone
{
    public class PhoneUpdOrderReq : BaseReq
    {
        /// <summary>
        /// 时间戳参数time 
        /// 时间戳是指UTC时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        public string Id { get; set; }

        public string Orderstat { get; set; }


        public override string Check()
        {
            return Success;
        }
    }
}
