using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTractorReq:BasePageReq
    { 
        public BaseTractor Query { get; set; }
        public override string Check()
        {
            return "0";
        }
    }
}
