﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderGoodsAddReq : BaseInfoReq
    {
        public OrderGoods OrderGoods { get; set; }
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if (null == OrderGoods)
            {
                return "参数为空";
            }

            if (string.IsNullOrWhiteSpace(OrderGoods.Billid))
            {
                return "总单编号为空";
            }

            if ("add".Equals(Op))
            {
                if (string.IsNullOrWhiteSpace(OrderGoods.Realweight))
                {
                    return "预约重量为空";
                }
            }

            OrderGoods.Realweight = OrderGoods.Realweight.Trim('吨');
            OrderGoods.Realweight = OrderGoods.Realweight.Trim('t');
            OrderGoods.Realweight = OrderGoods.Realweight.Trim('T');
            OrderGoods.Realweight = OrderGoods.Realweight.Trim();

            if (Convert.ToDecimal(OrderGoods.Realweight) <= 0)
            {
                return "预约重量为空";
            }

            return "0";
        }
    }
}
