using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderGoodsReq : BasePageReq
    {
        public OrderGoods Query { get; set; }

        public override string Check()
        {
            if (null != Query)
            {
                Query.Driver = Query.Driver?.Trim();
            }
            return "0";
        }
    }
}
