﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BaseS.File.Log;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Runtime.CompilerServices;

namespace Yongrong.Srvc.BaseInfo
{
    /// <summary>
    /// 司机管理服务
    /// </summary>
    class BaseDriverSrvc: BaseSrvc
    {
        /// <summary>
        /// 获取驾驶员
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BaseDriver query, PageBean page, out List<BaseDriver> baselist)
        {
            baselist = new List<BaseDriver>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseDriver();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseDriver.Count(a =>
                (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Cardid) || query.Cardid.Equals(a.Cardid))
                && (string.IsNullOrWhiteSpace(query.Driverid) || query.Cardid.Equals(a.Driverid))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                && (string.IsNullOrWhiteSpace(query.Tel) || query.Tel.Equals(a.Tel))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BaseDriver.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Name) || (!string.IsNullOrWhiteSpace(a.Name) && a.Name.Contains(query.Name)))
                && (string.IsNullOrWhiteSpace(query.Cardid) || query.Cardid.Equals(a.Cardid))
                && (string.IsNullOrWhiteSpace(query.Driverid) || query.Cardid.Equals(a.Driverid))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                && (string.IsNullOrWhiteSpace(query.Tel) || query.Tel.Equals(a.Tel))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 添加和更新删除驾驶员
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseDriver addupdobj, string op)
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
                        addupdobj.Tel = addupdobj.Tel == null ? "" : addupdobj.Tel.Trim();
                        addupdobj.TractorId = addupdobj.TractorId == null ? "" : addupdobj.TractorId.Trim().ToUpper();
                        addupdobj.TrailerId = addupdobj.TrailerId == null ? "" : addupdobj.TrailerId.Trim().ToUpper();
                        addupdobj.Transport = addupdobj.Transport == null ? "" : addupdobj.Transport.Trim();
                        var old = db.BaseDriver.FirstOrDefault(a => addupdobj.Tel.Equals(a.Tel) && addupdobj.Transport.Equals(a.Transport));
                        if(null == old)
                        { 
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                            addupdobj.Id = GetSeq("SEQ_BASE_DRIVER");
                            addupdobj.Whiteflag = "1";

                            db.BaseDriver.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseDriver Tel:{addupdobj.Tel},Transport:{addupdobj.Transport} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        addupdobj.Name = addupdobj.Name == null ? "" : addupdobj.Name.Trim();
                        addupdobj.Tel = addupdobj.Tel == null ? "" : addupdobj.Tel.Trim();
                        addupdobj.TractorId = addupdobj.TractorId == null ? "" : addupdobj.TractorId.Trim();
                        addupdobj.TrailerId = addupdobj.TrailerId == null ? "" : addupdobj.TrailerId.Trim();
                        addupdobj.Transport = addupdobj.Transport == null ? "" : addupdobj.Transport.Trim();

                        var tmp = db.BaseDriver.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseDriver Id:{addupdobj.Id} 不存在".Notice();
                            return UpdNoRecord;
                        }

                        var tmp2 = db.BaseDriver.FirstOrDefault(a => addupdobj.Tel.Equals(a.Tel) && addupdobj.Transport.Equals(a.Transport) && a.Id != addupdobj.Id);

                        if(null != tmp2)
                        {
                            $"BaseDriver Tel:{addupdobj.Tel} 已经存在".Notice();
                            return AddRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseDriver.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseDriver.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BaseDriver Id:{addupdobj.Id} 不存在".Notice();
                            return DelNoRecord;
                        }

                        db.BaseDriver.Remove(tmp1);
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
