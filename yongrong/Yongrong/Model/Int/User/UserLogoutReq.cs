using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.User
{
    public class UserLogoutReq : BaseReq
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                if (string.IsNullOrWhiteSpace(UserId))
                {
                    return "账号无效";
                }
            }

            return Success;
        }
    }
}
