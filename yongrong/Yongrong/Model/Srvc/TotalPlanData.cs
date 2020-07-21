using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Srvc
{
    /// <summary>
    /// 接受每天进场/出厂/退货总单计划数据
    /// </summary>
    public partial class TotalPlanData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string RiQi { get; set; }

        /// <summary>
        /// 总量
        /// </summary>
        public Double TotalNum { get; set; }

        /// <summary>
        /// 剩余量
        /// </summary>
        public Double SurplusNum { get; set; }
    }
}
