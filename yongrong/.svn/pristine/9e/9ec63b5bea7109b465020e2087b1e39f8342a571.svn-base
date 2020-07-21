using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sys
{
    public class UserOpLogGetReq : BasePageReq
    {
        /// <summary>
        /// 状态
        /// </summary>
        public UserOplog Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if(string.IsNullOrWhiteSpace(Token))
            {
                return "Token为空";
            }

            if (null == Query)
            {
                return "Query 为空";
            }

            return Success;
        }
    }
}
