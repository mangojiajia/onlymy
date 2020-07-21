using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sum
{
    public class GateGetReq : BasePageReq
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiGateevent Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if (null == Page)
            {
                Page = new PageBean();
            }

            return Success;
        }
    }
}
