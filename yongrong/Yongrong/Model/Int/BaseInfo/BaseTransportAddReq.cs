using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTransportAddReq : BaseInfoReq
    {
        public BaseTransport Transport { get; set; }

        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            if(null == Transport)
            {
                return "参数为空";
            }
            if(string.IsNullOrWhiteSpace(Transport.Name))
            {
                return "运输公司名称为空";
            }
            /*if(string.IsNullOrWhiteSpace(Transport.Legal))
            {
                return "法人为空";
            }*/
            /*if(string.IsNullOrWhiteSpace(Transport.Code))
            {
                return "运输公司编码为空";
            }*/
            return "0";
        }
    }
}
