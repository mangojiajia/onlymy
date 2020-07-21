using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;

namespace Yongrong.Srvc.BaseInfo
{
    class BillGoodsRefundSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取原辅料退货计划总单管理
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BillGoodsRefund query, PageBean page, out List<BillGoodsRefund> baselist)
        {
            baselist = new List<BillGoodsRefund>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BillGoodsRefund();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BillGoodsRefund.Count(a =>
                (string.IsNullOrWhiteSpace(query.Billid) || (!string.IsNullOrWhiteSpace(a.Billid) && a.Billid.Contains(query.Billid)))
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Billstat) || query.Billstat.Equals(a.Billstat))
                && (string.IsNullOrWhiteSpace(query.Maker) || (!string.IsNullOrWhiteSpace(a.Maker) && a.Maker.Contains(query.Maker)))
                && (string.IsNullOrWhiteSpace(query.Supercargo) || (!string.IsNullOrWhiteSpace(a.Supercargo) && a.Supercargo.Contains(query.Supercargo)))
                && (string.IsNullOrWhiteSpace(query.Tractorids) || (!string.IsNullOrWhiteSpace(query.Tractorids) && a.Tractorids.Contains(query.Tractorids)))
                && (string.IsNullOrWhiteSpace(query.Drivers) || (!string.IsNullOrWhiteSpace(query.Drivers) && a.Drivers.Contains(query.Drivers)))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BillGoodsRefund.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Billid) || (!string.IsNullOrWhiteSpace(a.Billid) && a.Billid.Contains(query.Billid)))
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Billstat) || query.Billstat.Equals(a.Billstat))
                && (string.IsNullOrWhiteSpace(query.Maker) || (!string.IsNullOrWhiteSpace(a.Maker) && a.Maker.Contains(query.Maker)))
                && (string.IsNullOrWhiteSpace(query.Supercargo) || (!string.IsNullOrWhiteSpace(a.Supercargo) && a.Supercargo.Contains(query.Supercargo)))
                && (string.IsNullOrWhiteSpace(query.Tractorids) || (!string.IsNullOrWhiteSpace(query.Tractorids) && a.Tractorids.Contains(query.Tractorids)))
                && (string.IsNullOrWhiteSpace(query.Drivers) || (!string.IsNullOrWhiteSpace(query.Drivers) && a.Drivers.Contains(query.Drivers)))
                    ).OrderByDescending(a => a.Id)
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
        public static string AddOrUpdate(BillGoodsRefund addupdobj, string op)
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
                        var bill = db.BillGoodsRefund.FirstOrDefault(a => addupdobj.Billid.Equals(a.Billid));
                        if (null != bill)
                        {
                            $"BillGoodsRefund对应的Billid：{addupdobj.Billid}已经存在".Warn();
                            return AddRecord;
                        }
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        addupdobj.Id = GetSeq("SEQ_GOODSREFUND");
                        addupdobj.Levelnumber = addupdobj.Goodsnumber;

                        db.BillGoodsRefund.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.BillGoodsRefund.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BillGoodsRefund无法找到{addupdobj.Id}".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BillGoodsRefund.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BillGoodsRefund.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BillGoodsRefund无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.BillGoodsRefund.Remove(tmp1);
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
        /// 根据总单号获取总单信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="billGoodsIn"></param>
        /// <returns></returns>
        public static bool GetOne(BillGoodsRefund query, out BillGoodsRefund billGoodsRefund)
        {
            billGoodsRefund = null;
            if (null == query)
            {
                "查询条件为null,不返回对象".Warn(Exit);
                return false;
            }
            if (string.IsNullOrEmpty(query.Billid))
            {
                "查询条件为空,不返回对象".Warn(Exit);
                return false;
            }

            using (var db = DbContext)
            {
                billGoodsRefund = db.BillGoodsRefund.FirstOrDefault(a =>
                    (string.IsNullOrWhiteSpace(query.Billid) || query.Billid.Equals(a.Billid)));
            }

            return null != billGoodsRefund;
        }
    }
}
