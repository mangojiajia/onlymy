using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sys
{
   public class AbnormalGetReq : BasePageReq
    {
        public Abnormal Query { get; set; }

        public override string Check()
        {
            if (null != Query)
            {
                Query.Abnormalname = Query.Abnormalname?.Trim();
                Query.Abnormaltype = Query.Abnormaltype?.Trim();
                Query.Isdispose = Query.Isdispose?.Trim();
            }

            return Success;
        }
    }
}
