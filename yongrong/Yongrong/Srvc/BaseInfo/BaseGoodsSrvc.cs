using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BaseGoodsSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Get(BaseGoods query, PageBean page, out List<BaseGoods> basegoodslist)
        {
            basegoodslist = new List<BaseGoods>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BaseGoods();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BaseGoods.Count(a =>
                (string.IsNullOrWhiteSpace(query.Goodsid) || (!string.IsNullOrWhiteSpace(a.Goodsid) && a.Goodsid.Contains(query.Goodsid)))
                && (string.IsNullOrWhiteSpace(query.Goodsname) || (!string.IsNullOrWhiteSpace(a.Goodsname) && a.Goodsname.Contains(query.Goodsname)))
                );

                page.SumPageCount();

                basegoodslist.AddRange(
                    db.BaseGoods.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Goodsid) || (!string.IsNullOrWhiteSpace(a.Goodsid) && a.Goodsid.Contains(query.Goodsid)))
                && (string.IsNullOrWhiteSpace(query.Goodsname) || (!string.IsNullOrWhiteSpace(a.Goodsname) && a.Goodsname.Contains(query.Goodsname))))
                    .OrderByDescending(a => a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 的添加和更新 删除物料信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BaseGoods goods, string op)
        {
            if (null == goods)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch(op)
                {
                    case ADD:
                        var old = db.BaseGoods.FirstOrDefault(a => goods.Goodsid.Equals(a.Goodsid));

                        if (null == old)
                        {
                            goods.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;

                            goods.Id = GetSeq("SEQ_BASE_GOODS");

                            db.BaseGoods.Add(goods);
                        }
                        else
                        {
                            $"BaseGoods goods.Goodsid:{goods.Goodsid} 已存在".Warn();
                            return AddRecord;
                        }
                        break;
                    case UPD:
                        var dbgoods = db.BaseGoods.FirstOrDefault(a => a.Id == goods.Id);

                        if (null == dbgoods)
                        {
                            $"BaseGoods Id:{goods.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        goods.Upd(dbgoods);

                        db.BaseGoods.Update(dbgoods);
                        break;
                    case DEL:
                        var tmp2 = db.BaseGoods.FirstOrDefault(a => a.Id == goods.Id);

                        if (null == tmp2)
                        {
                            $"BaseGoods Id:{goods.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.BaseGoods.Remove(tmp2);
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
        /// 联想查询物料
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="basegoodslist"></param>
        /// <returns></returns>
        public static bool Search(BaseGoods query, out List<string> basegoodslist)
        {
            basegoodslist = new List<string>();

            if (null == query)
            {
                query = new BaseGoods();
            }

            query.Debug();

            using (var db = DbContext)
            {
                basegoodslist.AddRange(
                    db.BaseGoods.Where(a =>
                     (string.IsNullOrWhiteSpace(query.Goodsname) || (!string.IsNullOrWhiteSpace(a.Goodsname) && a.Goodsname.Contains(query.Goodsname))))
                    .Select(a => a.Goodsname));
            }

            return true;
        }
    }
}
