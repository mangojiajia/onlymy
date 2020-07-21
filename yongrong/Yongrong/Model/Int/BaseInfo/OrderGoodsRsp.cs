using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderGoodsRsp: BasePageRsp
    {
        public List<OrderGoods> List { get; set; }

        public OrderGoods order { get; set; }
    }
}
