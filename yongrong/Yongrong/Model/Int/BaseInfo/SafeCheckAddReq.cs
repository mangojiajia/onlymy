﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class SafeCheckAddReq : BaseInfoReq
    {
        public SafeCheck SafeCheck { get; set; }
        public override string Check()
        {
            return "0";
        }
    }
}
