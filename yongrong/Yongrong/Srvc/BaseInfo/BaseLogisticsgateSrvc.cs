using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseLogisticsgateSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BaseLogisticsgate query, PageBean page, out List<BaseLogisticsgate> baselist)
        {
            baselist = new List<BaseLogisticsgate>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseLogisticsgate();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseLogisticsgate.Count(a =>
                (string.IsNullOrWhiteSpace(query.Gateid) || (!string.IsNullOrWhiteSpace(a.Gateid) && a.Gateid.Contains(query.Gateid)))
                && (string.IsNullOrWhiteSpace(query.Gatename) || (!string.IsNullOrWhiteSpace(a.Gatename) && a.Gatename.Equals(query.Gatename)))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BaseLogisticsgate.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Gateid) || (!string.IsNullOrWhiteSpace(a.Gateid) && a.Gateid.Contains(query.Gateid)))
                && (string.IsNullOrWhiteSpace(query.Gatename) || (!string.IsNullOrWhiteSpace(a.Gatename) && a.Gatename.Equals(query.Gatename)))
                    )
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 物流门的添加和更新
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseLogisticsgate addupdobj, string op)
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
                        var old = db.BaseLogisticsgate.FirstOrDefault(a => addupdobj.Gateid.Equals(a.Gateid));
                        if(null == old)
                        { 
                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                            addupdobj.Id = GetSeq("SEQ_BASE_LOGISTICSGATE");

                            db.BaseLogisticsgate.Add(addupdobj);
                        }
                        else
                        {
                            $"BaseLogisticsgate Gateid:{addupdobj.Gateid} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        var tmp = db.BaseLogisticsgate.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BaseLogisticsgate Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BaseLogisticsgate.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BaseLogisticsgate.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BaseLogisticsgate Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseLogisticsgate.Remove(tmp1);
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
