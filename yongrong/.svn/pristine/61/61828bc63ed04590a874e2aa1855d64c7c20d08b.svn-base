using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;
using System.Collections.Concurrent;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseTractorSrvc:BaseSrvc
    {
        /// <summary>
        /// 牵引车表
        /// key:车牌号
        /// </summary>
        static readonly ConcurrentDictionary<string, object> tractorTab
            = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 初始化牵引车牌
        /// </summary>
        public static void Init()
        {
            GetAllTractorIds(out var tratorids);

            foreach (var tractorid in tratorids)
            {
                if (!string.IsNullOrWhiteSpace(tractorid))
                {
                    tractorTab.TryAdd(tractorid, 0);
                }
               
            }
        }

        /// <summary>
        /// 是否包含车牌
        /// </summary>
        /// <param name="tractorid"></param>
        /// <returns></returns>
        public static bool Contains(string tractorid)
        {
            if (string.IsNullOrWhiteSpace(tractorid))
            {
                return false;
            }

            return tractorTab.ContainsKey(tractorid);
        }

        /// <summary>
        /// 牵引车查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baseTrailerslist"></param>
        /// <returns></returns>
        public static bool Get(BaseTractor query, PageBean page, out List<BaseTractor> baseTrailerslist)
        {
            baseTrailerslist = new List<BaseTractor>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseTractor();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseTractor.Count(a =>
                (string.IsNullOrWhiteSpace(query.Carid) || (!string.IsNullOrWhiteSpace(a.Carid) && a.Carid.Contains(query.Carid)))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                );

                page.SumPageCount();

                baseTrailerslist.AddRange(
                    db.BaseTractor.Where(a =>
                   (string.IsNullOrWhiteSpace(query.Carid) || (!string.IsNullOrWhiteSpace(a.Carid) && a.Carid.Contains(query.Carid)))
                && (string.IsNullOrWhiteSpace(query.TractorId) || query.TractorId.Equals(a.TractorId))
                && (string.IsNullOrWhiteSpace(query.Transport) || (!string.IsNullOrWhiteSpace(a.Transport) && a.Transport.Contains(query.Transport)))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }
            return true;
        }

        /// <summary>
        /// 获取所有牵引车
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baseTrailerslist"></param>
        /// <returns></returns>
        public static bool GetAllTractorIds(out List<string> tractorids)
        {
            tractorids = new List<string>();

            using (var db = DbContext)
            {
                tractorids.AddRange(db.BaseTractor.Select(a => a.TractorId));
            }

            $"获取到所有牵引车:{tractorids.Count}个".Info();

            return true;
        }

        /// <summary>
        /// 牵引车的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseTractor baseTractor, string op)
        {
            if (null == baseTractor)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        baseTractor.TractorId = baseTractor.TractorId == null ? "" : baseTractor.TractorId.Trim().ToUpper();
                        baseTractor.Transport = baseTractor.Transport == null ? "" : baseTractor.Transport.Trim();

                        var old = db.BaseTractor.FirstOrDefault(a => baseTractor.TractorId.Equals(a.TractorId) && baseTractor.Transport.Equals(a.Transport));
                        if (null == old)
                        {
                            baseTractor.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                            baseTractor.Id = GetSeq("SEQ_BASE_TRACTOR");

                            tractorTab.TryAdd(baseTractor.TractorId, 0);

                            db.BaseTractor.Add(baseTractor);
                        }
                        else
                        {
                            $"BaseTractor TractorId:{baseTractor.TractorId} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        baseTractor.TractorId = baseTractor.TractorId == null ? "" : baseTractor.TractorId.Trim();
                        baseTractor.Transport = baseTractor.Transport == null ? "" : baseTractor.Transport.Trim();
                        var tmp = db.BaseTractor.FirstOrDefault(a => a.Id == baseTractor.Id);

                        if (null == tmp)
                        {
                            $"BaseTractor Id:{baseTractor.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        baseTractor.Upd(tmp);

                        tractorTab.TryAdd(tmp.TractorId, 0);

                        db.BaseTractor.Update(tmp);
                        break;
                    case DEL:
                        var tmp2 = db.BaseTractor.FirstOrDefault(a => a.Id == baseTractor.Id);

                        if (null == tmp2)
                        {
                            $"BaseTractor Id:{baseTractor.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseTractor.Remove(tmp2);
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
