using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Org
{
    public class PermissionRsp: BasePageRsp
    {
        public List<Permission> List { get; set; }

        public List<MenuBean> Menu { get; set; } = new List<MenuBean>();
        public List<UserPermission> UserPermissionList { get; set; }


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
        }
    }
}
