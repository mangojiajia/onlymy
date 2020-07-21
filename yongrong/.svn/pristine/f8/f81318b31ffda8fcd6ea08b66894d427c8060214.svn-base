using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;
using BaseS.Const;
using Yongrong.Model.Int.Pda;

namespace Yongrong.Srvc.BaseInfo
{
    class OrderConfigSrvc: BaseSrvc
    {
        /// <summary>
        /// 0501 查询 储运调度管理-预约后台管理
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(OrderConfig query, PageBean page, out List<OrderConfig> basegoodslist)
        {
            basegoodslist = new List<OrderConfig>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new OrderConfig();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.OrderConfig.Count(a =>
                (string.IsNullOrWhiteSpace(query.Configtype) || (!string.IsNullOrWhiteSpace(a.Configtype) && a.Configtype.Contains(query.Configtype)))
             && (string.IsNullOrWhiteSpace(query.Loadarea) || (!string.IsNullOrWhiteSpace(a.Loadarea) && a.Loadarea.Contains(query.Loadarea)))
             && (string.IsNullOrWhiteSpace(query.Unloadarea) || (!string.IsNullOrWhiteSpace(a.Unloadarea) && a.Unloadarea.Contains(query.Unloadarea)))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.OrderConfig.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Configtype) || (!string.IsNullOrWhiteSpace(a.Configtype) && a.Configtype.Contains(query.Configtype)))
             && (string.IsNullOrWhiteSpace(query.Loadarea) || (!string.IsNullOrWhiteSpace(a.Loadarea) && a.Loadarea.Contains(query.Loadarea)))
             && (string.IsNullOrWhiteSpace(query.Unloadarea) || (!string.IsNullOrWhiteSpace(a.Unloadarea) && a.Unloadarea.Contains(query.Unloadarea)))
                    )
                    .OrderByDescending(a => a.Id)
                    //.GroupBy(a => a.Ordertime)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }


        /// <summary>
        /// 0501 查询 储运调度管理-预约后台管理
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get1(OrderConfig query, PageBean page, out List<OrderConfig> basegoodslist)
        {
            basegoodslist = new List<OrderConfig>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new OrderConfig();
            }

            using (var db = DbContext)
            {
                page.Row = db.OrderConfig.Count(a =>
                (string.IsNullOrWhiteSpace(query.Configtype) || query.Configtype.Equals(a.Configtype))
             && (string.IsNullOrWhiteSpace(query.Loadarea) || query.Loadarea.Equals(a.Loadarea))
             && (string.IsNullOrWhiteSpace(query.Unloadarea) || query.Unloadarea.Equals(a.Unloadarea))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.OrderConfig.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Configtype) || query.Configtype.Equals(a.Configtype))
                    && (string.IsNullOrWhiteSpace(query.Loadarea) || query.Loadarea.Equals(a.Loadarea))
                    && (string.IsNullOrWhiteSpace(query.Unloadarea) || query.Unloadarea.Equals(a.Unloadarea))
                    )
                    .OrderBy(a => a.Id)
                    //.GroupBy(a => a.Ordertime)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 添加和更新 储运调度管理-预约后台管理
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(OrderConfig addupdobj, string op)
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
                        var old = db.OrderConfig.FirstOrDefault(a => "物流环节".Equals(a.Configtype));

                        if ("物流环节".Equals(addupdobj.Configtype) && null != old)
                        {
                            addupdobj.Upd(old);
                            db.OrderConfig.Update(old);
                        }
                        else
                        {
                            addupdobj.Id = GetSeq("SEQ_ORDER_CONFIG");

                            addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            db.OrderConfig.Add(addupdobj);
                        }
                        break;
                    case UPD:
                        var tmp = db.OrderConfig.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"OrderConfig无法找到{addupdobj.Id}".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.OrderConfig.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.OrderConfig.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"OrderConfig无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.OrderConfig.Remove(tmp1);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }

        public static int GetMaxNumber(OrderConfig query)
        {
            if(null == query)
            {
                return 0;
            }
            query.Info("查询调度默认配置");
            OrderConfig orderConfig = null;
            using (var db = DbContext)
            {
                string date = DateTime.Now.ToString(BTip.DateFormater);
                orderConfig = db.OrderConfig.FirstOrDefault(a => a.Ordertime.StartsWith(date)
                && a.Configtype.Equals("发货区") && a.Goodsname.Equals(query.Goodsname)
                && (string.IsNullOrWhiteSpace(query.Loadarea) || query.Loadarea.Equals(a.Loadarea))
                && (string.IsNullOrWhiteSpace(query.Unloadarea) || query.Unloadarea.Equals(a.Unloadarea)));
                if(null != orderConfig)
                {
                    return Decimal.ToInt32(orderConfig.Maxcarnumber ?? 0);
                }
                orderConfig = db.OrderConfig.FirstOrDefault(a=>a.Configtype.Equals("后台") && a.Goodsname.Equals(query.Goodsname)
                && (string.IsNullOrWhiteSpace(query.Loadarea) || query.Loadarea.Equals(a.Loadarea))
                && (string.IsNullOrWhiteSpace(query.Unloadarea) || query.Unloadarea.Equals(a.Unloadarea)));
                if(null != orderConfig)
                {
                    return Decimal.ToInt32(orderConfig.Maxcarnumber ?? 0);
                }
            }
            return int.MaxValue;
        }
    }
}
