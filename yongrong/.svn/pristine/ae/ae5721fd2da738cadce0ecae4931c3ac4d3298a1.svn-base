using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class OrderConfigAddReq: BaseInfoReq
    {
        public OrderConfig OrderConfig { get; set; }
        public override string Check()
        {
            if(null == OrderConfig)
            {
                return "请求参数为空";
            }
            if ("del".Equals(Op))
            {
                return Success;
            }
            if(string.IsNullOrWhiteSpace(OrderConfig.Goodsname))
            {
                return "物料名称为空";
            }
            if(OrderConfig.Maxcarnumber <= 0)
            {
                return "当日预约车辆总数/大货车数量限制必须大于零";
            }
            /*if(null != OrderConfig.Configtype && "发货区".Equals(OrderConfig.Configtype))
            {
                if(string.IsNullOrWhiteSpace(OrderConfig.Ordertime))
                {
                    return "预约时间为空";
                }
            }*/
            return "0";
        }
    }
}
