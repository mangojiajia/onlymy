﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Srvc
{
    /// <summary>
    /// 接受过磅单数据
    /// </summary>
    public partial class WeighingSheetData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Dataday { get; set; }

        /// <summary>
        /// 司机预约总车次
        /// </summary>
        public Int32 Total1 { get; set; }


        /// <summary>
        /// 完成审核车次
        /// </summary>
        public Int32 Complete1 { get; set; }

    }
}
