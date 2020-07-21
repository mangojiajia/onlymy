using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Srvc
{
    /// <summary>
    /// 接受车辆每天入场数量数据
    /// </summary>
    public partial class VehicleEntryData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Dataday { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Int32 Num { get; set; }
    }
}
