using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseSupplierSrvc:BaseSrvc
    {
        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BaseSupplier query, PageBean page, out List<BaseSupplier> baselist)
        {
            baselist = new List<BaseSupplier>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseSupplier();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseSupplier.Count(a =>
                (string.IsNullOrWhiteSpace(query.Enterprisename) || (!string.IsNullOrWhiteSpace(a.Enterprisename) && a.Enterprisename.Contains(query.Enterprisename)))
                && (string.IsNullOrWhiteSpace(query.Enterpriseid) || query.Enterpriseid.Equals(a.Enterpriseid))
                && (string.IsNullOrWhiteSpace(query.Creditid) || query.Creditid.Equals(a.Creditid))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BaseSupplier.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Enterprisename) || (!string.IsNullOrWhiteSpace(a.Enterprisename) && a.Enterprisename.Contains(query.Enterprisename)))
                    && (string.IsNullOrWhiteSpace(query.Enterpriseid) || query.Enterpriseid.Equals(a.Enterpriseid))
                    && (string.IsNullOrWhiteSpace(query.Creditid) || query.Creditid.Equals(a.Creditid)))
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 供应商的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseSupplier addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch(op)
                {
                    case ADD:
                        addupdobj.Enterprisename = addupdobj.Enterprisename == null ? "" : addupdobj.Enterprisename.Trim();
                        addupdobj.Creditid = addupdobj.Creditid == null ? "" : addupdobj.Creditid.Trim();

                        var old = db.BaseSupplier.FirstOrDefault(a => addupdobj.Enterprisename.Equals(a.Enterprisename));
                        if(null == old)
                        {
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                            addupdobj.Id = GetSeq("SEQ_BASE_SUPPLIER");

                            db.BaseSupplier.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseSupplier Enterprisename:{addupdobj.Enterprisename} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        addupdobj.Enterprisename = addupdobj.Enterprisename == null ? "" : addupdobj.Enterprisename.Trim();
                        addupdobj.Creditid = addupdobj.Creditid == null ? "" : addupdobj.Creditid.Trim();

                        var tmp = db.BaseSupplier.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseSupplier Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseSupplier.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseSupplier.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BaseSupplier Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseSupplier.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }

        /// <summary>
        /// 联想获取供应商
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Search(BaseSupplier query, out List<string> baselist)
        {
            baselist = new List<string>();


            query.Debug();

            using (var db = DbContext)
            {
                
                baselist.AddRange(
                    db.BaseSupplier.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Enterprisename) || (!string.IsNullOrWhiteSpace(a.Enterprisename) && a.Enterprisename.Contains(query.Enterprisename)))
                    ).Select(a => a.Enterprisename)
                );
            }

            return true;
        }
    }
}
