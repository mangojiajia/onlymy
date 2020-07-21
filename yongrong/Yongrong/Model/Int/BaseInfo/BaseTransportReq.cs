using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTransportReq : BasePageReq
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public BaseTransport Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            return "0";
        }
    }
}
