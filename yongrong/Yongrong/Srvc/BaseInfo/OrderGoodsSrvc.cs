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
    class OrderGoodsSrvc : BaseSrvc
    {
        /// <summary>
        /// 获取原辅料进厂预约管理
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool Get(OrderGoods query, PageBean page, out List<OrderGoods> baselist)
        {
            baselist = new List<OrderGoods>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new OrderGoods();
            }

            //if (string.IsNullOrWhiteSpace(query.Orderstat))
            //{
            //    query = new OrderGoods();
            //}
            query.Debug();

            using (var db = DbContext)
            {
                page.Row = db.OrderGoods.Count(a =>
                (string.IsNullOrWhiteSpace(query.Ordername) || (!string.IsNullOrWhiteSpace(a.Ordername) && a.Ordername.Contains(query.Ordername)))
                && (string.IsNullOrWhiteSpace(query.Tractorid) || query.Tractorid.Trim().Equals(a.Tractorid.Trim()))
                && (string.IsNullOrWhiteSpace(query.Orderstat) ||query.Orderstat.Equals(a.Orderstat))
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Orderid) || query.Orderid.Equals(a.Orderid))
                && (string.IsNullOrWhiteSpace(query.Issendback) || query.Issendback.Equals(a.Issendback))
                && (string.IsNullOrWhiteSpace(query.Istoexit) || query.Istoexit.Equals(a.Istoexit))
                && (string.IsNullOrWhiteSpace(query.Ordertime) || (!string.IsNullOrWhiteSpace(query.Ordertime) && a.Ordertime.StartsWith(query.Ordertime)))
                && (string.IsNullOrWhiteSpace(query.Starttime) || 0 >= query.Starttime.CompareTo(a.Ordertime))
                && (string.IsNullOrWhiteSpace(query.Endtime) || 0 <= query.Endtime.CompareTo(a.Ordertime))
                && (string.IsNullOrWhiteSpace(query.Safecheckflag) || query.Safecheckflag.Equals(a.Safecheckflag))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                && (string.IsNullOrWhiteSpace(query.Goodsname) || a.Goodsname.Contains(query.Goodsname))

                );

                page.SumPageCount();

                baselist.AddRange(
                    db.OrderGoods.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Ordername) || (!string.IsNullOrWhiteSpace(a.Ordername) && a.Ordername.Contains(query.Ordername)))
                && (string.IsNullOrWhiteSpace(query.Tractorid) || query.Tractorid.Equals(a.Tractorid))
                && (string.IsNullOrWhiteSpace(query.Orderstat) || query.Orderstat.Equals(a.Orderstat))
                && (string.IsNullOrWhiteSpace(query.Company) || (!string.IsNullOrWhiteSpace(a.Company) && a.Company.Contains(query.Company)))
                && (string.IsNullOrWhiteSpace(query.Orderid) || query.Orderid.Equals(a.Orderid))
                && (string.IsNullOrWhiteSpace(query.Issendback) || query.Issendback.Equals(a.Issendback))
                && (string.IsNullOrWhiteSpace(query.Istoexit) || query.Istoexit.Equals(a.Istoexit))
                && (string.IsNullOrWhiteSpace(query.Ordertime) || (!string.IsNullOrWhiteSpace(query.Ordertime) && a.Ordertime.StartsWith(query.Ordertime)))
                && (string.IsNullOrWhiteSpace(query.Starttime) || 0 >= query.Starttime.CompareTo(a.Ordertime))
                && (string.IsNullOrWhiteSpace(query.Endtime) || 0 <= query.Endtime.CompareTo(a.Ordertime))
                && (string.IsNullOrWhiteSpace(query.Safecheckflag) || query.Safecheckflag.Equals(a.Safecheckflag))
                   && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                && (string.IsNullOrWhiteSpace(query.Goodsname) || a.Goodsname.Contains(query.Goodsname))
                    ).OrderBy(a => a.Orderstat).OrderByDescending(o=>o.Createtime)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );


            }

            return true;
        }


        /// <summary>
        /// 获取iphone预约管理查询预约最新的预约时间5条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool GetIphoneOrderData(OrderGoods query, PageBean page, out List<OrderGoods> baselist)
        {
            baselist = new List<OrderGoods>();

            if (null == page)
            {
                page = new PageBean();
            }

            if (null == query)
            {
                query = new OrderGoods();
            }

            query.Debug();
            //if (string.IsNullOrWhiteSpace(query.Orderstat))
            //{
            //    query = new OrderGoods();
            //}

            using (var db = DbContext)
            {
                page.Row = db.OrderGoods.Count(a =>
                (string.IsNullOrWhiteSpace(query.Ordername) || query.Ordername.Equals(a.Ordername))
                && (string.IsNullOrWhiteSpace(query.Tractorid) || query.Tractorid.Equals(a.Tractorid))
                && (string.IsNullOrWhiteSpace(query.Orderstat) || query.Orderstat.Equals(a.Orderstat))
                && (string.IsNullOrWhiteSpace(query.Company) || query.Company.Equals(a.Company))
                && (string.IsNullOrWhiteSpace(query.Orderid) || query.Orderid.Equals(a.Orderid))
                && (string.IsNullOrWhiteSpace(query.Issendback) || query.Issendback.Equals(a.Issendback))
                && (string.IsNullOrWhiteSpace(query.Istoexit) || query.Istoexit.Equals(a.Istoexit))
                && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                );

                page.SumPageCount();

                baselist.AddRange(
                    db.OrderGoods.Where(a =>
                    (string.IsNullOrWhiteSpace(query.Ordername) || query.Ordername.Equals(a.Ordername))
                    && (string.IsNullOrWhiteSpace(query.Tractorid) || query.Tractorid.Equals(a.Tractorid))
                    && (string.IsNullOrWhiteSpace(query.Orderstat) || query.Orderstat.Equals(a.Orderstat))
                    && (string.IsNullOrWhiteSpace(query.Company) || query.Company.Equals(a.Company))
                    && (string.IsNullOrWhiteSpace(query.Orderid) || query.Orderid.Equals(a.Orderid))
                    && (string.IsNullOrWhiteSpace(query.Issendback) || query.Issendback.Equals(a.Issendback))
                    && (string.IsNullOrWhiteSpace(query.Istoexit) || query.Istoexit.Equals(a.Istoexit))
                    && (string.IsNullOrWhiteSpace(query.Driver) || query.Driver.Equals(a.Driver))
                    ).OrderByDescending(a=>a.Id)
                    .Skip((page.Index - 1) * page.Size).Take(page.Size)
                    );
            }

            return true;
        }

        /// <summary>
        /// 根据总单编号查询当日已经预约的数量
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int CountOrderGoods(OrderGoods query)
        {
            if (null == query)
            {
                return 0;
            }
            using (var db = DbContext)
            {
                string date = DateTime.Now.ToString(BTip.DateFormater);

                int count = db.OrderGoods.Count(a =>
                  (string.IsNullOrWhiteSpace(query.Billid) || query.Billid.Equals(a.Billid))
                   && (string.IsNullOrWhiteSpace(query.Orderstat) || query.Orderstat.Equals(a.Orderstat))
                  && a.Ordertime.StartsWith(date));

                return count;
            }
        }

        /// <summary>
        /// 添加和更新预约信息
        /// </summary>
        /// <param name="addupdobj"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string AddOrUpdate(OrderGoods addupdobj, string op, string role)
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
                        addupdobj.Safecheckflag = "0";
                        if(string.IsNullOrWhiteSpace(addupdobj.Orderid))
                        {
                            addupdobj.Orderid = OrderGoodsSrvc.CreateOrderId().ToString("0000");
                        }                       
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        //addupdobj.Ordertime = DateTime.Now.ToString("yyyy-MM-dd");
                        addupdobj.Id = GetSeq("SEQ_ORDER_GOODSIN");
                        db.OrderGoods.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.OrderGoods.FirstOrDefault(a => a.Id == addupdobj.Id);
                        
                        if (null == tmp)
                        {
                            $"LogisticsGoodsextenterorder无法找到{addupdobj.Id}".Warn();
                            return UpdNoRecord;
                        }

                        var oldstat = tmp.Orderstat;
                        addupdobj.Upd(tmp);

                        if ("1".Equals(addupdobj.Orderstat) && "0".Equals(oldstat))
                        {
                            if (Sys.SaleRole.Contains(role))
                            {
                                tmp.Billcheck = "1";
                            }
                            else if (Sys.CustomRole.Contains(role))
                            {
                                tmp.Customercheck = "1";
                            }

                            if ("1".Equals(tmp.Billcheck) && "1".Equals(tmp.Customercheck))
                            {
                                tmp.Orderstat = "1";
                            }
                            else
                            {
                                tmp.Orderstat = "0";
                            }
                        }

                        db.OrderGoods.Update(tmp);

                        // 退还预约时扣除的数量
                        if ("3".Equals(tmp.Orderstat) || "2".Equals(tmp.Orderstat))
                        {
                            var stat = OrderGoodsSrvc.RechargeBill(tmp);
                            stat.Info("RechargeBill Stat");
                        }

                        break;
                    case DEL:
                        var tmp1 = db.OrderGoods.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"LogisticsGoodsextenterorder无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.OrderGoods.Remove(tmp1);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                SaveChanges(db, "OrderGOodsSrvc"+op);
                //db.SaveChanges();
            }

            return Success;
        }

        /// <summary>
        /// 生成预约编号
        /// 从SEQ_ORDERID里获取自增量，然后对10000取余
        /// </summary>
        /// <returns></returns>
        public static decimal CreateOrderId()
        {
            decimal id = GetSeq("SEQ_ORDERID");
            return id % 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderGoods"></param>
        /// <returns></returns>
        public static bool GetOneById(OrderGoods query, out OrderGoods orderGoods)
        {
            orderGoods = null;
            if(null == query)
            {
                "OrderGoodsSrvc.GetOneById查询条件为null,不返回对象".Warn(Exit);
                return false;
            }

            if(0 >= query.Id)
            {
                "OrderGoodsSrvc.GetOneById查询条件为null,不返回对象".Warn(Exit);
                return false;
            }

            using (var db = DbContext)
            {
                orderGoods = db.OrderGoods.FirstOrDefault(a => a.Id == query.Id);
            }

            return null != orderGoods;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string SubBill(OrderGoods req)
        {
            bool billexist = false;
            dynamic bill;
            string unloadarea = string.Empty;
            string loadarea = string.Empty;
            string ordername = string.Empty;
            BillGoodsIn b1 = null;
            BillGoodsOut b2 = null;
            BillGoodsRefund b3 = null;

            switch (req.GetBillType)
            {
                case "进厂":
                    billexist = BillGoodsInSrvc.GetOne(new BillGoodsIn() { Billid = req.Billid }, out b1);
                    bill = b1;
                    unloadarea = "卸货区";
                    ordername = b1.Maker;
                    break;
                case "出厂":
                    billexist = BillGoodsOutSrvc.GetOne(new BillGoodsOut() { Billid = req.Billid }, out b2);
                    bill = b2;
                    loadarea = "装货区";
                    ordername = b2.Salesman;
                    break;
                default: // 退货
                    billexist = BillGoodsRefundSrvc.GetOne(new BillGoodsRefund { Billid = req.Billid }, out b3);
                    bill = b3;
                    ordername = b3.Maker;
                    if ("1".Equals(req.Issendback))
                    {
                        unloadarea = "卸货区";
                    }
                    else
                    {
                        loadarea = "装货区";
                    }
                    break;
            }
            
            if (!billexist)
            {
                return "总单不存在";
            }

            if (0 == bill.Goodsnumber)
            {
                return "总提量为0";
            }

            if (0 == bill.Levelnumber)
            {
                return "剩余量为0";
            }

            if(Convert.ToDecimal(req.Realweight) > 100)
            {
                req.Realweight = (Convert.ToDecimal(req.Realweight) / 1000).ToString();
            }

            if (bill.Levelnumber < Convert.ToDecimal(req.Realweight))
            {
                return "预约量超过剩余量";
            }

            /*OrderGoodsSrvc.GetUnCheckOrder(req, out var baselist);
            if(null != baselist && baselist.Count > 0)
            {
                return "你还有未审核的预约，不能再申请新的预约";
            }

            OrderGoodsSrvc.GetUnFinishOrder(req, out var baselist2);
            if(null != baselist2 && baselist2.Count > 0)
            {
                return "你还有未完成的预约，不能再申请新的预约";
            }*/

            //预约表里没数据的话会报错
            int orderNumber = 0;
            int maxNumber = 0;

            orderNumber = OrderGoodsSrvc.CountOrderGoods(new Model.Db.OrderGoods() { Billid = req.Billid , Orderstat ="1"});
            orderNumber += OrderGoodsSrvc.CountOrderGoods(new Model.Db.OrderGoods() { Billid = req.Billid, Orderstat = "0" });

            if (orderNumber > 0)
            {
                maxNumber = OrderConfigSrvc.GetMaxNumber(new Model.Db.OrderConfig() { Goodsname = req.Goodsname, Unloadarea = unloadarea, Loadarea = loadarea });

                if (0 == maxNumber)
                {
                    return "储运调度管理里未设置‘当日预约车辆总数/大货车数量限制’";
                }
                //(orderNumber + ":" + maxNumber).Info("预约默认调度数判断");
                if (orderNumber >= maxNumber)
                {
                    return "车辆预约数已经达到最大";
                }
             
            }

            if (string.IsNullOrWhiteSpace(bill.Drivers) || !bill.Drivers.Contains(req.Driver))
            {
                return "预约的司机不在总单里";
            }

            if (string.IsNullOrWhiteSpace(bill.Tractorids) || !bill.Tractorids.Contains(req.Tractorid))
            {
                return "预约的牵引车车牌号不在总单里";
            }

            if (string.IsNullOrWhiteSpace(bill.Trailerids) || !bill.Trailerids.Contains(req.Trailerid))
            {
                return "预约的挂车车牌号不在总单里";
            }

            /*if (string.IsNullOrWhiteSpace(bill.Supercargo) || !bill.Supercargo.Contains(req.Supercargo))
            {
                return "预约的押运员不在总单里";
            }*/

            req.Orderid = OrderGoodsSrvc.CreateOrderId().ToString("0000");
            req.Ordername = ordername;
            req.Company = bill.Company;
            req.Goodsid = bill.Goodsid;
            req.Goodsname = bill.Goodsname;
            

            if (string.IsNullOrWhiteSpace(bill.Status))
            {
                bill.Status = "11";
            }

            if ("11".Equals(bill.Status))
            {
                req.Orderstat = "1";
            }
            else
            {
                req.Orderstat = "0";

                if (2 == bill.Status.Length)
                {
                    req.Billcheck = bill.Status.Substring(0, 1);
                    req.Customercheck = bill.Status.Substring(1, 1);
                }
            }

            OrderGoodsSrvc.AddOrUpdate(req, ADD, string.Empty);

            bill.Levelnumber = bill.Levelnumber - Convert.ToDecimal(req.Realweight);

            switch (req.GetBillType)
            {
                case "进厂":
                    BillGoodsInSrvc.AddOrUpdate(b1, UPD);
                    break;
                case "出厂":
                    BillGoodsOutSrvc.AddOrUpdate(b2, UPD);
                    
                    break;
                default: // 退货
                    BillGoodsRefundSrvc.AddOrUpdate(b3, UPD);
                    break;
            }


            return Success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string RechargeBill(OrderGoods req)
        {
            req.Debug("返回扣除的预约数量");

            switch (req.GetBillType)
            {
                case "进厂":
                    if(!BillGoodsInSrvc.GetOne(new BillGoodsIn() { Billid = req.Billid }, out var b1))
                    {
                        return "总单不存在";
                    }
                    b1.Levelnumber = b1.Levelnumber + Convert.ToDecimal(req.Realweight);
                    BillGoodsInSrvc.AddOrUpdate(b1, UPD);
                    break;
                case "出厂":
                    if (!BillGoodsOutSrvc.GetOne(new BillGoodsOut() { Billid = req.Billid }, out var b2))
                    {
                        return "总单不存在";
                    }
                    b2.Levelnumber = b2.Levelnumber + Convert.ToDecimal(req.Realweight);
                    BillGoodsOutSrvc.AddOrUpdate(b2, UPD);
                    break;
                default: // 退货
                    if (!BillGoodsRefundSrvc.GetOne(new BillGoodsRefund() { Billid = req.Billid }, out var b3))
                    {
                        return "总单不存在";
                    }
                    b3.Levelnumber = b3.Levelnumber + Convert.ToDecimal(req.Realweight);
                    BillGoodsRefundSrvc.AddOrUpdate(b3, UPD);
                    break;
            }

            return Success;
        }

        /// <summary>
        /// 获取司机或者牵引车某天未审核的预约
        /// </summary>
        /// <param name="query"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool GetUnCheckOrder(OrderGoods query, out List<OrderGoods> baselist)
        {
            baselist = new List<OrderGoods>();

            using (var db = DbContext)
            {

                baselist.AddRange(
                    db.OrderGoods.Where(a =>
                    query.Ordertime.Equals(a.Ordertime)
                    && "0".Equals(a.Orderstat)
                    && (query.Driver.Equals(a.Driver) || query.Tractorid.Equals(a.Tractorid))
                    ));
            }

            return true;
        }

        /// <summary>
        /// 获取司机或者牵引车未在某一天未完成的预约
        /// </summary>
        /// <param name="query"></param>
        /// <param name="baselist"></param>
        /// <returns></returns>
        public static bool GetUnFinishOrder(OrderGoods query, out List<OrderGoods> baselist)
        {
            baselist = new List<OrderGoods>();

            using (var db = DbContext)
            {

                baselist.AddRange(
                    db.OrderGoods.Where(a =>
                    query.Ordertime.Equals(a.Ordertime) 
                    && "1".Equals(a.Orderstat)
                    && (query.Driver.Equals(a.Driver) || query.Tractorid.Equals(a.Tractorid))
                    && (string.IsNullOrWhiteSpace(a.Grosstime) || string.IsNullOrWhiteSpace(a.Taretime))
                    ));
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_"></param>
        public static void TaskDay(object _)
        {
            using var db = DbContext;

            List<string> undolist = new List<string>() { "0", "1" };
            var passDate = DateTime.Now.AddDays(Sys.UndoOrderReturn).ToString("yyyy-MM-dd");

            var undoOrders = db.OrderGoods.Where(a => 
            undolist.Contains(a.Orderstat) 
            && (string.IsNullOrWhiteSpace(a.Taretime))
            && (string.IsNullOrWhiteSpace(a.Grosstime))
            && (0 <= passDate.CompareTo(a.Ordertime))
            );

            foreach(var order in undoOrders)
            {
                //获取车辆安检表信息
                var safecheck = db.SafeCheck.FirstOrDefault(a => a.Id == order.Id);

                // 安检已经通过情况下不需要进行状态变更(运输车辆在厂区内存在多天的情况)
                if (null != safecheck &&　safecheck.IsAllPass)
                {
                    continue;
                }

                order.Orderstat = "3";
                
                db.OrderGoods.Update(order);

                SaveChanges(db, "预约过期,返回预扣");

                try
                {
                    // 退货
                    if ("2".Equals(order.Istoexit))
                    {
                        var refund = db.BillGoodsRefund.FirstOrDefault(a => order.Billid.Equals(a.Billid));

                        if (null == refund)
                        {
                            $"退货单号:{order.Billid} 未找到退货总单".Warn();
                            continue;
                        }

                        refund.Levelnumber += decimal.Parse(order.Realweight);
                        
                        db.BillGoodsRefund.Update(refund);

                        SaveChanges(db, "退货总单未使用退回预订量");
                        continue;
                    }

                    // 提货
                    if (order.IsTakeGoods)
                    {
                        var outOrder = db.BillGoodsOut.FirstOrDefault(a => order.Billid.Equals(a.Billid));

                        if (null == outOrder)
                        {
                            $"出厂单号:{order.Billid} 未找到出厂总单".Warn();
                            continue;
                        }

                        outOrder.Levelnumber += decimal.Parse(order.Realweight);
                        db.BillGoodsOut.Update(outOrder);
                        SaveChanges(db, "出厂总单未使用退回预订量");
                        continue;
                    }

                    var inOrder = db.BillGoodsIn.FirstOrDefault(a => order.Billid.Equals(a.Billid));

                    if (null == inOrder)
                    {
                        $"进厂单号:{order.Billid} 未找到进厂总单".Warn();
                        continue;
                    }

                    inOrder.Levelnumber += decimal.Parse(order.Realweight);
                    db.BillGoodsIn.Update(inOrder);
                    SaveChanges(db, "进厂总单未使用退回预订量");
                }
                catch(Exception e)
                {
                    $"Err:{e.Message} \n\t StackTrace:{e.StackTrace}".Warn();
                }
            }
        }
    }
}
