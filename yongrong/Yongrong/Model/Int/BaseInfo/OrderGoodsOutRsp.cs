using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderGoodsOutRsp : BasePageRsp
    {
        public List<OrderGoods> List { get; set; }
    }
}
