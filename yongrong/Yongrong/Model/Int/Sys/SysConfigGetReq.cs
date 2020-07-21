using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sys
{
   public  class SysConfigGetReq : BasePageReq
    {
        public SysConfig Query { get; set; }

        public override string Check()
        {
            if (null != Query)
            {
                Query.Ckey = Query.Ckey?.Trim();
                Query.Ekey = Query.Ekey?.Trim();
                Query.Price = Query.Price?.Trim();
            }
            return Success;
        }

    }
}
