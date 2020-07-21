using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderInfoAddReq : BaseInfoReq
    {
        public OrderInfo OrderInfo { get; set; }

        public override string Check()
        {
            return "0";
        }
    }
}
