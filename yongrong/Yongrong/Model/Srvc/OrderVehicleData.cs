using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Srvc
{
    /// <summary>
    /// 接受每天预约车辆数据
    /// </summary>
    public partial class OrderVehicleData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string OrderTime { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Int32 Num { get; set; }
    }
}
