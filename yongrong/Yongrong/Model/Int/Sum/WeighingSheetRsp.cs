﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Srvc;

namespace Yongrong.Model.Int.Sum
{
    public  class WeighingSheetRsp : BaseRsp
    {

        /// <summary>
        /// 应答数据
        /// </summary>
        public List<WeighingSheetData> List { get; set; }
    }
}
