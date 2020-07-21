using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using BaseS.File.Log;
using System.Linq;
using Yongrong.Db;
using System.Data;
using BaseS.Security;

namespace Yongrong.Srvc.Users
{
    class UserPermissionSrvc : BaseSrvc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="userPermissions"></param>
        /// <returns></returns>
        public static bool Get(UserPermission query, out List<UserPermission> userPermissions)
        {
            userPermissions = new List<UserPermission>();

            if (null == query)
            {
                "需要获取参数为空".Notice();
                return false;
            }

            query.Debug();

            using (var db = DbContext)
            {
                userPermissions.AddRange(db.UserPermission.Where(a =>
               (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))));
            }

            return true;
        }

        /// <summary>
        /// 添加更新删除 用户权限
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(List<UserPermission> addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                List<UserPermission> uplist = new List<UserPermission>();

                var userid = addupdobj.FirstOrDefault()?.Userid;

                if (!string.IsNullOrWhiteSpace(userid))
                {
                    uplist.AddRange(db.UserPermission.Where(a => userid.Equals(a.Userid)));

                    if (1 <= uplist.Count)
                    {
                        db.UserPermission.RemoveRange(uplist);

                        SaveChanges(db, "UserPermission");
                    }
                }

                if (!DEL.Equals(op))
                {
                    List<UserPermission> addlist = new List<UserPermission>();

                    foreach (var rp in addupdobj)
                    {
                        if (addlist.Any(a => a.Userid.Equals(rp.Userid) && a.Permissionid.Equals(rp.Permissionid)))
                        {
                            continue;
                        }

                        rp.Createtime = DateTime.Now.ToString();
                        rp.Id = GetSeq("SEQ_USER_PERMISSION");

                        addlist.Add(rp);
                    }

                    db.UserPermission.AddRange(addlist);

                    SaveChanges(db, "UserPermission add");
                }
            }

            return Success;
        }

    }
}
