using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.User
{
    public class UserLoginReq : BasePageReq
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }






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

                if (string.IsNullOrWhiteSpace(Pwd))
                {
                    return "密码无效";
                }
            }

            Page = new PageBean();

            return "0";
        }
    }
}
