using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseCustomerAddReq : BaseInfoReq
    {
        public BaseCustomer Customer { get; set; }

        public int? Issynch { get; set; } 

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
            if(null == Customer)
            {
                return "参数为空";
            }
            if(string.IsNullOrWhiteSpace(Customer.Enterprisename))
            {
                return "公司名称为空";
            }
            /*if(string.IsNullOrWhiteSpace(Customer.Creditid))
            {
                return "统一会信用代码为空";
            }
            if(string.IsNullOrWhiteSpace(Customer.Legalname))
            {
                return "法人为空";
            }*/
            return "0";
        }
    }
}
