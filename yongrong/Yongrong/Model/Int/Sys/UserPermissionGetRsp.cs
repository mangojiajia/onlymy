using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using static Yongrong.Model.Int.Org.PermissionRsp;

namespace Yongrong.Model.Int.Org
{
    public class UserPermissionGetRsp: BasePageRsp
    {
        /// <summary>
        /// 部门名称集合
        /// </summary>
        public List<string> RoleNames { get; set; } = new List<string>();

        /// <summary>
        /// 当前部门
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 第一个部门或者查询部门,部门下用户及用户对应的权限集合
        /// </summary>
        public List<UserPermissionObj> Users { get; set; } = new List<UserPermissionObj>();

        /// <summary>
        /// 部门权限范围
        /// </summary>
        public List<PagePermission> RolePs { get; set; } = new List<PagePermission>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class PagePermission
    {
        public string P2name { get; set; }

        public List<string> P3 { get; set; } = new List<string>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserPermissionObj 
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        public List<PagePermission> UserPs { get; set; }
    }
}
