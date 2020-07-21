using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
  public  class BaseSupplierAddReq : BaseInfoReq
    {

        public BaseSupplier Supplier { get; set; }

        public int? Issynch { get; set; }
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            if(null == Supplier)
            {
                return "参数为空";
            }
            if(string.IsNullOrWhiteSpace(Supplier.Enterprisename))
            {
                return "公司名称为空";
            }
            /*if(string.IsNullOrWhiteSpace(Supplier.Creditid))
            {
                return "统一会信用代码为空";
            }*/
            return "0";
        }
    }
}
