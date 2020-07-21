using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderInfoRsp : BasePageRsp
    {
        public List<OrderInfo> List { get; set; }
    }
}
