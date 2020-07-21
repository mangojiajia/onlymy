using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.User
{
    public class UserResetPwdReq : BasePageReq
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserId;

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd;

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd;

        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPwd;

        public override string Check()
        {
            if(string.IsNullOrWhiteSpace(UserId))
            {
                return "账号为空";
            }
            if(string.IsNullOrWhiteSpace(OldPwd))
            {
                return "原密码为空";
            }
            if (string.IsNullOrWhiteSpace(NewPwd))
            {
                return "新密码为空";
            }
            if (string.IsNullOrWhiteSpace(ConfirmPwd))
            {
                return "确认密码为空";
            }
            if (!NewPwd.Equals(ConfirmPwd))
            {
                return "新密码和确认密码不一致";
            }            
            return "0";
        }
    }
}
