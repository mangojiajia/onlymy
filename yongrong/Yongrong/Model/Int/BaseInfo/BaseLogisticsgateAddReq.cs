using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseLogisticsgateAddReq : BaseInfoReq
    {

        public BaseLogisticsgate Gate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            if(null == Gate)
            {
                return "参数为空";
            }
            if(string.IsNullOrWhiteSpace(Gate.Gateid))
            {
                return "物流门编号为空";
            }
            if(string.IsNullOrWhiteSpace(Gate.Gatename))
            {
                return "物流门名称为空";
            }
            return "0";
        }
    }
}
