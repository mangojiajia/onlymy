using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sum
{
    public class GateGetRsp : BasePageRsp
    {
        /// <summary>
        /// 应答数据
        /// </summary>
        public List<ApiGateevent> List { get; set; }
    }
}
