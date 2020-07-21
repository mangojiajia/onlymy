using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int
{
    public abstract class BaseReq
    {
        /// <summary>
        /// 成功应答
        /// </summary>
        public const string Success = "0";

        public abstract string Check();

        public string Token { get; set; }
    }
}
