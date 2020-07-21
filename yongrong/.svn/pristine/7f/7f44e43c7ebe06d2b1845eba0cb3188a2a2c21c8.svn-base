using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.User
{
    public class UserLoginRsp : BasePageRsp
    {
        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// TOKEN时效
        /// </summary>
        public string Expire { get; set; }

        /// <summary>
        /// 登录者账号
        /// </summary>
        public Yongrong.Model.Db.User User { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Role Role { get; set; }

        public List<MenuBean> Menu { get; set; } = new List<MenuBean>();

        public List<Todolist> TodoList { get; set; } = new List<Todolist>();

        public List<Todolist> DoneList { get; set; } = new List<Todolist>();
    }

    public class MenuBean
    {
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 栏目编号
        /// </summary>
        public string ItemPermission { get; set; }

        /// <summary>
        /// 子栏目
        /// </summary>
        public List<SubItem> SubItems { get; set; }
    }

    public class SubItem
    {
        public string SubItemName { get; set; }

        public string SubItemPermission { get; set; }

        public List<string> P3 { get; set; } = new List<string>();
    }
}
