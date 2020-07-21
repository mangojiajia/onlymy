﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseSupercargoSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取押运员
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BaseSupercargo query, PageBean page, out List<BaseSupercargo> baselist)
        {
            baselist = new List<BaseSupercargo>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseSupercargo();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseSupercargo.Count(a =>
                (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Cardid) || query.Cardid.Equals(a.Cardid))
                && (string.IsNullOrWhiteSpace(query.Driverid) || query.Driverid.Equals(a.Driverid))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BaseSupercargo.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Cardid) || query.Cardid.Equals(a.Cardid))
                && (string.IsNullOrWhiteSpace(query.Driverid) || query.Driverid.Equals(a.Driverid))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport))))
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 添加和更新删除押运员
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseSupercargo addupdobj, string op)
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
                        addupdobj.Name = addupdobj.Name == null ? "" : addupdobj.Name.Trim();
                        addupdobj.Transport = addupdobj.Transport == null ? "" : addupdobj.Transport.Trim();
                        addupdobj.Driver = addupdobj.Driver == null ? "" : addupdobj.Driver.Trim();

                        var old = db.BaseSupercargo.FirstOrDefault(a => addupdobj.Cardid.Equals(a.Cardid) && addupdobj.Transport.Equals(a.Transport));
                        if (null == old)
                        {
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 

                            addupdobj.Id = GetSeq("SEQ_BASE_SUPERCARGO");
                            addupdobj.Whiteflag = "1";

                            db.BaseSupercargo.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseSupercargo Cardid:{addupdobj.Cardid} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        addupdobj.Name = addupdobj.Name == null ? "" : addupdobj.Name.Trim();
                        addupdobj.Transport = addupdobj.Transport == null ? "" : addupdobj.Transport.Trim();
                        addupdobj.Driver = addupdobj.Driver == null ? "" : addupdobj.Driver.Trim();
                        var tmp = db.BaseSupercargo.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseSupercargo Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseSupercargo.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseSupercargo.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BaseSupercargo Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseSupercargo.Remove(tmp1);
                        break;
                    default:
                        "未加操作类型".Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }
    }
}
