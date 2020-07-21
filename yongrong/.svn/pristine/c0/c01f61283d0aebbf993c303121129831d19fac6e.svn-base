using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseLoadingplaceSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取装卸区
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(BaseLoadingplace query, PageBean page, out List<BaseLoadingplace> basegoodslist)
        {
            basegoodslist = new List<BaseLoadingplace>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseLoadingplace();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseLoadingplace.Count(a =>
                (string.IsNullOrWhiteSpace(query.Placeid) || (!string.IsNullOrWhiteSpace(a.Placeid) && a.Placeid.Contains(query.Placeid)))
                && (string.IsNullOrWhiteSpace(query.Placename) || (!string.IsNullOrWhiteSpace(a.Placename) && a.Placename.Contains(query.Placename)))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.BaseLoadingplace.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Placeid) || (!string.IsNullOrWhiteSpace(a.Placeid) && a.Placeid.Contains(query.Placeid)))
                && (string.IsNullOrWhiteSpace(query.Placename) || (!string.IsNullOrWhiteSpace(a.Placename) && a.Placename.Contains(query.Placename)))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 装卸区的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseLoadingplace loadingplace, string op)
        {
            if (null == loadingplace)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch(op)
                {
                    case ADD:
                        var old = db.BaseLoadingplace.FirstOrDefault(a => loadingplace.Placeid.Equals(a.Placeid));
                        if(null == old)
                        {                        
                            loadingplace.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
                            loadingplace.Id = GetSeq("SEQ_BASE_LOADINGPLACE");
                            db.BaseLoadingplace.Add(loadingplace);
                        }
                        else
                        {
                            $"BaseLoadingplace Placeid:{loadingplace.Placeid} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        var tmp = db.BaseLoadingplace.FirstOrDefault(a => a.Id == loadingplace.Id);

                        if (null == tmp)
                        {
                            $"BaseLoadingplace Id:{loadingplace.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        loadingplace.Upd(tmp);

                        db.BaseLoadingplace.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseLoadingplace.FirstOrDefault(a => a.Id == loadingplace.Id);

                        if (null == tmp1)
                        {
                            $"BaseLoadingplace Id:{loadingplace.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseLoadingplace.Remove(tmp1);
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
