﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseTrailerSrvc:BaseSrvc
    {
        /// <summary>
        /// 挂车查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baseTrailerslist"></param>
        /// <returns></returns>
        public static bool Get(BaseTrailer query, PageBean page, out List<BaseTrailer>  baseTrailerslist)
        {
            baseTrailerslist = new List<BaseTrailer>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseTrailer();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseTrailer.Count(a =>
                (string.IsNullOrWhiteSpace(query.TrailerId) || query.TrailerId.Equals(a.TrailerId))
                && (string.IsNullOrWhiteSpace(query.Region) || (!string.IsNullOrWhiteSpace(a.Region) && a.Region.Contains(query.Region)))
                && (string.IsNullOrWhiteSpace(query.Degree) || (!string.IsNullOrWhiteSpace(a.Degree) && a.Degree.Contains(query.Degree)))
                && (string.IsNullOrWhiteSpace(query.Carid) || query.Carid.Equals(a.Carid))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                );

                page.SumPageCount();

                baseTrailerslist.AddRange(
                    db.BaseTrailer. Where(a =>
                    (string.IsNullOrWhiteSpace(query.TrailerId) || query.TrailerId.Equals(a.TrailerId))
                && (string.IsNullOrWhiteSpace(query.Region) || (!string.IsNullOrWhiteSpace(a.Region) && a.Region.Contains(query.Region)))
                && (string.IsNullOrWhiteSpace(query.Degree) || (!string.IsNullOrWhiteSpace(a.Degree) && a.Degree.Contains(query.Degree)))
                && (string.IsNullOrWhiteSpace(query.Carid) || query.Carid.Equals(a.Carid))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }
            return true;
        }


        /// <summary>
        /// 挂车的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseTrailer  baseTrailer, string op)
        {
            if (null == baseTrailer)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        baseTrailer.TrailerId = baseTrailer.TrailerId == null ? "" : baseTrailer.TrailerId.Trim().ToUpper();
                        baseTrailer.Transport = baseTrailer.Transport == null ? "" : baseTrailer.Transport.Trim();

                        var old = db.BaseTrailer.FirstOrDefault(a => baseTrailer.TrailerId.Equals(a.TrailerId) && (!string.IsNullOrWhiteSpace(baseTrailer.Transport) && baseTrailer.Transport.Equals(a.Transport)));
                        if(null == old)
                        { 
                            baseTrailer.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            baseTrailer.Id = GetSeq("SEQ_BASE_TRAILER");

                            db.BaseTrailer.Add(baseTrailer);
                        }
                        else
                        {
                            $"BaseTrailer TrailerId:{baseTrailer.TrailerId} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        baseTrailer.TrailerId = baseTrailer.TrailerId == null ? "" : baseTrailer.TrailerId.Trim();
                        baseTrailer.Transport = baseTrailer.Transport == null ? "" : baseTrailer.Transport.Trim();
                        var dbbaseTrailer = db.BaseTrailer.FirstOrDefault(a => a.Id == baseTrailer.Id);

                        if (null == dbbaseTrailer)
                        {
                            $"BaseTrailer Id:{baseTrailer.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        baseTrailer.Upd(dbbaseTrailer);

                        db.BaseTrailer.Update(dbbaseTrailer);
                        break;
                    case DEL:
                        var tmp2 = db.BaseTrailer.FirstOrDefault(a => a.Id == baseTrailer.Id);

                        if (null == tmp2)
                        {
                            $"BaseTrailer Id:{baseTrailer.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseTrailer.Remove(tmp2);
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
