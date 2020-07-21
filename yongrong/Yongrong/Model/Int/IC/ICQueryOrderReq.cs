using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.IC
{
    public class ICQueryOrderReq
    {
        /// <summary>
        /// 预约号
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string Tractor { get; set; }
    }
}
