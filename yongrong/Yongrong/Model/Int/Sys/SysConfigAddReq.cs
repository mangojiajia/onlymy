﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;

namespace Yongrong.Model.Int.Sys
{
   public class SysConfigAddReq : BaseInfoReq
    {
        public SysConfig Query { get; set; }

        public override string Check()
        {
            if (null == Query)
            {
                return "Query不能为空";
            }

            if (string.IsNullOrWhiteSpace(Query.Ekey))
            {
                return "英文名不能为空";
            }

            Query.Ekey = Query.Ekey.Trim();

            if (string.IsNullOrWhiteSpace(Query.Ckey))
            {
                return "中文名不能为空";
            }

            Query.Ckey = Query.Ckey.Trim();

            if (string.IsNullOrWhiteSpace(Query.Price))
            {
                return "值不能为空";
            }

            Query.Price = Query.Price.Trim();

            return Success;
        }

        }
}
