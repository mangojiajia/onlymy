using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;

namespace Yongrong.Model.Int.Org
{
    public class RoleAddReq : BaseInfoReq
    {
        public RoleAddObj Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if (null == Role)
            {
                return "参数为空";
            }

            if (string.IsNullOrWhiteSpace(Role.Roleid))
            {
                return "Roleid empty";
            }

            return "0";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleAddObj
    {
        public string Createtime { get; set; }
        public string Desc { get; set; }

        public string Name { get; set; }

        public string Roleid { get; set; }

        public string Rolename { get; set; }

        public string Tel { get; set; }

        public string Menucode { get; set; }

        public string Menuname { get; set; }

        public string Account { get; set; }

        public string Psd { get; set; }
    }
}
