﻿using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using Yongrong.Model.Db;
using Yongrong.Model.Int;

namespace Yongrong.Srvc.BaseInfo
{
    class SafeCheckSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取车辆安全检查
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(SafeCheck query, PageBean page, out List<SafeCheck> basegoodslist)
        {
            basegoodslist = new List<SafeCheck>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new SafeCheck();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.SafeCheck.Count(a =>
                (string.IsNullOrWhiteSpace(query.Checkman) || query.Checkman.Equals(a.Checkman))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.SafeCheck.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Checkman) || query.Checkman.Equals(a.Checkman))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver)))
                    .OrderByDescending(a => a.Createtime)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 添加和更新 车辆安全检查
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(SafeCheck addupdobj, string op)
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
                    case UPD:
                        var tmp = db.SafeCheck.FirstOrDefault(a => a.Id == addupdobj.Id);
                        if (null == tmp)
                        {
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //addupdobj.Id = GetSeq("SEQ_SAFE_CHECK");

                            db.SafeCheck.Add(addupdobj);
                        }
                        else
                        {
                            addupdobj.Upd(tmp);

                            db.SafeCheck.Update(tmp);
                        }
                        break;
                    case DEL:
                        var tmp1 = db.SafeCheck.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"SafeCheck无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.SafeCheck.Remove(tmp1);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }


        /// <summary>
        /// 得到单个车辆安检表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool GetOne(SafeCheck query, out SafeCheck safecheck)
        {
            safecheck = null;

            if (null == query)
            {
                "查询条件为null,不返回对象".Warn(Exit);
                return false;
            }
            using (var db = DbContext)
            {
                safecheck = db.SafeCheck.FirstOrDefault(a => a.Id == query.Id);
            }

            return null != safecheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orders"></param>
        public static void UpdOrderGoodsSafeCheck(IEnumerable<OrderGoods> orders)
        {
            if (null == orders || 0 == orders.Count())
            {
                return;
            }

            using (var db = DbContext)
            {
                foreach (var o in orders)
                {
                    var c = db.SafeCheck.FirstOrDefault(a => a.Id == o.Id);

                    o.IsSafeCheck = c?.IsAllPass ?? false;
                }
            }
        }

        public static bool UpdOrderGoodSafeCheck(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            decimal safecheckid = Convert.ToDecimal(id);
            using (var db = DbContext)
            {

                var safecheck = db.SafeCheck.FirstOrDefault(a => a.Id == safecheckid);

                if (null != safecheck)
                {
                    if (safecheck.IsAllPass)
                    {
                        return true;

                    }
                }
         

                return false;
            }


        }

    }
}
