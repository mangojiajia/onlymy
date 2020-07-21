using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int
{
    public abstract class BasePageReq : BaseReq
    {
        /// <summary>
        /// 分页请求
        /// </summary>
        public PageBean Page { get; set; }
    }
}
