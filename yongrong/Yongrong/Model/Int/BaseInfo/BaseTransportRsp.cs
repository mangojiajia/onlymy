﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTransportRsp : BasePageRsp
    {
        public List<BaseTransport> Transpots { get; set; }
    }
}
