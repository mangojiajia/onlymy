﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;
using Yongrong.Db;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseCustomSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BaseCustomer query, PageBean page, out List<BaseCustomer> baselist)
        {
            baselist = new List<BaseCustomer>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseCustomer();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseCustomer.Count(a =>
                (string.IsNullOrWhiteSpace(query.Enterprisename) || (!string.IsNullOrWhiteSpace(a.Enterprisename) && a.Enterprisename.Contains(query.Enterprisename)))
                && (string.IsNullOrWhiteSpace(query.Enterpriseid) || query.Enterpriseid.Equals(a.Enterpriseid))
                && (string.IsNullOrWhiteSpace(query.Creditid) || query.Creditid.Equals(a.Creditid))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BaseCustomer.Where(a =>
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
        /// 添加和更新客户信息
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseCustomer addupdobj, string op)
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
                        addupdobj.Enterprisename = addupdobj.Enterprisename == null ? "" : addupdobj.Enterprisename.Trim();
                        addupdobj.Creditid = addupdobj.Creditid == null ? "" : addupdobj.Creditid.Trim();

                        var old = db.BaseCustomer.FirstOrDefault(a => addupdobj.Enterprisename.Equals(a.Enterprisename));
                        if (null == old)
                        {
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            addupdobj.Id = GetSeq("SEQ_BASE_CUSTOMER");

                            db.BaseCustomer.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseCustomer Enterprisename:{addupdobj.Enterprisename} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        addupdobj.Enterprisename = addupdobj.Enterprisename == null ? "" : addupdobj.Enterprisename.Trim();
                        addupdobj.Creditid = addupdobj.Creditid == null ? "" : addupdobj.Creditid.Trim();

                        var tmp = db.BaseCustomer.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseCustomer无法找到{addupdobj.Id}".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseCustomer.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseCustomer.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if(null == tmp1)
                        {
                            $"BaseCustomer无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.BaseCustomer.Remove(tmp1);
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
        /// 获取客户信息（客户名联想）
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Search(BaseCustomer query, out List<string> baselist)
        {
            baselist = new List<string>();

            query.Debug();

            using (var db = DbContext)
            {
                baselist.AddRange(
                    db.BaseCustomer.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Enterprisename) || (!string.IsNullOrWhiteSpace(a.Enterprisename) && a.Enterprisename.Contains(query.Enterprisename)))
                    ).Select(a => a.Enterprisename));
            }

            return true;
        }
    }
}
