using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;

namespace Yongrong.Model.Int.Sys
{
   public class AbnormalAddReq : BaseInfoReq
    {

        public Abnormal Query { get; set; }

        public override string Check()
        {
            if (string.IsNullOrWhiteSpace(Query.Abnormalname))
            {
                return "异常名不能为空";
            }

            Query.Abnormalname = Query.Abnormalname.Trim();

            if (string.IsNullOrWhiteSpace(Query.Abnormalcase))
            {
                return "异常情况不能为空";
            }

            Query.Abnormalcase = Query.Abnormalcase.Trim();

            return Success;
        }

    }
}
