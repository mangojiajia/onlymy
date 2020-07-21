using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseCustomerReq : BasePageReq
    {
        public string Enterpriseid { get; set; }
        public string Enterprisename { get; set; }
        public string Creditid { get; set; }

        public override string Check()
        {
            return "0";
        }
    }
}
