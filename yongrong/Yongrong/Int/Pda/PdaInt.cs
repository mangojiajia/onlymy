﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int.Pda;
using BaseS.Serialization;
using Yongrong.Srvc.Users;
using BaseS.Security;
using BaseS.File.Log;
using BaseS.Collection;
using Yongrong.Srvc.BaseInfo;
using System.Linq;
using BaseS.String;
using Yongrong.Srvc.Gate;
using Yongrong.Srvc.Weighbridge;
using System.Collections.Concurrent;
using Yongrong.Model.Db;
using Yongrong.Srvc.Sys;
using Yongrong.Model.Base;

namespace Yongrong.Int.Pda
{
    class PdaInt : BaseInt
    {

        /// <summary>
        /// Pda 登录请求
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Login(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PdaLoginRsp rsp = new PdaLoginRsp() { Stat = Success };

            reqbytes.Json2ObjT<PdaLoginReq>(out PdaLoginReq req);
            //进日志
            req.Debug(string.Empty, Enter);

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

            //司机登陆账号是手机号 用户表userid是手机号 pwd 是账号的md5加密
            if (!UserSrvc.Login(new Model.Db.User() { Userid = req.Uid }, out var user))
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = (req.Uid + req.Time + user.Pwd).ToMD5String(32).ToUpper();

            if (!sign.Equals(req.Sign))
            {
                rsp.Stat = $"sign error,Inner:{sign} Err:{req.Sign}";
                return rsp.ToBytes();
            }

            rsp.Stat = Success;
            rsp.Token = user.Token;
            rsp.Expire = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd HH:mm:ss");


            rsp.Obj2JsonT<PdaLoginRsp>(out string rspmessage);

            //新增Pda日志
            PdaLog pdalog = new PdaLog
            {
                Tractorid = user?.Userid,
                Operation = ConstPda.Login,
                Rspcode = ConstPda.Success,
                Reqparameter = reqbytes?.B2String(),
                Rspmessage = rspmessage
            };

            PdaLogSrvc.AddOrUpdate(pdalog, ADD);

            user.Info("用户登录成功");

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "PDA登录");

            return rsp.ToBytes();
        }

