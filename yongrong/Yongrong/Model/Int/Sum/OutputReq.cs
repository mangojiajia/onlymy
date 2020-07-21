using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sum
{
    public class OutputReq : BaseReq
    {
        public ApiGateevent Query { get; set; }

        public override string Check()
        {
            if(null == Query)
            {
                return "参数为空";
            }
            return "0";
        }
    }
}
