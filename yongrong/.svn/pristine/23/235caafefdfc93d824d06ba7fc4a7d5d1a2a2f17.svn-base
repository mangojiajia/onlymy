using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using System.Linq;
using Yongrong.Model.Int;
using BaseS.Security;
using Yongrong.Db;
using BaseS.File.Log;

namespace Yongrong.Srvc.Users
{
    class RoleSrvc : BaseSrvc
    {
        /// <summary>
        /// 查询部门
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(Role query, PageBean page, out List<Role> basegoodslist)
        {
            basegoodslist = new List<Role>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new Role();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.Role.Count(a =>
                (string.IsNullOrWhiteSpace(query.Roleid) || query.Roleid.Equals(a.Roleid))
                && (string.IsNullOrWhiteSpace(query.Rolename) || query.Rolename.Equals(a.Rolename))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.Role.Where(a =>
                   (string.IsNullOrWhiteSpace(query.Roleid) || query.Roleid.Equals(a.Roleid))
                   && (string.IsNullOrWhiteSpace(query.Rolename) || query.Rolename.Equals(a.Rolename))
                   )
                    .OrderBy(a => a.Userid)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 添加和更新部门
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(Role addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            if (string.IsNullOrWhiteSpace(addupdobj.Roleid))
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                        db.Role.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.Role.FirstOrDefault(a => addupdobj.Roleid.Equals(a.Roleid));

                        if (null == tmp)
                        {
                            $"Role Id:{addupdobj.Roleid} 不存在".Notice();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.Role.Update(tmp);
                        break;
                    case DEL:

                        var tmp1 = db.Role.FirstOrDefault(a => addupdobj.Roleid.Equals(a.Roleid));

                        if (null == tmp1)
                        {
                            $"Role :{addupdobj.Roleid} 不存在".Notice();
                            return DelNoRecord;
                        }

                        db.Role.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                SaveChanges(db, "UserPermission");
            }

            return Success;
        }
    }
}
