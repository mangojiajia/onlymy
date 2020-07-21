using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.User
{
    public class UserLoginAddReq:BaseReq
    {
        public Userinfo User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if (null == User)
            {
                return "参数为空";
            }

            return "0";
        }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        public class Userinfo
        {
            public string Tel { get; set; }

            public string Account { get; set; }

            public string Pwd { get; set; }

            public string Post { get; set; }
        }
    }
}
