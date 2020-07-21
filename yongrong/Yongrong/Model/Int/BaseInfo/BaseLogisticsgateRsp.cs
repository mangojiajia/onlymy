using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseLogisticsgateRsp : BasePageRsp
    {
        public List<BaseLogisticsgate> Gates { get; set; }
    }
}
