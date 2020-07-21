using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.Sys
{
    class RolePermissionSrvc : BaseSrvc
    {
        /// <summary>
        /// 查询部门权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="allrolepermission"></param>
        /// <returns></returns>
        public static bool Get(RolePermission query, out List<RolePermission> allrolepermission)
        {
            allrolepermission = new List<RolePermission>();

            if (null == query)
            {
                query = new RolePermission();
            }
            
            query.Debug();

            using (var db = DbContext)
            {
                allrolepermission.AddRange(
                    db.RolePermission.Where(a =>
                   (string.IsNullOrWhiteSpace(query.Roleid) || query.Roleid.Equals(a.Roleid))
                   ));
            }

            return true;
        }

        /// <summary>
        /// 添加和更新部门权限
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(List<RolePermission> addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                List<RolePermission> rmlist = new List<RolePermission>();

                var roleid = addupdobj.FirstOrDefault()?.Roleid;

                if (!string.IsNullOrWhiteSpace(roleid))
                {
                    rmlist.AddRange(db.RolePermission.Where(a => roleid.Equals(a.Roleid)));

                    if (1 <= rmlist.Count)
                    {
                        db.RolePermission.RemoveRange(rmlist);

                        SaveChanges(db, "RolePermission");
                    }
                }

                if (!DEL.Equals(op))
                {
                    List<RolePermission> addlist = new List<RolePermission>();

                    foreach (var rp in addupdobj)
                    {
                        if (addlist.Any(a => a.Roleid.Equals(rp.Roleid) && a.Permissionid.Equals(rp.Permissionid)))
                        {
                            continue;
                        }

                        rp.Createtime = DateTime.Now;
                        rp.Id = GetSeq("SEQ_ROLE_PERMISSION");

                        addlist.Add(rp);
                    }

                    db.RolePermission.AddRange(addlist);

                    SaveChanges(db, "RolePermission add");
                }
            }

            return Success;
        }
    }
}
