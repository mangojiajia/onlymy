using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class AuthorityManagement
    {
        /// <summary>
        /// 权限管理Id
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Departmentname { get; set; }
        /// <summary>
        /// 管理员名称
        /// </summary>
        public string Adminname { get; set; }
        /// <summary>
        /// 管理权限(多个字段逗号分隔)与这个名字的Detil表关联
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pass { get; set; }
        /// <summary>
        /// 是否启用 "enable" 启用 "disable" 禁用
        /// </summary>
        public string Isenable { get; set; }
        public string Createtime { get; set; }



        #region 管理权限字段(子表里面)
        [NotMapped]
        public string Permissionname { get; set; }
        #endregion

        public void Upd(AuthorityManagement old)
        {
            old.Departmentname = this.Departmentname;
            old.Adminname = this.Adminname;
            old.Permission = this.Permission;
            old.Tel = this.Tel;
            old.Account = this.Account;
            old.Pass = this.Pass;
            old.Isenable = this.Isenable;
            old.Createtime = this.Createtime;
        }
    }
}
