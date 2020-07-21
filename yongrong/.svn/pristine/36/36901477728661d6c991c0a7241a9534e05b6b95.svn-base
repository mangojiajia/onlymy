using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sys
{
    public class RoleGetReq : BasePageReq
    {
        public Role Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if (null == this.Page)
            {
                this.Page = new PageBean();                    
            }

            return Success;
        }
    }
}
