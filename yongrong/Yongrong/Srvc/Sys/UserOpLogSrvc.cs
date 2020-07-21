using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using System.Linq;
using BaseS.File.Log;
using Yongrong.Model.Int;

namespace Yongrong.Srvc.Sys
{
    class UserOpLogSrvc: BaseSrvc
    {
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(UserOplog query, PageBean page, out List<UserOplog> baselist)
        {
            baselist = new List<UserOplog>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new UserOplog();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.UserOplog.Count(a =>
                (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                && (string.IsNullOrWhiteSpace(query.UseridDest) || query.UseridDest.Equals(a.UseridDest))
                && (string.IsNullOrWhiteSpace(query.Stat) || query.Stat.Equals(a.Stat))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.UserOplog.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Userid) || query.Userid.Equals(a.Userid))
                    && (string.IsNullOrWhiteSpace(query.UseridDest) || query.UseridDest.Equals(a.UseridDest))
                    && (string.IsNullOrWhiteSpace(query.Stat) || query.Stat.Equals(a.Stat))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 添加和更新删除操作日志
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(UserOplog addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                        addupdobj.Id = GetSeq("SEQ_USER_OPLOG");

                        db.UserOplog.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.UserOplog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"UserOplog Id:{addupdobj.Id} 不存在".Notice();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.UserOplog.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.UserOplog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"UserOplog Id:{addupdobj.Id} 不存在".Notice();
                            return DelNoRecord;
                        }

                        db.UserOplog.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                SaveChanges(db, "UserOpLogSrvc");
            }

            return Success;
        }
    }
}
