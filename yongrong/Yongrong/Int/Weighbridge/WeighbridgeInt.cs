﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Weigh;
using BaseS.Serialization;
using Yongrong.Model.Db;
using Yongrong.Srvc.Weighbridge;
using BaseS.Security;
using BaseS.String;
using BaseS.File.Log;
using BaseS.Collection;
using Yongrong.Srvc.Gate;
using Yongrong.Srvc.BaseInfo;
using System.Linq;
using Yongrong.Srvc.Sys;
using Yongrong.Model.Base;

namespace Yongrong.Int.Weighbridge
{
    class WeighbridgeInt : BaseInt
    {
        /// <summary>
        /// 接收到地磅反馈的消息
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] RecvWeigh(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            WeighSyncRsp rsp = new WeighSyncRsp() { Stat = Success };

            reqbytes.Json2ObjT<WeighSyncReq>(out WeighSyncReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            reqmap.TryAdd(OP_Detail, reqbytes.B2String());
            reqmap.TryAdd(OP_User, "地磅系统");
            reqmap.TryAdd(OP_Content, "过磅信息同步");

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = (req.Orderid + req.Msgid + req.Passtime).ToMD5String(32).ToLower();

            if (!sign.Equals(req.Sign))
            {
                rsp.Stat = $"sign error";
                $"sign error,Inner:{sign} Err:{req.Sign}".Warn();
                return rsp.ToBytes();
            }

            reqbytes.Json2ObjT<ApiWeigh>(out ApiWeigh req2);

            if (!Weigh.WeighIp.Contains(reqmap.GetDicStr(Http.ClientIPName)))
            {
                $"无效的IP地址:{reqmap.GetDicStr(Http.ClientIPName)}".Warn();
                rsp.Stat = "ip block";
                return rsp.ToBytes();
            }

            WeighbridgeSrvc.AddSync(req2);

            List<OrderGoods> list = new List<OrderGoods>();

            // 预约编号如果为空,按照车牌号和Safecheckflag字段查询
            if (string.IsNullOrWhiteSpace(req2.Orderid))
            {
                if ("2".Equals(req2.Weightime))
                {

                    OrderGoodsSrvc.Get(new OrderGoods() { Tractorid = req2.Tractorid, Safecheckflag = "2" }, null, out list);

                }

                if ("1".Equals(req2.Weightime) || 0 == list.Count)
                {

                    OrderGoodsSrvc.Get(new OrderGoods() { Tractorid = req2.Tractorid, Safecheckflag = "1" }, null, out list);

                }
           
            }
            else
            {

                OrderGoodsSrvc.Get(new OrderGoods() { Orderid = req2.Orderid, Orderstat = "1" }, null, out  list);    
            }

            // 无此预约信息挂钩的订单
            if (null == list || 0 >= list.Count || null == list.FirstOrDefault())
            {
                $"当日{DateTime.Now.ToString("yyyy-MM-dd")}没有此预约信息:预约编码为{req2.Orderid},车牌号为{req2.Tractorid}的订单".Warn();
                return rsp.ToBytes();
            }

            var order = list.FirstOrDefault();

            if ("1".Equals(req2.Weightime))
            {
                // 提货流程
                if (order.IsTakeGoods)
                {
                    if (!string.IsNullOrWhiteSpace(order.Taretime))
                    {
                        $"该预约重复收到一次过磅信息".Tip();
                        return rsp.ToBytes();
                    }
                    else
                    {
                        order.Tareman = req2.Operator;
                        order.Taretime = req2.Passtime;
                        order.Tareweight = req.Weight;
                    }
                }
                else // 送货流程
                {
                    if (!string.IsNullOrWhiteSpace(order.Grosstime))
                    {
                        $"该预约重复收到一次过磅信息".Tip();
                        return rsp.ToBytes();
                    }
                    else
                    {
                        order.Grosstime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        order.Grossweight = req2.Weight;
                        order.Grossman = req2.Operator;
                    }
                }

                order.Safecheckflag = ConstOrderGoods.AWEIGHING;

                order.Debug($"提货:{order.IsTakeGoods} - 1次过磅更新后的预约信息");

                OrderGoodsSrvc.AddOrUpdate(order, UPD, string.Empty);
            }

            // 2次过磅后,自动设置门禁通行
            if ("2".Equals(req2.Weightime))
            {
                // 提货流程
                if (order.IsTakeGoods)
                {
                    if (!string.IsNullOrWhiteSpace(order.Grosstime))
                    {
                        $"该预约重复收到二次过磅信息".Tip();
                        return rsp.ToBytes();
                    }
                    else
                    {
                        order.Grosstime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        order.Grossweight = req2.Weight;
                        order.Grossman = req2.Operator;
                    }
                }
                else // 送货流程
                {
                    if (!string.IsNullOrWhiteSpace(order.Taretime))
                    {
                        $"该预约重复收到二次过磅信息".Tip();
                        return rsp.ToBytes();
                    }
                    else
                    {
                        order.Tareman = req2.Operator;
                        order.Taretime = req2.Passtime;
                        order.Tareweight = req.Weight;
                    }
                }

                order.Netweight = System.Math.Abs(Convert.ToDecimal(BString.GetString(order.Grossweight)) - Convert.ToDecimal(BString.GetString(order.Tareweight))).ToString();
                order.Safecheckflag = ConstOrderGoods.TWEIGHING;

                order.Debug($"提货:{order.IsTakeGoods} - 2次过磅更新后的预约信息");

                OrderGoodsSrvc.AddOrUpdate(order, UPD, string.Empty);

                if (Gate.GateFlag)
                {
                    if (!GateSrvc.GetCarInfos(new Model.Int.Gate.GateGetCarInfoReq() { PlateNo = req2.Tractorid }))
                    {
                        GateSrvc.AddCarInfo(new Model.Int.Gate.GateAddCarReq() { PlateNo = req2.Tractorid, CarType = 2 });
                    }

                    if (Gate.GateSouthFlag)
                    {
                        GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                        {
                            EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(Gate.EnterTimeSpan)),
                            Money = "0.00",
                            ParkUuid = Gate.GateSouthUuid, // 南门出入口
                            PlateNo = req2.Tractorid,
                            Remark = Gate.AppRemark,
                            //StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now)
                            // 防止手动开闸的问题,充值开始时间小于入场时间
                            StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddDays(-2.0))
                        });
                    }

                    GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                    {
                        EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(Gate.EnterTimeSpan)),
                        Money = "0.00",
                        ParkUuid = Gate.GateNorthUuid, // 北门出入口
                        PlateNo = req2.Tractorid,
                        Remark = Gate.AppRemark,
                        //StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now)
                        // 防止手动开闸的问题,充值开始时间小于入场时间
                        StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddDays(-2.0))
                    });

                }
            }

            return rsp.ToBytes();
        }
    }
}
