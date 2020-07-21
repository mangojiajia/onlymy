using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Srvc
{
    /// <summary>
    /// 接受客户提货/供应商送货数量汇总数据
    /// </summary>
    public partial class PickUpData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Dataday { get; set; }

        /// <summary>
        /// 总卸货量/装货量
        /// </summary>
        public Double TotalLoad { get; set; }
    }
}
