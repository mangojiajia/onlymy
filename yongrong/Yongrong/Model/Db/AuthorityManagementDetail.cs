using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    /// <summary>
    /// 权限管理明细表
    /// </summary>
    public partial class AuthorityManagementDetail
    {
        /// <summary>
        /// 权限管理子表Id
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Menucode { get; set; }
        /// <summary>
        /// 是否有新增权限 "yes","no" 表示
        /// </summary>
        public string Isadd { get; set; }
        /// <summary>
        /// 是否有编辑权限 "yes","no" 表示
        /// </summary>
        public string Isedit { get; set; }
        /// <summary>
        /// 是否有查看权限 "yes","no" 表示
        /// </summary>
        public string Isview { get; set; }
       

        public void Upd(AuthorityManagementDetail old)
        {
            old.Menucode = this.Menucode;
            old.Isadd = this.Isadd;
            old.Isedit = this.Isedit;
            old.Isview = this.Isview;
        }
    }
}
