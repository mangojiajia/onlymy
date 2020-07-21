using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int.User;

namespace Yongrong.Model.Int.Sys
{
    public class RoleGetRsp : BasePageRsp
    {

        /// <summary>
        /// 
        /// </summary>
        public List<RoleObj> AllRoles { get; set; }

        /// <summary>
        /// 全部权限码集合
        /// </summary>
        public List<Permission> PermissionList { get; set; }

        public List<MenuBean> Menu { get; set; } = new List<MenuBean>();
    }

    public class RoleObj : Role
    {
        public string Account { get; set; }

        public string Desc { get; set; }

        public string Menucode { get; set; }

        public string Menuname { get; set; }

        public string Name { get; set; }

        public string Post { get; set; }

        public string Psd { get; set; }

        public string Tel { get; set; }

    }
}
