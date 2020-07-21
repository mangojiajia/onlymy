﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using System.Linq;
using BaseS.File.Log;
using BaseS.Const;

namespace Yongrong.Srvc.BaseInfo
{
    class BillGoodsInSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取原辅料进厂计划总单管理
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(BillGoodsIn query, PageBean page, out List<BillGoodsIn> baselist)
        {
            baselist = new List<BillGoodsIn>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new BillGoodsIn();
            }

            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.BillGoodsIn.Count(a =>
                (string.IsNullOrWhiteSpace(query.Billstat) || query.Billstat.Equals(a.Billstat)
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Billid) || (!string.IsNullOrWhiteSpace(a.Billid) && a.Billid.Contains(query.Billid))))
                && (string.IsNullOrWhiteSpace(query.Maker) || (!string.IsNullOrWhiteSpace(a.Maker) && a.Maker.Contains(query.Maker)))
                && (string.IsNullOrWhiteSpace(query.Supercargo) || (!string.IsNullOrWhiteSpace(a.Supercargo) && a.Supercargo.Contains(query.Supercargo)))
                && (string.IsNullOrWhiteSpace(query.Tractorids) || (!string.IsNullOrWhiteSpace(query.Tractorids) && a.Tractorids.Contains(query.Tractorids)))
                && (string.IsNullOrWhiteSpace(query.Drivers) || (!string.IsNullOrWhiteSpace(query.Drivers) && a.Drivers.Contains(query.Drivers)))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.BillGoodsIn.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Billstat) || query.Billstat.Equals(a.Billstat)
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Billid) || (!string.IsNullOrWhiteSpace(a.Billid) && a.Billid.Contains(query.Billid))))
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
        /// 根据总单号获取总单信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="billGoodsIn"></param>
        /// <returns></returns>
        public static bool GetOne(BillGoodsIn query, out BillGoodsIn billGoodsIn)
        {
            billGoodsIn = null;
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
                billGoodsIn = db.BillGoodsIn.FirstOrDefault(a =>
                    (string.IsNullOrWhiteSpace(query.Billid) || query.Billid.Equals(a.Billid)));
            }

            return null != billGoodsIn;
        }


        /// <summary>
        /// 添加和更新总单信息
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(BillGoodsIn addupdobj, string op)
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
                        var bill = db.BillGoodsIn.FirstOrDefault(a => addupdobj.Billid.Equals(a.Billid));
                        if(null != bill)
                        {
                            $"BillGoodsIn对应的Billid：{addupdobj.Billid}已经存在".Warn();
                            return AddRecord;
                        }
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                        addupdobj.Id = GetSeq("SEQ_BILL_GOODSIN");
                        addupdobj.Levelnumber = addupdobj.Goodsnumber;
                        db.BillGoodsIn.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.BillGoodsIn.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"BillGoodsIn无法找到{addupdobj.Id}".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.BillGoodsIn.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.BillGoodsIn.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"BillGoodsIn无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.BillGoodsIn.Remove(tmp1);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                db.SaveChanges();
            }

            return Success;
        }
    }
}
