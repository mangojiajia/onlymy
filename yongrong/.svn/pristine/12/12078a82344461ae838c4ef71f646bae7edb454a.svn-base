using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Srvc.Users
{
    class PermissionSrvc : BaseSrvc
    {
        /// <summary>
        /// 查询基础权限码
        /// </summary>
        /// <param name="query"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static bool Get(Permission query, out List<Permission> permissions)
        {
            if (null == query)
            {
                query = new Permission();
            }

            permissions = new List<Permission>();

            using (var db = DbContext)
            {
                permissions.AddRange(db.Permission);
            }

            return true;
        }
        
    }
}