        /// <summary>
        /// Pda 登出请求
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] Logout(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PdaLoginRsp rsp = new PdaLoginRsp() { Stat = Success };

            reqbytes.Json2ObjT<PdaLoginReq>(out PdaLoginReq req);
            //进日志
            req.Debug(string.Empty, Enter);

            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            req.Token = reqmap.GetDicStr("Token");

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            UserSrvc.GetOne(new Model.Db.User() { Userid = req.Uid }, out var user);

            if (null == user)
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = (req.Uid + req.Time + user.Pwd).ToMD5String(32).ToUpper();

            if (!sign.Equals(req.Sign))
            {
                rsp.Stat = $"sign error,Inner:{sign} Err:{req.Sign}";
                return rsp.ToBytes();
            }

            rsp.Stat = Success;
            rsp.Token = "";
            rsp.Expire = "";

            user.Info("用户登出成功");

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "PDA退出登录");

            return rsp.ToBytes();
        }

        /// <summary>
        /// Pda 查询预约
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] QueryOrder(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PdaQueryOrderRsp rsp = new PdaQueryOrderRsp() { Stat = Success };

            reqbytes.Json2ObjT<PdaQueryOrderReq>(out PdaQueryOrderReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            if (!string.IsNullOrWhiteSpace(reqmap.GetDicStr("Token")))
            {
                req.Token = reqmap.GetDicStr("Token");
            }

            //req.Info("PdaQueryOrderReq");

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user);

            if (null == user)
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = (req.Time + req.Order + req.Tractor + user.Pwd).ToMD5String(32).ToUpper();

            if (!sign.Equals(req.Sign))
            {
                rsp.Stat = $"sign error,Inner:{sign} Err:{req.Sign}";

                //新增Pda日志
                PdaLog pdalog1 = new PdaLog
                {
                    Orderid = req?.Order,
                    Tractorid = req?.Tractor,
                    Operation = ConstPda.Query,
                    Rspcode = ConstPda.Fail,
                    Reqparameter = reqbytes?.B2String() + $",用户密码:{user.Pwd}",
                    Rspmessage = $"签名错误,Inner:{sign} Err:{req.Sign}"
                };

                PdaLogSrvc.AddOrUpdate(pdalog1, ADD);

                return rsp.ToBytes();
            }

            OrderGoodsSrvc.Get(new Model.Db.OrderGoods() { Tractorid = req.Tractor, Orderid = req.Order, Orderstat = "1", Ordertime = DateTime.Now.ToString("yyyy-MM-dd") }, null, out var list);

            string rspmessage = string.Empty;

            if (null != list && 1 <= list.Count)
            {
                list.Reverse();
                var order = list.FirstOrDefault(a => string.IsNullOrWhiteSpace(a.Grosstime) && string.IsNullOrWhiteSpace(a.Taretime));

                if (null != order)
                {
                    rsp.Data = new Order()
                    {
                        Id = (int)order.Id,
                        Driver = order.Driver,
                        Loadweight = order.Loadweight,
                        Ordercompany = order.Company,
                        Orderstat = order.Orderstat,
                        Ordertime = order.Ordertime,
                        Ordertype = order.Issendback,
                        Realweight = order.Realweight,
                        Refund = order.Istoexit,
                        Supercargo = order.Supercargo,
                        Tractorid = order.Tractorid,
                        Trailerid = order.Trailerid,
                        OrderMan = order.Ordername,
                        OrderNum = order.Orderid
                    };

                    rsp.Obj2JsonT<PdaQueryOrderRsp>(out rspmessage);
                }
            }

            //新增Pda日志
            PdaLog pdalog = new PdaLog
            {
                Orderid = req?.Order,
                Tractorid = req?.Tractor,
                Operation = ConstPda.Query,
                Rspcode = ConstPda.Success,
                Reqparameter = reqbytes?.B2String(),
                Rspmessage = rspmessage
            };

            PdaLogSrvc.AddOrUpdate(pdalog, ADD);

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "查询PDA预约信息");

            return rsp.ToBytes();
        }

        /// <summary>
        /// Pda 提交审核结果
        /// </summary>
        /// <param name="reqmap"></param>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static byte[] SubmitCheck(Dictionary<string, string> reqmap, byte[] reqbytes)
        {
            PdaSubmitCheckRsp rsp = new PdaSubmitCheckRsp() { Stat = Success };

            reqbytes.Json2ObjT<PdaSubmitCheckReq>(out PdaSubmitCheckReq req);
            //进日志
            req.Debug(string.Empty, Enter);
            if (null == req)
            {
                rsp.Stat = JsonErr;
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            req.Token = reqmap.GetDicStr("Token");

            if (!Success.Equals(req.Check()))
            {
                rsp.Stat = req.Check();
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            UserSrvc.GetOne(new Model.Db.User() { Token = req.Token }, out var user);

            if (null == user)
            {
                rsp.Stat = "用户不存在";
                reqmap.TryAdd(OP_Content, rsp.Stat);
                reqmap.TryAdd(OP_Detail, reqbytes.B2String());
                return rsp.ToBytes();
            }

            var sign = (req.Time + user.Pwd).ToMD5String(32).ToUpper();

            if (!sign.Equals(req.Sign))
            {
                //新增Pda日志
                PdaLog pdalog1 = new PdaLog
                {
                    Tractorid = user?.Userid,
                    Operation = ConstPda.Submit,
                    Rspcode = ConstPda.Fail,
                    Reqparameter = reqbytes?.B2String() + $",用户密码:{user.Pwd}",
                    Rspmessage = $"签名错误,Inner:{sign} Err:{req.Sign}"
                };

                PdaLogSrvc.AddOrUpdate(pdalog1, ADD);

                rsp.Stat = $"sign error";
                return rsp.ToBytes();
            }

            OrderGoodsSrvc.GetOneById(new Model.Db.OrderGoods() { Id = req.CheckData.Id }, out var orderGoods);

            if (null == orderGoods)
            {
                rsp.Stat = "预约信息不存在";
                return rsp.ToBytes();
            }

            SafeCheckSrvc.AddOrUpdate(new Model.Db.SafeCheck()
            {
                Id = req.CheckData.Id,
                Expburn = req.CheckData.Expburn == null ? "合格" : ("pass".Equals(req.CheckData.Expburn) ? "合格" : "不合格"),
                Helmet = req.CheckData.Helmet == null ? "合格" : ("pass".Equals(req.CheckData.Helmet) ? "合格" : "不合格"),
                Workshoe = req.CheckData.Workshoe == null ? "合格" : ("pass".Equals(req.CheckData.Workshoe) ? "合格" : "不合格"),
                Smock = req.CheckData.Smock == null ? "合格" : ("pass".Equals(req.CheckData.Smock) ? "合格" : "不合格"),
                Driver = orderGoods.Driver == null ? "" : orderGoods.Driver.Trim(),
                Tractorid = orderGoods.Tractorid == null ? "" : orderGoods.Tractorid.Trim(),
                Trailerid = orderGoods.Trailerid == null ? "" : orderGoods.Trailerid.Trim(),
                Supercargo = orderGoods.Supercargo == null ? "" : orderGoods.Supercargo.Trim(),
                Remark = req.CheckData.Remark,
                Tire = req.CheckData.Tire == null ? "合格" : ("pass".Equals(req.CheckData.Tire) ? "合格" : "不合格"),
                Checkman = req.CheckData.Uid,
                Checktime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            }, ADD);

            if (!req.CheckData.IsAllPass)
            {
                StringBuilder sb = new StringBuilder(777);
                sb.Append(req.CheckData?.Expburn == "pass" ? "" : "易燃易爆 ");
                sb.Append(req.CheckData?.Helmet == "pass" ? "" : "安全帽 ");
                sb.Append(req.CheckData?.Workshoe == "pass" ? "" : "劳保鞋 ");
                sb.Append(req.CheckData?.Smock == "pass" ? "" : "工作服 ");
                sb.Append(req.CheckData?.Tire == "pass" ? "" : "轮胎 ");

                //新增异常表数据
                Abnormal abnormal = new Abnormal
                {
                    Abnormaltype = "安检失败",
                    Abnormalname = user?.Userid,
                    Abnormalcase = $"司机:{ orderGoods?.Driver},车牌:{ orderGoods?.Tractorid},检查到{sb}不合格"
                };

                AbnormalSrvc.AddOrUpdate(abnormal, ADD);
            }

            if (req.CheckData.IsAllPass && Gate.GateFlag)
            {
                orderGoods.Safecheckflag = "1";
                OrderGoodsSrvc.AddOrUpdate(orderGoods, UPD, user.Userid);

                if (!dicOrderGoods.ContainsKey(orderGoods.Tractorid))
                {
                    (orderGoods).Debug("把" + orderGoods.Tractorid + "预约信息写入到内存", Exit);
                    dicOrderGoods.TryAdd(orderGoods.Tractorid, orderGoods);
                }

                if (!GateSrvc.GetCarInfos(new Model.Int.Gate.GateGetCarInfoReq() { PlateNo = orderGoods.Tractorid.Trim() }))
                {
                    GateSrvc.AddCarInfo(new Model.Int.Gate.GateAddCarReq() { PlateNo = orderGoods.Tractorid.Trim(), CarType = 2 });
                }

                if (Gate.GateSouthFlag)
                {
                    GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                    {
                        EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(Gate.EnterTimeSpan)),
                        Money = "0.00",
                        ParkUuid = Gate.GateSouthUuid, // 南门出入口
                        PlateNo = orderGoods.Tractorid.Trim(),
                        Remark = Gate.AppRemark,
                        // 防止手动开闸的问题,充值开始时间小于入场时间
                        StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddDays(-15.0))
                    });
                }

                GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                {
                    EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(Gate.EnterTimeSpan)),
                    Money = "0.00",
                    ParkUuid = Gate.GateNorthUuid, // 北门出入口
                    PlateNo = orderGoods.Tractorid.Trim(),
                    Remark = Gate.AppRemark,
                    // 防止手动开闸的问题,充值开始时间小于入场时间
                    StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddDays(-15.0))
                });
            }

            if (req.CheckData.IsAllPass && Weigh.WeighFlag)
            {
                // 通知地磅系统
                var dpext = new Yongrong.Model.Int.Weigh.DataParamExt()
                {
                    Driver = orderGoods.Driver,
                    Endtime = DateTime.Now.AddHours(Gate.EnterTimeSpan + Weigh.WeighDelayHour).ToString("yyyy-MM-dd HH:mm:ss"),
                    Msg_id = DateTime.Now.Ticks.ToString(),
                    Order_id = orderGoods.Orderid,
                    Order_type = orderGoods.IsTakeGoods ? "提货" : "送货",
                    Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Tractor_id = orderGoods.Tractorid,
                    Trailer_id = orderGoods.Trailerid,
                    Weigh_time = "1"
                };

                dpext.Sign = BMD5.ToMD5String(dpext.Order_id + dpext.Msg_id + dpext.Starttime, 32);

                dpext.Debug();

                WeighbridgeSrvc.BatchInsert(dpext);
            }

            rsp.Stat = Success;

            //新增Pda日志
            PdaLog pdalog = new PdaLog
            {
                Orderid = orderGoods?.Orderid,
                Tractorid = orderGoods?.Tractorid,
                Operation = ConstPda.Submit,
                Rspcode = ConstPda.Success,
                Reqparameter = reqbytes?.B2String(),
                Rspmessage = "Stat:" + rsp?.Stat + ",Msg:" + rsp?.Msg
            };

            PdaLogSrvc.AddOrUpdate(pdalog, ADD);

            user.Info("用户提交请求成功");

            reqmap.TryAdd(OP_User, user?.Userid);
            reqmap.TryAdd(OP_Content, "PDA车辆安全信息提交");

            return rsp.ToBytes();
        }
    }
}
