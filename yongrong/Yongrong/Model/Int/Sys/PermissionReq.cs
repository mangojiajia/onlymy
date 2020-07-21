using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Org
{
    public class PermissionReq: BasePageReq
    {
        public Permission Query { get; set; }
        public override string Check()
        {
            return "0";
        }
    }
}
